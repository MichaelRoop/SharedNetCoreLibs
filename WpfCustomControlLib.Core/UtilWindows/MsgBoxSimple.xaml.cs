using System;
using System.Windows;
using WpfCustomControlLib.Core.Helpers;
using WpfHelperClasses.Core;

namespace WpfCustomControlLib.Core.UtilWindows {

    /// <summary>Interaction logic for MsgBoxSimple.xaml</summary>
    public partial class MsgBoxSimple : Window {

        Window parent = null;

        #region Static methods

        public static void ShowBox(Window win, string msg) {
            win.Dispatcher.Invoke(() => {
                try {
                    MsgBoxSimple box = new MsgBoxSimple(win, msg);
                    box.ShowDialog();
                }
                catch (Exception) {}
            });
        }


        public static void ShowBox(Window win, string title, string msg) {
            win.Dispatcher.Invoke(() => {
                try {
                    MsgBoxSimple box = new MsgBoxSimple(win, title, msg);
                    box.ShowDialog();
                }
                catch (Exception) { }
            });
        }


        public static void ShowBox(string msg) {
            MsgBoxSimple box = new MsgBoxSimple(msg);
            box.ShowDialog();
        }


        public static void ShowBox(string title, string msg) {
            MsgBoxSimple box = new MsgBoxSimple(title, msg);
            box.ShowDialog();
        }

        #endregion

        #region Constructors

        public MsgBoxSimple() {
            InitializeComponent();
            this.SizeToContent = SizeToContent.WidthAndHeight;
        }


        public MsgBoxSimple(string msg) : this() {
            this.txtBlock.Text = msg;
        }


        public MsgBoxSimple(string title, string msg) : this(msg) {
            this.Title = title;
        }


        private MsgBoxSimple(Window parent) {
            InitializeComponent();
            this.SizeToContent = SizeToContent.WidthAndHeight;
        }


        public MsgBoxSimple(Window parent, string msg) : this(parent) {
            this.parent = parent;
            this.txtBlock.Text = msg;
        }


        public MsgBoxSimple(Window parent, string title, string msg) : this(parent, msg) {
            this.Title = title;
        }

        #endregion

        public override void OnApplyTemplate() {
            this.BindMouseDownToCustomTitleBar();
            this.HideTitleBarIcon();
            base.OnApplyTemplate();
        }


        private void Window_ContentRendered(object sender, EventArgs e) {
            if (parent != null) {
                this.CenterToParent(this.parent);
            }
        }


        private void btnOk_Click(object sender, RoutedEventArgs e) {
            this.Close();
        }
    }
}
