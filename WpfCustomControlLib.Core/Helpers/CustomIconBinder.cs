namespace WpfCustomControlLib.Core.Helpers {

    public static class CustomIconBinder {

        public static string OK { get { return CustomIconBinder.YES; } }
        public static string YES { get { return CustomIconBinder.GetIconSource("icons8-checkmark-50.png"); } }
        public static string NO { get { return CustomIconBinder.CANCEL; } }
        public static string CANCEL { get { return CustomIconBinder.GetIconSource("icons8-close-window-50-noborder.png"); } }


        private static string GetIconSource(string name) {
            return string.Format(@"WpfCustomControlLib.Core;component\Images\{0}", name);
        }

    }
}
