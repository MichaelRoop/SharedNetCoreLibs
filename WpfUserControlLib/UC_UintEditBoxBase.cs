﻿using LogUtils.Net;
using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Input;
using WpfUserControlLib.DataModels;
using WpfUserControlLib.Enumerations;
using WpfUserControlLib.Interfaces;

namespace WpfUserControlLib {

    public abstract class UC_UintEditBoxBase : UserControl, IUintEditBox {

        private ClassLog log = new ClassLog("UC_UintEditBoxBase");
        private Func<string, bool> validateFunc = null;
        private List<IUintEditBox> dependants = new List<IUintEditBox>();

        #region  IUintEditBox

        public event EventHandler<UInt64> OnValueChanged;
        public event EventHandler OnValueEmpty;
        public event EventHandler<UIntEditStatusInfo> OnError;

        public UC_UintEditBoxBase() {
            this.validateFunc = this.DummyValidator;
        }


        public void RegisterDependant(IUintEditBox editBox) {
            this.dependants.Add(editBox);
        }


        public bool SetValue(UInt64 value) {
            if (this.validateFunc(value.ToString())) {
                this.DoSetValue(value);
                return true;
            }
            this.DoSetEmpty();
            return false;
        }


        public void SetEmpty() {
            this.DoSetEmpty();
        }


        public void SetValidator(Func<string, bool> func) {
            this.validateFunc = func;
        }

        #endregion

        #region Abstract

        protected abstract void DoSetValue(UInt64 value);
        protected abstract void DoSetEmpty();

        public abstract string Text { get; }

        #endregion

        #region Protected

        protected void RaiseValueEmpty() {
            this.OnValueEmpty?.Invoke(this, new EventArgs());
            this.SetDependantsEmpty();
        }

        protected void RaiseValueChanged(UInt64 value) {
            this.OnValueChanged?.Invoke(this, value);
            this.SetDependantsValue(value);
        }

        private void PostError(UintEditStatus status, string err) {
            try {
                this.OnError?.Invoke(this, new UIntEditStatusInfo(status, err));
            }
            catch (Exception ex) {
                this.log.Exception(9999, err, ex);
            }
        }

        protected void SetDependantsEmpty() {
            foreach (var d in this.dependants) {
                d.SetEmpty();
            }
        }


        protected void SetDependantsValue(UInt64 value) {
            foreach (var d in this.dependants) {
                d.SetValue(value);
            }
        }


        protected void ValidateRange(Func<string> valueFunc, KeyEventArgs args) {
            try {
                if (!this.validateFunc(valueFunc.Invoke())) {
                    args.Handled = true;
                }
            }
            catch (OverflowException ofex) {
                args.Handled = true;
                this.log.Exception(9999, "ValidateRange", "", ofex);
                this.PostError( UintEditStatus.InvalidInput, "");
            }
            catch (Exception ex) {
                args.Handled = true;
                this.log.Exception(9999, "ValidateRange", "", ex);
                this.PostError(UintEditStatus.UnhandledError, ex.Message);
            }
        }



        protected void ProcessPreviewKey(KeyEventArgs args, Func<bool> isForbidden, Func<bool> isValidValue, Action onOkToProcess) {
            try {
                if (isForbidden()) {
                    args.Handled = true;
                }
                else {
                    if (isValidValue()) {
                        onOkToProcess();
                    }
                }
            }
            catch (Exception ex) {
                this.log.Exception(9999, "", ex);
            }
        }


        protected void ProcessTextChanged(string text, Func<UInt64> valueFunc) {
            try {
                this.log.Info("SetValues", text);
                if (text.Length == 0) {
                    this.RaiseValueEmpty();
                }
                else {
                    UInt64 value = valueFunc.Invoke();
                    this.SetValue(value);
                    this.RaiseValueChanged(value);
                }
            }
            catch (Exception ex) {
                this.log.Exception(9999, "", ex);
            }
        }

        #endregion

        #region Private

        private bool DummyValidator(string value) {
            return true;
        }

        #endregion

    }

}
