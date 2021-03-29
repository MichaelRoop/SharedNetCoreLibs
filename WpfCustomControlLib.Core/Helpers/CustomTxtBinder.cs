using LanguageFactory.Net.data;
using LanguageFactory.Net.Languages.en;
using LanguageFactory.Net.Messaging;
using System;
using System.Collections.Generic;
using System.Text;

namespace WpfCustomControlLib.Core.Helpers {

    public static class CustomTxtBinder {

        #region Data

        /// <summary>
        /// Language module to use at design time because XAML designer cannot 
        /// access DI injector which is only initialised at runtime
        /// </summary>
        private static SupportedLanguage designLanguage = new English();

        private static Func<MsgCode, string> getMsgFunc = null;

        #endregion

        static CustomTxtBinder() {
            SetGetMsgFun(CustomTxtBinder.DefaultGetTxt);
        }


        /// <summary>Replace the default english supported language with current language</summary>
        /// <param name="func">The get message function to set</param>
        public static void SetGetMsgFun(Func<MsgCode, string> func) {
            CustomTxtBinder.getMsgFunc = func;
        }


        #region Messages

        public static string OK { get { return GetTxt(MsgCode.Ok); } }
        public static string Cancel { get { return GetTxt(MsgCode.cancel); } }
        public static string Yes { get { return GetTxt(MsgCode.yes); } }
        public static string No { get { return GetTxt(MsgCode.no); } }
        public static string Continue { get { return GetTxt(MsgCode.Continue); } }
        public static string Delete { get { return GetTxt(MsgCode.Delete); } }

        #endregion

        #region Private

        private static string GetTxt(MsgCode code) {
            try {
                return CustomTxtBinder.getMsgFunc(code);
            }
            catch (Exception) {
                return "ERR";
            }
        }


        private static string DefaultGetTxt(MsgCode code) {
            return designLanguage.GetText(code);
        }

        #endregion

    }

}
