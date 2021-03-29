using System;
using System.Windows;
using WpfCustomControlLib.Core.Helpers;
using WpfHelperClasses.Core;

namespace WpfCustomControlLib.Core.UtilWindows {

    /// <summary>logic for MsgBoxEnterText.xaml</summary>
    public partial class MsgBoxEnterText : Window {

        #region Data

        private Window parent = null;
        private ButtonGroupSizeSyncManager widthManager = null;

        #endregion

        #region Data types

        /// <summary>Return values from this message box</summary>
        public enum MsgBoxTextInputResult {
            OK,
            Cancel,
        }

        public class MsgBoxTextInputData {
            public MsgBoxTextInputResult Result { get; set; } = MsgBoxTextInputResult.Cancel;
            public string Text { get; set; } = string.Empty;
        }

        #endregion

        #region Properties

        public MsgBoxTextInputData Result { get; private set; } = new MsgBoxTextInputData();

        #endregion

        #region Static methods

        public static MsgBoxTextInputData ShowBox(Window win, string title, string msg, string defaultTxt = "") {
            MsgBoxEnterText box = new MsgBoxEnterText(win, title, msg, defaultTxt);
            box.ShowDialog();
            return box.Result;
        }

        #endregion

        #region Constructors

        public MsgBoxEnterText(Window parent, string title, string msg, string defaultTxt = "") {
            InitializeComponent();
            this.Title = title;
            this.txtBlock.Text = msg;
            this.txtInput.Text = defaultTxt;
            this.widthManager = new ButtonGroupSizeSyncManager(this.btnOk, this.btnCancel);
            this.widthManager.PrepForChange();
        }

        #endregion

        #region Windows events

        public override void OnApplyTemplate() {
            this.BindMouseDownToCustomTitleBar();
            this.HideTitleBarIcon();
            base.OnApplyTemplate();
        }


        private void Window_Loaded(object sender, RoutedEventArgs e) {
            this.SizeToContent = SizeToContent.WidthAndHeight;
            if (parent != null) {
                this.CenterToParent(this.parent);
            }
        }


        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e) {
            this.widthManager.Teardown();
        }

        #endregion

        #region Button events

        private void btnOk_Click(object sender, RoutedEventArgs e) {
            this.Result.Result = MsgBoxTextInputResult.OK;
            this.Result.Text = this.txtInput.Text;
            this.Close();
        }


        private void btnCancel_Click(object sender, RoutedEventArgs e) {
            this.Close();
        }

        #endregion

    }
}
