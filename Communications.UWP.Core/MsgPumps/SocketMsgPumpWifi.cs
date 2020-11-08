using System.Threading;
using Windows.Networking.Sockets;
using Windows.Storage.Streams;

namespace Communications.UWP.Core.MsgPumps {

    /// <summary>
    /// Derived instance of SocketMsgPumpBase for Wifi that passes its 
    /// static members to base to enable base to share with async methods
    /// </summary>
    public class SocketMsgPumpWifi : SocketMsgPumpBase {


        #region static members

        private static CancellationTokenSource CANCEL_TOKEN = null;
        private static ManualResetEvent FINISH_READ_EVENT = new ManualResetEvent(false);

        #endregion

        #region SocketMsgPumpBase overrides for base to use its statics

        protected override CancellationTokenSource GetCancelToken() {
            return CANCEL_TOKEN;
        }


        protected override ManualResetEvent GetReadFinishEvent() {
            return FINISH_READ_EVENT;
        }


        protected override CancellationTokenSource SetCancelToken(CancellationTokenSource tokenSource) {
            CANCEL_TOKEN = tokenSource;
            return CANCEL_TOKEN;
        }

        #endregion

    }
}
