using CommunicationStack.Net.DataModels;
using CommunicationStack.Net.Enumerations;
using CommunicationStack.Net.interfaces;
using LogUtils.Net;
using System;
using System.Threading;
using System.Threading.Tasks;
using VariousUtils.Net;
using Windows.Networking;
using Windows.Networking.Sockets;
using Windows.Storage.Streams;

namespace Communications.UWP.Core.MsgPumps {
    public abstract class SocketMsgPumpBase : IMsgPump<SocketMsgPumpConnectData> {

        #region Data

        private ClassLog log = new ClassLog("SocketMsgPumpBase");
        private static bool continueReading = false;
        private static uint readBufferMaxSizer = 256;
        private static bool isConnected = false;

        #endregion

        #region Properties
        public bool Connected {
            get {
                return isConnected;
            }
            private set {
                isConnected = value;
            }
        }

        #endregion

        #region Event Handlers 

        public event EventHandler<MsgPumpResults> MsgPumpConnectResultEvent;
        public event EventHandler<byte[]> MsgReceivedEvent;

        #endregion

        public SocketMsgPumpBase() { }



        public void ConnectAsync(SocketMsgPumpConnectData paramsObj) {
            this.TearDown(true);
            Task.Run(async () => {
                try {
                    this.log.InfoEntry("ConnectAsync");
                    this.log.Info("ConnectAsync", () => string.Format(
                        "Host:{0} Service:{1}", paramsObj.RemoteHostName, paramsObj.ServiceName));

                    readBufferMaxSizer = paramsObj.MaxReadBufferSize;
                    StreamSocket socket = this.SetSocket(new StreamSocket());
                    await socket.ConnectAsync(
                        new HostName(paramsObj.RemoteHostName),
                        paramsObj.ServiceName,
                        paramsObj.ProtectionLevel);

                    StreamSocketInformation i = socket.Information;
                    this.log.Info("ConnectAsync", () => string.Format(
                        "Connected to socket Local {0}:{1} Remote {2}:{3} - {4} : Protection:{5}",
                        i.LocalAddress, i.LocalPort,
                        i.RemoteHostName, i.RemotePort, i.RemoteServiceName, i.ProtectionLevel));

                    DataWriter writer = this.SetWriter(new DataWriter(socket.OutputStream));
                    writer.UnicodeEncoding = Windows.Storage.Streams.UnicodeEncoding.Utf8;

                    DataReader reader = this.SetReader(new DataReader(socket.InputStream));
                    reader.InputStreamOptions = InputStreamOptions.Partial;
                    reader.UnicodeEncoding = Windows.Storage.Streams.UnicodeEncoding.Utf8;
                    reader.ByteOrder = ByteOrder.LittleEndian;

                    CancellationTokenSource readCancelationToken = this.SetCancelToken(new CancellationTokenSource());
                    readCancelationToken.Token.ThrowIfCancellationRequested();
                    continueReading = true;

                    this.Connected = true;
                    this.LaunchReadTask();

                    this.MsgPumpConnectResultEvent?.Invoke(this, new MsgPumpResults(MsgPumpResultCode.Connected));
                }
                catch (Exception e) {
                    this.log.Exception(9999, "Connect Asyn Error", e);
                    this.MsgPumpConnectResultEvent?.Invoke(this, new MsgPumpResults(MsgPumpResultCode.ConnectionFailure));
                }
            });

        }

        public void Disconnect() {
            this.TearDown(false);
        }

        public void WriteAsync(byte[] msg) {
            if (this.Connected) {
                if (this.GetSocket() != null) {
                    Task.Run(async () => {
                        try {
                            this.log.Info("WriteAsync", () =>
                                string.Format("Sent:{0}", msg.ToFormatedByteString()));

                            this.GetWriter().WriteBytes(msg);
                            await this.GetSocket().OutputStream.WriteAsync(this.GetWriter().DetachBuffer());
                            await this.GetSocket().OutputStream.FlushAsync();
                        }
                        catch (Exception e) {
                            this.log.Exception(9999, "", e);
                        }
                    });
                }
                else {
                    this.log.Error(9999, "Socket is null");
                }
            }
            else {
                // TODO - add events for error
                this.log.Error(9999, "Not Connected");
            }
        }

