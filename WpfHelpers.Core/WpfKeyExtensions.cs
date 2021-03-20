using System.Windows.Input;

namespace WpfHelperClasses.Core {

    public static class WpfKeyExtensions {


        /// <summary>Any numeric value from 0-9, keyboard or keypad</summary>
        /// <param name="key">The key press to evaluate</param>
        /// <returns>true if value 0-9, otherwise false</returns>
        public static bool IsNumeric(this Key key) {
            switch (key) {
                case Key.D0:
                case Key.D1:
                case Key.D2:
                case Key.D3:
                case Key.D4:
                case Key.D5:
                case Key.D6:
                case Key.D7:
                case Key.D8:
                case Key.D9:
                case Key.NumPad0:
                case Key.NumPad1:
                case Key.NumPad2:
                case Key.NumPad3:
                case Key.NumPad4:
                case Key.NumPad5:
                case Key.NumPad6:
                case Key.NumPad7:
                case Key.NumPad8:
                case Key.NumPad9:
                    return true;
                default:
                    return false;
            }
        }


        /// <summary>Any numeric value from 2-9</summary>
        /// <param name="key">The key press to evaluate</param>
        /// <returns>true if value 2-9, otherwise false</returns>
        public static bool IsNonBinaryNumeric(this Key key) {
            switch (key) {
                case Key.D2:
                case Key.D3:
                case Key.D4:
                case Key.D5:
                case Key.D6:
                case Key.D7:
                case Key.D8:
                case Key.D9:
                case Key.NumPad2:
                case Key.NumPad3:
                case Key.NumPad4:
                case Key.NumPad5:
                case Key.NumPad6:
                case Key.NumPad7:
                case Key.NumPad8:
                case Key.NumPad9:
                    return true;
                default:
                    return false;
            }
        }


        /// <summary>Values from A to F</summary>
        /// <param name="key">The key press to evaluate</param>
        /// <returns>true if A-F, otherwise false</returns>
        public static bool IsHexLetter(this Key key) {
            switch (key) {
                case Key.A:
                case Key.B:
                case Key.C:
                case Key.D:
                case Key.E:
                case Key.F:
                    return true;
                default:
                    return false;
            }
        }



        /// <summary>Values from G to Z</summary>
        /// <param name="key">The key press to evaluate</param>
        /// <returns>true if G-Z, otherwise false</returns>
        public static bool IsNonHexLetter(this Key key) {
            switch (key) {
                case Key.G:
                case Key.H:
                case Key.I:
                case Key.J:
                case Key.K:
                case Key.L:
                case Key.M:
                case Key.N:
                case Key.O:
                case Key.P:
                case Key.Q:
                case Key.R:
                case Key.S:
                case Key.T:
                case Key.U:
                case Key.V:
                case Key.W:
                case Key.X:
                case Key.Y:
                case Key.Z:
                    return true;
                default:
                    return false;
            }
        }

        public static bool IsLetter(this Key key) {
            return key.IsNonHexLetter() || key.IsHexLetter();
        }




        /// <summary>Is key 0-9, A-F</summary>
        /// <param name="key">The key press to evaluate</param>
        /// <returns>true if 0-9 or A-F, otherwise false</returns>
        public static bool IsHexDecimal(Key key) {
            if (key.IsHexLetter()) {
                return true;
            }
            return key.IsNumeric();
        }


        ///// <summary>Punctuation and math keys</summary>
        ///// <param name="key">The key press to evaluate</param>
        ///// <returns>true if G-Z, otherwise false</returns>
        //public static bool IsPunctuation(this Key key) {
        //    switch (key) {


        //        // whitespace
        //        //case Key.Tab:
        //        //case Key.Space:

        //        case Key.LineFeed:
        //        case Key.Clear:
        //        case Key.Enter:
        //        case Key.PageUp:
        //        case Key.Next:
        //        case Key.End:
        //        case Key.Home:

