using WpfUserControlLib.Enumerations;

namespace WpfUserControlLib.DataModels {

    public class UIntEditStatusInfo {
        public UintEditStatus Status { get; set; } = UintEditStatus.UnhandledError;
        public string ExtraInfo { get; set; } = string.Empty;
        public UIntEditStatusInfo() { }
        public UIntEditStatusInfo(UintEditStatus status, string err) {
            this.Status = status;
            this.ExtraInfo = err;
        }

    }

}