        #region Abstract methods

        protected abstract StreamSocket GetSocket();
        protected abstract DataWriter GetWriter();
        protected abstract DataReader GetReader();
        protected abstract CancellationTokenSource GetCancelToken();
        protected abstract ManualResetEvent GetReadFinishEvent();

        protected abstract StreamSocket SetSocket(StreamSocket socket);
        protected abstract DataWriter SetWriter(DataWriter writer);
        protected abstract DataReader SetReader(DataReader reader);
        protected abstract CancellationTokenSource SetCancelToken(CancellationTokenSource tokenSource);

        #endregion

        #region Private

        private void LaunchReadTask() {
            Task.Run(async () => {
                this.log.InfoEntry("DoReadTask +++");
                this.GetReadFinishEvent().Reset();
                DataReader reader = this.GetReader();
                while (continueReading) {
                    try {
                        int count = (int)await reader.LoadAsync(readBufferMaxSizer).AsTask(this.GetCancelToken().Token);
                        if (count > 0) {
                            this.log.Error(9, "received");
                            if (count > 0) {
                                byte[] tmpBuff = new byte[count];
                                reader.ReadBytes(tmpBuff);
                                this.HandlerMsgReceived(this, tmpBuff);
                            }
                        }
                    }
                    catch (TaskCanceledException) {
                        this.log.Info("DoReadTask", "Cancelation");
                        break;
                    }
                    catch (Exception e) {
                        this.log.Exception(9999, "", e);
                        break;
                    }
                }
                this.log.InfoExit("DoReadTask ---");
                this.GetReadFinishEvent().Set();
            });
        }


        // TODO - probably make async task so it can be awaited
        private void TearDown(bool sleepAfterSocketDispose) {
            try {
                #region Cancel Read Thread
                continueReading = false;
                CancellationTokenSource tk = this.GetCancelToken();
                if (tk != null) {
                    tk.Cancel();
                    tk.Dispose();
                    this.SetCancelToken(null);
                    if (!this.GetReadFinishEvent().WaitOne(2000)) {
                        this.log.Error(9999, "Timed out waiting for read cancelation");
                    }
                }
                #endregion

                #region Close Writer and Reader
                DataWriter w = this.GetWriter();
                if (w != null) {
                    try { 
                        w.DetachStream();
                    }
                    catch (Exception e) { 
                        this.log.Exception(9999, "", e); 
                    }
                    w.Dispose();
                    this.SetWriter(null);
                }

                DataReader r = this.GetReader();
                if (r != null) {
                    try { 
                        r.DetachStream(); 
                    }
                    catch (Exception e) { 
                        this.log.Exception(9999, "", e); 
                    }
                    r.Dispose();
                    this.SetReader(null);
                }
                #endregion

                #region Close socket
                StreamSocket s = this.GetSocket();
                if (s != null) {
                    // The socket was closed so cannot cancel IO
                    s.Dispose();
                    this.SetSocket(null);
                    // Seems socket does not shut itself down fast enough before next call to connect
                    if (sleepAfterSocketDispose) {
                        Thread.Sleep(500);
                    }
                }
                #endregion
                this.Connected = false;
            }
            catch (Exception e) {
                this.log.Exception(9999, "", e);
            }

        }

        private void HandlerMsgReceived(object sender, byte[] msg) {
            this.log.Info("HandlerMsgReceived", () => string.Format("Received:{0}", msg.ToFormatedByteString()));
            try {
                this.MsgReceivedEvent?.Invoke(sender, msg);
            }
            catch (Exception e) {
                this.log.Exception(9999, "", e);
            }
        }

        #endregion

    }
}
