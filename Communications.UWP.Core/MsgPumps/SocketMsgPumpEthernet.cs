using System.Threading;
using Windows.Networking.Sockets;
using Windows.Storage.Streams;

namespace Communications.UWP.Core.MsgPumps {

    /// <summary>
    /// Derived instance of SocketMsgPumpBase for Ethernet that passes its 
    /// static members to base to enable base to share with async methods
    /// </summary>
    public class SocketMsgPumpEthernet : SocketMsgPumpBase {

        #region static members

        private static CancellationTokenSource CANCEL_TOKEN = null;
        private static ManualResetEvent FINISH_READ_EVENT = new ManualResetEvent(false);

        #endregion

        #region SocketMsgPumpBase overrides for base to use its statics


        protected override ManualResetEvent ReadFinishEvent {
            get { return FINISH_READ_EVENT; }
        }


        protected override CancellationTokenSource CancelToken {
            get { return CANCEL_TOKEN; }
            set { CANCEL_TOKEN = value; }
        }

        #endregion

    }
}
