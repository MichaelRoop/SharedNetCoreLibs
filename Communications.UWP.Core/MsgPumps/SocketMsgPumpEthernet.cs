using CommunicationStack.Net.DataModels;
using CommunicationStack.Net.Enumerations;
using CommunicationStack.Net.interfaces;
using LogUtils.Net;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using VariousUtils.Net;
using Windows.Networking;
using Windows.Networking.Sockets;
using Windows.Storage.Streams;

namespace Communications.UWP.Core.MsgPumps {

    public class SocketMsgPumpEthernet : SocketMsgPumpBase /*IMsgPump<SocketMsgPumpConnectData>*/ {

        #region static members

        private static StreamSocket SOCKET = null;
        private static DataWriter WRITER = null;
        private static DataReader READER = null;
        private static CancellationTokenSource CANCEL_TOKEN = null;
        private static ManualResetEvent FINISH_READ_EVENT = new ManualResetEvent(false);

        #endregion

        protected override CancellationTokenSource GetCancelToken() {
            return CANCEL_TOKEN;
        }

        protected override DataReader GetReader() {
            return READER;
        }

        protected override ManualResetEvent GetReadFinishEvent() {
            return FINISH_READ_EVENT;
        }

        protected override StreamSocket GetSocket() {
            return SOCKET;
        }

        protected override DataWriter GetWriter() {
            return WRITER;
        }

        protected override CancellationTokenSource SetCancelToken(CancellationTokenSource tokenSource) {
            CANCEL_TOKEN = tokenSource;
            return CANCEL_TOKEN;
        }

        protected override DataReader SetReader(DataReader reader) {
            READER = reader;
            return READER;
        }


        protected override StreamSocket SetSocket(StreamSocket socket) {
            SOCKET = socket;
            return SOCKET;
        }

        protected override DataWriter SetWriter(DataWriter writer) {
            WRITER = writer;
            return WRITER;
        }



        //#region Events

        //public event EventHandler<MsgPumpResults> MsgPumpConnectResultEvent;
        //public event EventHandler<byte[]> MsgReceivedEvent;

        //#endregion

        //#region Properties

        //public bool Connected { get; private set; } = false;

        //#endregion

        //#region Data

        //private ClassLog log = new ClassLog("SocketMsgPumpEthernet");
        //private static StreamSocket socket = null;
        //private static DataWriter writer = null;
        //private static DataReader reader = null;
        //private static CancellationTokenSource readCancelationToken = null;
        //private static bool continueReading = false;
        //private static uint readBufferMaxSizer = 256;
        //private static ManualResetEvent readFinishedEvent = new ManualResetEvent(false);

        //#endregion


        //public void ConnectAsync(SocketMsgPumpConnectData paramsObj) {
        //    this.TearDown(true);
        //    Task.Run(async () => {
        //        try {
        //            this.log.InfoEntry("ConnectAsync");
        //            this.log.Info("ConnectAsync", () => string.Format(
        //                "Host:{0} Service:{1}", paramsObj.RemoteHostName, paramsObj.ServiceName));

        //            readBufferMaxSizer = paramsObj.MaxReadBufferSize;
        //            socket = new StreamSocket();
        //            await socket.ConnectAsync(
        //                new HostName(paramsObj.RemoteHostName),
        //                paramsObj.ServiceName,
        //                paramsObj.ProtectionLevel);

        //            StreamSocketInformation i = socket.Information;
        //            this.log.Info("ConnectAsync", () => string.Format(
        //                "Connected to socket Local {0}:{1} Remote {2}:{3} - {4} : Protection:{5}",
        //                i.LocalAddress, i.LocalPort,
        //                i.RemoteHostName, i.RemotePort, i.RemoteServiceName, i.ProtectionLevel));

        //            writer = new DataWriter(socket.OutputStream);
        //            writer.UnicodeEncoding = Windows.Storage.Streams.UnicodeEncoding.Utf8;

        //            reader = new DataReader(socket.InputStream);
        //            reader.InputStreamOptions = InputStreamOptions.Partial;
        //            reader.UnicodeEncoding = Windows.Storage.Streams.UnicodeEncoding.Utf8;
        //            reader.ByteOrder = ByteOrder.LittleEndian;

        //            readCancelationToken = new CancellationTokenSource();
        //            readCancelationToken.Token.ThrowIfCancellationRequested();
        //            continueReading = true;

