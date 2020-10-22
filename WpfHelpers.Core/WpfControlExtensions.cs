using System;
using System.Collections.Generic;
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

    }
}
