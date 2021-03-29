using ChkUtils.Net;
using ChkUtils.Net.ErrObjects;
using LogUtils.Net;
using System;
using System.Text;
using System.Windows;
using WpfCustomControlLib.Core.Helpers;
using WpfHelperClasses.Core;

namespace WpfCustomControlLib.Core.UtilWindows {

    /// <summary>Logic for CrashReport.xaml</summary>
    public partial class CrashReport : Window {

        #region Data and events

        private string buildNumber = "2021.01.01.00";
        private string appName = "UNKNOWN APP";
        private Window parent = null;
        private ButtonGroupSizeSyncManager buttonWidthManager = null;

        #endregion

        #region Constructors and window events


        public static void ShowBox(Exception ex, Window parent, string appName) {
            try {
                CrashReport win = new CrashReport(ex, parent, appName);
                win.ShowDialog();
            }
            catch (Exception e) {
                Log.Exception(9999, "CrashReport", "ShowBox", e);
            }
        }

        public static void ShowBox(ErrReport report, Window parent, string appName) {
            CrashReport win = new CrashReport(report, parent, appName);
            win.ShowDialog();
        }



        public CrashReport(Exception ex, Window parent, string appName) {
            InitializeComponent();
            this.appName = appName;
            this.ProcessException(ex);
            this.buttonWidthManager = new ButtonGroupSizeSyncManager(this.btnCopy, this.btnCancel, this.btnEmail);
            this.buttonWidthManager.PrepForChange();
        }


        public CrashReport(ErrReport report, Window parent, string appName) {
            this.parent = parent;
            this.appName = appName;
            InitializeComponent();
            this.ProcessException(report);
            this.buttonWidthManager = new ButtonGroupSizeSyncManager(this.btnCopy, this.btnCancel, this.btnEmail);
            this.buttonWidthManager.PrepForChange();
        }


        public override void OnApplyTemplate() {
            this.BindMouseDownToCustomTitleBar();
            base.OnApplyTemplate();
        }


        private void Window_Loaded(object sender, RoutedEventArgs e) {
            this.SizeToContent = SizeToContent.WidthAndHeight;
            if (this.parent != null) {
                this.CenterToParent(this.parent);
            }
        }


        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e) {
            this.buttonWidthManager.Teardown();
        }

        #endregion

        #region Button handlers

        private void btnCancel_Click(object sender, RoutedEventArgs e) {
            this.Close();
        }

        private void btnCopy_Click(object sender, RoutedEventArgs e) {
            try {
                this.errBox.SelectAll();
                this.errBox.Copy();
                StringBuilder sb = new StringBuilder();
                sb.Append("          Date: ").Append(DateTime.Now.ToLongDateString()).AppendLine("")
                    .Append(" Windows Ver: ").Append(Environment.OSVersion.VersionString).AppendLine("")
                    .Append("    App Name: ").Append(this.appName).AppendLine("")
                    .Append("Build number: ").Append(this.buildNumber).AppendLine("").AppendLine("")
                    .Append(Clipboard.GetText());
                Clipboard.Clear();
                Clipboard.SetText(sb.ToString());
                this.errBox.Select(0, 0);
                this.Close();
            }
            catch (Exception ex) {
                Log.Exception(8888, "btnCopy_Click", "", ex);
                this.Close();
            }
        }


        private void btnEmail_Click(object sender, RoutedEventArgs e) {
            Dispatcher.Invoke(async () => {
                try {
                    string body = this.errBox.Text.Replace("\r\n", "%0d%0A");
                    StringBuilder sb = new StringBuilder();
                    sb.Append("mailto:MultiCommTerminal@gmail.com")
                        .Append(string.Format("?subject={0} CRASH REPORT", this.appName))
                        .Append("&body=")
                        .Append("Date: ").Append(DateTime.Now.ToLongDateString()).Append("%0d%0A")
                        .Append("Windows Ver: ").Append(Environment.OSVersion.VersionString).Append("%0d%0A")
                        .Append("App Name: ").Append(this.appName).Append("%0d%0A")
                        .Append("Build number: ").Append(this.buildNumber).Append("%0d%0A").Append("%0d%0A")
                        .Append(body).Append("%0d%0A");
                    // Note: requires Microsoft.SDK.Contracts loaded via NuGet
                    await Windows.System.Launcher.LaunchUriAsync(new Uri(sb.ToString()));
                    this.Close();
                }
                catch (Exception ex) {
                    Log.Exception(8888, "btnEmail_Click", "", ex);
                    this.Close();
                }
            });
        }

        #endregion

        #region Private

        private void ProcessException(Exception e) {
            try {
                if (e != null) {
                    ErrReportException erex = e as ErrReportException;
                    ErrReport report = null;
                    if (erex != null) {
                        report = erex.Report;
                    }
                    else {
                        report = WrapErr.GetErrReport(0, e.Message, e);
                    }
                    this.errBox.Text = string.Format(
                        "{0}  {1}:{2}\r\n{3}\r\n{4}",
                        report.Code, report.AtClass, report.AtMethod, report.Msg, report.StackTrace);
                }
                else {
                    this.errBox.Text = "Null exception. No info";
                }
            }
            catch (Exception) {
                this.errBox.Text = "Failed to populate";
            }
        }


        private void ProcessException(ErrReport report) {
            try {
                if (report != null) {
                    this.errBox.Text = string.Format("{0}\r\n{1}", report.Msg, report.StackTrace);
                }
                else {
                    this.errBox.Text = "Null exception. No info";
                }
            }
            catch (Exception) {
                this.errBox.Text = "Failed to populate";
            }
        }

        #endregion

    }
}
