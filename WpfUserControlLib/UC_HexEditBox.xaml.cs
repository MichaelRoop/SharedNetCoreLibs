﻿using LogUtils.Net;
using System;
using System.Windows.Controls;
using System.Windows.Input;
using WpfHelperClasses.Core;

namespace WpfUserControlLib {
    /// <summary>
    /// Interaction logic for UC_HexEditBox.xaml
    /// </summary>
    public partial class UC_HexEditBox : UC_UintEditBoxBase {

        private ClassLog log = new ClassLog("UC_HexEditBox");

        public UC_HexEditBox() : base() {
            InitializeComponent();
        }

        public override string Text { get { return this.tbEdit.Text; } }


        protected override void DoSetValue(UInt64 value) {
            this.tbEdit.TextChanged -= this.tbEdit_TextChanged;
            int carretIndex = this.tbEdit.CaretIndex;
            this.tbEdit.Text = value.ToString("X");
            this.tbEdit.CaretIndex = carretIndex > this.tbEdit.Text.Length
                ? this.tbEdit.Text.Length
                : carretIndex;
            this.tbEdit.TextChanged += this.tbEdit_TextChanged;
        }


        protected override void DoSetEmpty() {
            this.tbEdit.TextChanged -= this.tbEdit_TextChanged;
            this.tbEdit.Text = "";
            this.tbEdit.TextChanged += this.tbEdit_TextChanged;
        }


        private void tbEdit_PreviewKeyDown(object sender, KeyEventArgs args) {
            try {
                if (args.Key.IsHexNumericForbidden()) {
                    args.Handled = true;
                }
                else {
                    if (args.Key.IsHexDecimal()) {
                        string add = args.Key.GetHexDecimalValue();
                        string newVal = this.tbEdit.PreviewKeyDownAssembleText(add);
                        this.log.Info("", () => string.Format("'{0}'  '{1}'  '{2}'", this.tbEdit, add, newVal));
                        if (newVal.Length > 0) {
                            this.ValidateRange(() => Convert.ToUInt64(newVal, 16).ToString(), args);
                        }
                    }
                }
            }
            catch (Exception ex) {
                this.log.Exception(9999, "", ex);
            }
        }

        private void tbEdit_TextChanged(object sender, TextChangedEventArgs e) {
            this.log.Info("tbEdit_TextChanged", () => this.tbEdit.Text);
            this.SetValues(this.tbEdit.Text, () => Convert.ToUInt64(this.tbEdit.Text, 16));
        }
    }
}
