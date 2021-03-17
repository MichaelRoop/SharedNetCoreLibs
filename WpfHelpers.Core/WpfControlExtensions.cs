using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace WpfHelperClasses.Core {
    public static class WpfControlExtensions {

        #region Buttons

        #endregion

        #region Labels

        #endregion

        #region UIElement

        // Anything derived from UIElement, not just controls

        public static void Collapse(this UIElement uiElement) {
            if (uiElement != null) {
                uiElement.Visibility = Visibility.Collapsed;
            }
        }

        public static void Show(this UIElement uiElement) {
            if (uiElement != null) {
                uiElement.Visibility = Visibility.Visible;
            }
        }

        public static void Hide(this UIElement uiElement) {
            if (uiElement != null) {
                uiElement.Visibility = Visibility.Hidden;
            }
        }


        public static void ToggleVisibility(this UIElement uiElement) {
            if (uiElement != null) {
                if (uiElement.Visibility == Visibility.Collapsed ||
                    uiElement.Visibility == Visibility.Hidden) {
                    uiElement.Show();
                }
                else {
                    uiElement.Collapse();
                }
            }
            else {
                // Provoke exception
                uiElement.Visibility = Visibility.Visible;
            }
        }


        public static void SetVisualEnabled(this UIElement uIElement, bool isEnabled, double opacity = 0.4) {
            if (uIElement != null) {
                uIElement.IsEnabled = isEnabled;
                if (!isEnabled) {
                    uIElement.Opacity = opacity;
                }
            }
        }


        public static void Enable(this UIElement uIElement) {
            if (uIElement != null) {
                uIElement.IsEnabled = true;
                uIElement.Opacity = 1.0;
            }
        }


        public static void Disable(this UIElement uIElement, double opacity = 0.4) {
            if (uIElement != null) {
                uIElement.IsEnabled = false;
                uIElement.Opacity = opacity;
            }
        }

        #endregion

        #region Selector based such as ListBox, ComboBox

        /// <summary>Get the selector's scroll viewer</summary>
        /// <param name="selector">The selector to access scroll viewer</param>
        /// <returns>The scroll viewer</returns>
        public static ScrollViewer GetScrollViewer(this Selector selector) {
            Border b = (Border)VisualTreeHelper.GetChild(selector, 0);
            return (ScrollViewer)VisualTreeHelper.GetChild(b, 0);
        }


        /// <summary>Add an item to the selector list</summary>
        /// <typeparam name="T">The type to add</typeparam>
        /// <param name="selector">The target list</param>
        /// <param name="source">The source list to attach</param>
        /// <param name="item">The item to add</param>
        public static void Add<T>(this Selector selector, List<T> source, T item) {
            if (selector != null) {
                selector.ItemsSource = null;
                source.Add(item);
                selector.ItemsSource = source;
            }
        }


        /// <summary>Add items to the selector list</summary>
        /// <typeparam name="T">The type to add</typeparam>
        /// <param name="selector">The target list</param>
        /// <param name="source">The source list to attach</param>
        /// <param name="item">The list of items to add</param>
        public static void Add<T>(this Selector selector, List<T> source, List<T> items) {
            if (selector != null) {
                selector.ItemsSource = null;
                foreach (T item in items) {
                    source.Add(item);
                }
                selector.ItemsSource = source;
            }
        }


        /// <summary>Add an item to a list and scroll to bottom while removing those over max</summary>
        /// <typeparam name="T">The type to add</typeparam>
        /// <param name="selector">The selector object</param>
        /// <param name="item">The item to add</param>
        /// <param name="scrollViewer">The list scroll viewer</param>
        /// <param name="maxItems">Maximum items allowed before removing oldest</param>
        public static void AddAndScroll<T>(this Selector selector, T item, ScrollViewer scrollViewer, int maxItems) {
            if (selector != null) {
                if (maxItems > 0) {
                    if (selector.Items.Count > maxItems) {
                        selector.Items.RemoveAt(0);
                    }
                    selector.Items.Add(item);
                    scrollViewer.ScrollToBottom();
                }
            }
        }


        public static void Clear<T>(this Selector selector, List<T> source) {
            if (selector != null) {
                selector.ItemsSource = null;
                source.Clear();
                selector.ItemsSource = source;
            }
        }


        public static void SetNewSource<T>(this Selector selector, ref List<T> source, List<T> newSource) {
            if (selector != null) {
                selector.ItemsSource = null;
                source = newSource;
                selector.ItemsSource = source;
            }
        }


        /// <summary>Get the selected item of type T from the Selector derived controls</summary>
        /// <typeparam name="T">The type to select</typeparam>
        /// <param name="selector">The target ListBox or other</param>
        /// <param name="onFound">Invoked when selected item found</param>
        /// <param name="onNone">Invoked when none selected or wrong type</param>
        public static void GetSelected<T>(this Selector selector, Action<T> onFound, Action onNone) where T:class {
            if (selector != null) {
                T item = selector.SelectedItem as T;
                if (item != null) {
                    onFound.Invoke(item);
                }
                else {
                    onNone.Invoke();
                }
            }
        }


        /// <summary>Get the selected item of type T from the Selector derived controls</summary>
        /// <typeparam name="T">The type to select</typeparam>
        /// <param name="selector">The target ListBox or other</param>
        /// <param name="onFound">Invoked when selected item found</param>
        public static void GetSelected<T>(this Selector selector, Action<T> onFound) where T : class {
            if (selector != null) {
                selector.GetSelected<T>((item) => {
                    onFound.Invoke(item);
                },
                () => {
                    // Don't care
                });
            }
        }


        /// <summary>Safe count of items in selector</summary>
        /// <param name="selector">Selector derived objects such as List</param>
        /// <returns>The number of items. 0 if not initialized</returns>
        public static int Count(this Selector selector) {
            if (selector != null && selector.Items != null) {
                return selector.Items.Count;
            }
            return 0;
        }

        #endregion


        //// Unfortunately it also turns off snap size max
        /// and does not get rid of the exta top border
        //private const int GWL_STYLE = -16, WS_MAXIMIZEBOX = 0x10000, WS_MINIMIZEBOX = 0x20000;

        //[DllImport("user32.dll")]
        //extern private static int GetWindowLong(IntPtr hwnd, int index);

        //[DllImport("user32.dll")]
        //extern private static int SetWindowLong(IntPtr hwnd, int index, int value);


        ///// <summary>
        ///// Hides the Minimize and Maximize buttons in a Window. Must be called in the constructor.
        ///// </summary>
        ///// <param name="window">The Window whose Minimize/Maximize buttons will be hidden.</param>
        //public static void HideMinimizeAndMaximizeButtons(this Window window) {
        //    window.SourceInitialized += (s, e) => {
        //        IntPtr hwnd = new System.Windows.Interop.WindowInteropHelper(window).Handle;
        //        int currentStyle = GetWindowLong(hwnd, GWL_STYLE);

        //        SetWindowLong(hwnd, GWL_STYLE, currentStyle & ~WS_MAXIMIZEBOX & ~WS_MINIMIZEBOX);
        //    };
        //}



    }
}