        //        // Math
        //        //case Key.Multiply:
        //        //case Key.Add:
        //        //case Key.Separator:
        //        //case Key.Subtract:
        //        //case Key.Divide:

        //        // decimal
        //        //case Key.Decimal:

        //        // Non hex letter
        //        //case Key.G:
        //        //case Key.H:
        //        //case Key.I:
        //        //case Key.J:
        //        //case Key.K:
        //        //case Key.L:
        //        //case Key.M:
        //        //case Key.N:
        //        //case Key.O:
        //        //case Key.P:
        //        //case Key.Q:
        //        //case Key.R:
        //        //case Key.S:
        //        //case Key.T:
        //        //case Key.U:
        //        //case Key.V:
        //        //case Key.W:
        //        //case Key.X:
        //        //case Key.Y:
        //        //case Key.Z:
        //            return true;
        //        default:
        //            return false;
        //    }
        //}


        /// <summary>Punctuation and math keys</summary>
        /// <param name="key">The key press to evaluate</param>
        /// <returns>true if G-Z, otherwise false</returns>
        public static bool IsMath(this Key key) {
            switch (key) {
                case Key.Multiply:
                case Key.Add:
                //case Key.Subtract:
                case Key.Divide:
                    return true;
                default:
                    return false;
            }
        }

        public static bool IsMinus(this Key key) {
            return key == Key.OemMinus || key == Key.Subtract;
        }




        public static bool IsWhitespace(this Key key) {
            switch (key) {
                case Key.Tab:
                case Key.Space:
                    return true;
                default:
                    return false;
            }
        }


        /// <summary>Decimal dot key</summary>
        /// <param name="key">The key press to evaluate</param>
        /// <returns>true if '.', otherwise false</returns>
        public static bool IsDecimal(this Key key) {
            return key == Key.Decimal;
        }


        // If we disable shift keys we eliminate the shifted values on keyboard which are not allowed


        /// <summary>Filter out forbidden keys, letters, decimal, minus</summary>
        /// <param name="key">The key press to evaluate</param>
        /// <returns>true if not allowed (mark handled), otherwise false</returns>
        public static bool IsUnsignedNumericForbidden(this Key key) {
            return key.IsNumericForbidden()
                || key.IsMinus()
                || key.IsLetter()
                || key.IsDecimal();
        }


        /// <summary>Filter out forbidden keys, non hex letters, decimal, minus</summary>
        /// <param name="key">The key press to evaluate</param>
        /// <returns>true if not allowed (mark handled), otherwise false</returns>
        public static bool IsHexNumericForbidden(this Key key) {
            return key.IsNumericForbidden()
                || key.IsMinus()
                || key.IsNonHexLetter()
                || key.IsDecimal();
        }


        /// <summary>Filter out forbidden keys, letters, decimal, minus, non binary numbers</summary>
        /// <param name="key">The key press to evaluate</param>
        /// <returns>true if not allowed (mark handled), otherwise false</returns>
        public static bool IsBinaryNumericForbidden(this Key key) {
            return key.IsNumericForbidden()
                || key.IsNonBinaryNumeric()
                || key.IsMinus()
                || key.IsLetter()
                || key.IsDecimal();
        }




        public static bool IsNumericForbidden(this Key key) {
            if (key.IsNumeric()) {
                // check against upper case non numberics coming in with same key
                if (Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift)) {
                    return true;
                }
            }
            if (key.IsWhitespace()) {
                return true;
            }
            if (key.IsMath()) {
                return true;
            }