        //            this.Connected = true;
        //            this.LaunchReadTask();

        //            this.MsgPumpConnectResultEvent?.Invoke(this, new MsgPumpResults(MsgPumpResultCode.Connected));
        //        }
        //        catch (Exception e) {
        //            this.log.Exception(9999, "Connect Asyn Error", e);
        //            this.MsgPumpConnectResultEvent?.Invoke(this, new MsgPumpResults(MsgPumpResultCode.ConnectionFailure));
        //        }
        //    });
        //}

        //public void Disconnect() {
        //    this.TearDown(false);
        //}

        //public void WriteAsync(byte[] msg) {
        //    if (this.Connected) {
        //        if (socket != null) {
        //            Task.Run(async () => {
        //                try {
        //                    this.log.Info("WriteAsync", () =>
        //                        string.Format("Sent:{0}", msg.ToFormatedByteString()));

        //                    writer.WriteBytes(msg);
        //                    await socket.OutputStream.WriteAsync(writer.DetachBuffer());
        //                }
        //                catch (Exception e) {
        //                    this.log.Exception(9999, "", e);
        //                }
        //            });
        //        }
        //        else {
        //            this.log.Error(9999, "Socket is null");
        //        }
        //    }
        //    else {
        //        // TODO - add events for error
        //        this.log.Error(9999, "Not Connected");
        //    }
        //}



        //private void LaunchReadTask() {
        //    Task.Run(async () => {
        //        this.log.InfoEntry("DoReadTask +++");
        //        readFinishedEvent.Reset();

        //        while (continueReading) {
        //            try {
        //                int count = (int)await reader.LoadAsync(readBufferMaxSizer).AsTask(readCancelationToken.Token);

        //                this.log.Error(9, "received");

        //                if (count > 0) {
        //                    byte[] tmpBuff = new byte[count];
        //                    reader.ReadBytes(tmpBuff);
        //                    this.HandlerMsgReceived(this, tmpBuff);
        //                }
        //            }
        //            catch (TaskCanceledException) {
        //                this.log.Info("DoReadTask", "Cancelation");
        //                break;
        //            }
        //            catch (Exception e) {
        //                this.log.Exception(9999, "", e);
        //                break;
        //            }
        //        }
        //        this.log.InfoExit("DoReadTask ---");
        //        readFinishedEvent.Set();
        //    });
        //}


        //private void HandlerMsgReceived(object sender, byte[] msg) {
        //    this.log.Info("HandlerMsgReceived", () => string.Format("Received:{0}", msg.ToFormatedByteString()));
        //    try {
        //        this.MsgReceivedEvent?.Invoke(sender, msg);
        //    }
        //    catch (Exception e) {
        //        this.log.Exception(9999, "", e);
        //    }
        //}


        ///// <summary>Tear down any connections, dispose and reset all resources</summary>
        //private void TearDown(bool sleepAfterSocketDispose) {
        //    try {
        //        #region Cancel Read Thread
        //        continueReading = false;
        //        if (readCancelationToken != null) {
        //            readCancelationToken.Cancel();
        //            readCancelationToken.Dispose();
        //            readCancelationToken = null;
        //            if (!readFinishedEvent.WaitOne(2000)) {
        //                this.log.Error(9999, "Timed out waiting for read cancelation");
        //            }
        //        }
        //        #endregion

        //        #region Close Writer and Reader
        //        if (writer != null) {
        //            try { writer.DetachStream(); }
        //            catch (Exception e) { this.log.Exception(9999, "", e); }
        //            writer.Dispose();
        //            writer = null;
        //        }

        //        if (reader != null) {
        //            try { reader.DetachStream(); }
        //            catch (Exception e) { this.log.Exception(9999, "", e); }
        //            reader.Dispose();
        //            reader = null;
        //        }
        //        #endregion

        //        #region Close socket
        //        if (socket != null) {
        //            // The socket was closed so cannot cancel IO
        //            socket.Dispose();
        //            socket = null;
        //            // Seems socket does not shut itself down fast enough before next call to connect
        //            if (sleepAfterSocketDispose) {
        //                Thread.Sleep(500);
        //            }
        //        }
        //        #endregion
        //        this.Connected = false;
        //    }
        //    catch (Exception e) {
        //        this.log.Exception(9999, "", e);
        //    }
        //}
    }
}
