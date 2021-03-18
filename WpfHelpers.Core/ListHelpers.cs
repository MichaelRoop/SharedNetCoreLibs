using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows.Controls;

namespace WpfHelperClasses.Core {

    public static class ListHelpers {

        /// <summary> Force a list view to resize its columns when data changes</summary>
        /// <param name="listView">The ListView to resize</param>
        /// <remarks>
        /// https://stackoverflow.com/questions/18389675/autoresize-listview-columns-on-content-update
        /// </remarks>
        public static void ForceColumnResize(this ListView listView) {
            try {
                GridView gv = listView.View as GridView;
                if (gv != null) {
                    foreach (var c in gv.Columns) {
                        if (double.IsNaN(c.Width)) {
                            c.Width = c.ActualWidth;
                        }
                        c.Width = double.NaN;
                    }
                }
            }
            catch(Exception e) {
                if (e != null && !string.IsNullOrEmpty(e.Message)) {
                    Debug.WriteLine(string.Format("ForceColumnResize: {0}", e.Message));
                }
                else {
                    Debug.WriteLine(string.Format("ForceColumnResize: Unknown exception"));
                }
            }
        }



    }

}
