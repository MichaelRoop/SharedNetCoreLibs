using System.Windows;
using System.Windows.Controls;

namespace WpfHelperClasses.Core {
    public static class WpfControlExtensions {

        #region Buttons

        #endregion

        #region Labels

        #endregion

        #region UIElement

        // Anything derived from UIElement, not just controls

        public static void Collapse(this UIElement uiElement) {
            uiElement.Visibility = Visibility.Collapsed;
        }

        public static void Show(this UIElement uiElement) {
            uiElement.Visibility = Visibility.Visible;
        }

        public static void Hide(this UIElement uiElement) {
            uiElement.Visibility = Visibility.Hidden;
        }


        #endregion

        #region Stack Panel


        #endregion

    }
}
