using System;
using System.Windows.Controls;

namespace WpfHelperClasses.Core {

    public static class WPF_ControlHelpers {

        public static void ResizeToWidest(params Button[] buttons) {
            double maxWidth = 0;
            foreach (Button button in buttons) {
                maxWidth = Math.Max(maxWidth, button.ActualWidth);
            }

            foreach (Button button in buttons) {
                button.Width = maxWidth;
            }
        }

    }
}
