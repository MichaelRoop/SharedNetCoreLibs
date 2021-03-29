namespace WpfCustomControlLib.Core.Helpers {

    public static class CustomIconBinder {

        public static string Add { get { return CustomIconBinder.GetIconSource("icons8-add-50-noborder.png"); } }
        public static string Cancel { get { return CustomIconBinder.GetIconSource("icons8-close-window-50-noborder.png"); } }
        public static string Email { get { return CustomIconBinder.GetIconSource("icons8-send-email-100.png"); } }
        public static string LanguageW { get { return CustomIconBinder.GetIconSource("icons8-language-white-50.png"); } }
        public static string No { get { return CustomIconBinder.Cancel; } }
        public static string OK { get { return CustomIconBinder.Yes; } }
        public static string Yes { get { return CustomIconBinder.GetIconSource("icons8-checkmark-50.png"); } }


        private static string GetIconSource(string name) {
            return string.Format(@"WpfCustomControlLib.Core;component\Images\{0}", name);
        }

    }
}
