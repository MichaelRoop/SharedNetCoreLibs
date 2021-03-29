using LanguageFactory.Net.data;
using LanguageFactory.Net.interfaces;
using LanguageFactory.Net.Languages.en;
using LanguageFactory.Net.Messaging;
using System;
using System.Collections.Generic;
using System.Text;

namespace WpfCustomControlLib.Core.Helpers {

    public static class CustomTxtBinder {

        #region Data

        private static ILangFactory factory = new SupportedLanguageFactory();
        private static object lockObj = new object();

        #endregion

        /// <summary>Replace default english supported language with current language factory</summary>
        ///<param name="currentFactory">The factory to set</param>
        public static void SetLanguageFactory(ILangFactory newFactory) {
            lock (lockObj) {
                factory = newFactory;
            }
        }


        #region Messages

        public static string OK { get { return GetTxt(MsgCode.Ok); } }
        public static string Cancel { get { return GetTxt(MsgCode.cancel); } }
        public static string Copy { get { return GetTxt(MsgCode.copy); } }
        public static string Email { get { return GetTxt(MsgCode.email); } }
        public static string Yes { get { return GetTxt(MsgCode.yes); } }
        public static string No { get { return GetTxt(MsgCode.no); } }
        public static string Continue { get { return GetTxt(MsgCode.Continue); } }
        public static string Delete { get { return GetTxt(MsgCode.Delete); } }
        public static string Save { get { return GetTxt(MsgCode.save); } }
        public static string Language { get { return GetTxt(MsgCode.language); } }
        public static string CrashReport { get { return GetTxt(MsgCode.CrashReport); } }

        #endregion

        #region Private

        private static string GetTxt(MsgCode code) {
            try {
                lock (lockObj) {
                    return factory.CurrentLanguage.GetText(code);
                }
            }
            catch (Exception) {
                return "ERR";
            }
        }

        #endregion

    }

}
