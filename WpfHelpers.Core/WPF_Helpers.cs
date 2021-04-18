using System;
using System.Windows;
using System.Windows.Controls;

namespace WpfHelperClasses.Core {

    public static class WPF_ControlHelpers {


        #region Window Helpers

        /// <summary>
        /// Centers child window over parent. Call at end of child WindowRendered event
        /// </summary>
        /// <param name="parent">The parent window</param>
        /// <param name="child">The child to center</param>
        public static void CenterChild(Window parent, Window child) {
            child.Left = parent.Left + ((parent.Width - child.ActualWidth) / 2.0);
            child.Top = parent.Top + ((parent.Height - child.ActualHeight) / 2.0);
        }


        /// <summary>Center the calling window to its parent</summary>
        /// <param name="child">The child to center</param>
        /// <param name="parent">The parent window</param>
        public static void CenterToParent( this Window child, Window parent) {
            CenterChild(parent, child);
        }


        /// <summary>Center the calling window over the user contol that opened it</summary>
        /// <param name="child">The window opening</param>
        /// <param name="parent">The user control doing the opening</param>
        public static void CenterToParent(this Window child, UserControl parent) {
            //https://stackoverflow.com/questions/44119226/wpf-set-dialog-window-position-relative-to-user-control
            Point locationFromScreen = parent.PointToScreen(new Point(0, 0));
            PresentationSource source = PresentationSource.FromVisual(parent);
            Point targetPoints = source.CompositionTarget.TransformFromDevice.Transform(
                locationFromScreen);
            Point focus = new Point();
            focus.X = targetPoints.X + (parent.Width / 2.0);
            focus.Y = targetPoints.Y + (parent.Height / 2.0);
            child.Top = focus.Y - (child.Height / 2.0);
            child.Left = focus.X - (child.Width / 2.0);

            if (child.Top < 0) {
                child.Top = 0;
            }
            if (child.Left < 0) {
                child.Left = 0;
            }

        }



        public static void CenterToParent(this Window child, UserControl parent, Window AppWindow) {
            child.CenterToParent(parent);
            double appBottom = AppWindow.Top + AppWindow.Height;
            double childBottom = child.Top + child.Height;
            if (childBottom > appBottom) {
                child.Top -= (childBottom - appBottom);
            }

            if (child.Left < 0) {
                child.Left = 0;
            }
        }




        #endregion


        /// <summary>Call at class startup before buttons are rendered and before content change</summary>
        /// <param name="buttons"></param>
        public static void ForceButtonMinMax(params Button[] buttons) {
            foreach (Button button in buttons) {
                button.MinWidth = 10;
                button.MaxWidth = 1000;
                button.Width = Double.NaN;
            }
        }


        /// <summary>Set the button array to width of widest button</summary>
        /// <remarks>
        /// To make this work call when all of the .SizeChanged of all the
        /// buttons have fired. 
        /// At the point that all have fired you need to disconnect the
        /// event to avoid the 1% added below to refire in an endless loop
        /// 
        /// Before content changes call the ForceButtonsMinMax so that this
        /// method will be able to properly resize buttons both ways
        /// 
        /// </remarks>
        /// <param name="buttons"></param>
        public static void ResizeToWidest(params Button[] buttons) {
            double maxWidth = 0;
            foreach (Button button in buttons) {
                maxWidth = Math.Max(maxWidth, button.ActualWidth);
            }

            foreach (Button button in buttons) {
                // If you set Width to other than NaN it turns off auto sizing
                // https://docs.microsoft.com/en-us/dotnet/api/system.windows.frameworkelement.width?view=netframework-4.8
                //button.Width = maxWidth;
                // The tiny % increase forces another redraw so you get the right size
                maxWidth += 1.0001;
                button.MaxWidth = maxWidth  + 1;
                button.MinWidth = maxWidth - 1;
            }
        }


    }
}