            switch (key) {
                case Key.None:
                case Key.Cancel:
                case Key.LineFeed:
                case Key.Clear:
                case Key.Enter:
                case Key.Pause:
                //case Key.Capital:
                //    break;
                case Key.HangulMode: // Korean
                case Key.JunjaMode:  // Korean
                case Key.FinalMode:
                case Key.HanjaMode:  // Korean
                case Key.ImeConvert:
                case Key.ImeNonConvert:
                case Key.ImeAccept:
                case Key.ImeModeChange:
                //case Key.Escape:
                //    break;
                //case Key.PageUp:
                //    break;
                //case Key.Next:
                //    break;
                //case Key.End:
                //    break;
                //case Key.Home:
                //    break;
                //case Key.Left:
                //    break;
                //case Key.Up:
                //    break;
                //case Key.Right:
                //    break;
                //case Key.Down:
                //    break;
                case Key.Select:
                case Key.Print:
                case Key.Execute:
                case Key.PrintScreen:
                case Key.Insert:
                //case Key.Delete:
                //    break;
                //case Key.Help:
                //    break;
                case Key.LWin:
                case Key.RWin:
                case Key.Apps:
                case Key.Sleep:
                    break;

                    // math
                //case Key.Separator:
                //case Key.Subtract:
                //case Key.Decimal:
                //case Key.Divide:
                case Key.F1:
                case Key.F2:
                case Key.F3:
                case Key.F4:
                case Key.F5:
                case Key.F6:
                case Key.F7:
                case Key.F8:
                case Key.F9:
                case Key.F10:
                case Key.F11:
                case Key.F12:
                case Key.F13:
                case Key.F14:
                case Key.F15:
                case Key.F16:
                case Key.F17:
                case Key.F18:
                case Key.F19:
                case Key.F20:
                case Key.F21:
                case Key.F22:
                case Key.F23:
                case Key.F24:
                //case Key.NumLock:
                //case Key.Scroll:
                //    break;

                    // Already handle shift at top
                //case Key.LeftShift:
                //    break;
                //case Key.RightShift:
                //    break;

                //// Allow control and Alt for now
                //case Key.LeftCtrl:
                //    break;
                //case Key.RightCtrl:
                //    break;
                //case Key.LeftAlt:
                //    break;
                //case Key.RightAlt:
                //    break;
                // Media keys. Not used while inputing text
                case Key.BrowserBack:
                case Key.BrowserForward:
                case Key.BrowserRefresh:
                case Key.BrowserStop:
                case Key.BrowserSearch:
                case Key.BrowserFavorites:
                case Key.BrowserHome:
                case Key.VolumeMute:
                case Key.VolumeDown:
                case Key.VolumeUp:
                case Key.MediaNextTrack:
                case Key.MediaPreviousTrack:
                case Key.MediaStop:
                case Key.MediaPlayPause:
                case Key.LaunchMail:
                case Key.SelectMedia:
                case Key.LaunchApplication1:
                case Key.LaunchApplication2:
                // Oem keys
                case Key.OemPlus:
                case Key.OemComma:
                case Key.OemMinus:
                case Key.OemPeriod:
                case Key.AbntC1:
                case Key.AbntC2:
                case Key.Oem1:
                case Key.Oem2:
                case Key.Oem3:
                case Key.Oem4:
                case Key.Oem5:
                case Key.Oem6:
                case Key.Oem7:
                case Key.Oem8:
                case Key.Oem102:
                case Key.OemClear:

                // Other language shifting keys
                case Key.ImeProcessed:
                case Key.System:
                case Key.DbeAlphanumeric: // DBE japanese
                case Key.DbeKatakana:
                case Key.DbeHiragana:
                case Key.DbeSbcsChar:
                case Key.DbeDbcsChar:
                case Key.DbeRoman:
                case Key.DbeEnterImeConfigureMode:
                case Key.DbeFlushString:
                case Key.DbeCodeInput:
                case Key.DbeNoCodeInput:
                case Key.DbeDetermineString:
                case Key.DbeEnterDialogConversionMode:
                //
                case Key.Attn:
                case Key.CrSel:
                case Key.DeadCharProcessed:
                    return true;
                default:
                    return false;
            }






            return false;
        }




    }

}
