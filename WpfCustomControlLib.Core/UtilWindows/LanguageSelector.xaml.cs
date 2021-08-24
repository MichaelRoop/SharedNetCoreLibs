using LanguageFactory.Net.data;
using LanguageFactory.Net.interfaces;
using LanguageFactory.Net.Messaging;
using LogUtils.Net;
using System;
using System.Windows;
using System.Windows.Controls;
using WpfCustomControlLib.Core.Helpers;
using WpfHelperClasses.Core;

namespace WpfCustomControlLib.Core.UtilWindows {

    /// <summary>logic for LanguageSelector.xaml</summary>
    public partial class LanguageSelector : Window {

        #region Data

        private LangCode languageOnEntry = LangCode.English;
        private ButtonGroupSizeSyncManager widthManager = null;
        private Window parent = null;
        ILangFactory languageFactory = null;
        ClassLog log = new ClassLog("");

        #endregion

        #region Constructors and windows events

        public static void ShowBox(Window parent, ILangFactory languageFactory) {
            LanguageSelector win = new LanguageSelector(parent, languageFactory);
            win.ShowDialog();
        }


        private LanguageSelector(Window parent, ILangFactory languageFactory) {
            this.parent = parent;
            this.languageFactory = languageFactory;
            InitializeComponent();
            this.SizeToContent = SizeToContent.WidthAndHeight;

            // Connect to language event
            this.languageFactory.LanguageChanged += Languages_LanguageChanged;
            this.languageOnEntry = this.languageFactory.CurrentLanguageCode;

            // Call before rendering which will trigger initial resize events
            this.widthManager = new ButtonGroupSizeSyncManager(this.btnCancel, this.btnSave);
            this.widthManager.PrepForChange();
        }


        public override void OnApplyTemplate() {
            this.BindMouseDownToCustomTitleBar();
            base.OnApplyTemplate();
        }


        private void Window_Loaded(object sender, RoutedEventArgs e) {
            this.lbxLanguages.ItemsSource = this.languageFactory.AvailableLanguages;
            this.lbxLanguages.SelectionChanged += this.lbLanguages_SelectionChanged;
            this.CenterToParent(this.parent);
        }


        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e) {
            this.lbxLanguages.SelectionChanged -= this.lbLanguages_SelectionChanged;
            this.languageFactory.LanguageChanged -= this.Languages_LanguageChanged;
            this.widthManager.Teardown();
        }


        #endregion

        #region Controls events

        private void btnCancel_Click(object sender, RoutedEventArgs e) {
            this.languageFactory.SetCurrentLanguage(this.languageOnEntry);
            this.Close();
        }


        private void btnSave_Click(object sender, RoutedEventArgs e) {
            // wrapper would have saved each change to settings so just close with last change
            this.Close();
        }


        private void lbLanguages_SelectionChanged(object sender, SelectionChangedEventArgs args) {
            try {
                LanguageDataModel data = this.lbxLanguages.SelectedItem as LanguageDataModel;
                if (data != null) {
                    this.languageFactory.SetCurrentLanguage(data.Code);
                }
            }
            catch (Exception ex) {
                this.log.Exception(9999, "lbLanguages_SelectionChanged", "", ex);
            }
        }

        #endregion

        #region Language events

        private void Languages_LanguageChanged(object sender, SupportedLanguage l) {
            this.Dispatcher.Invoke(() => {
                try {
                    this.Title = l.GetText(MsgCode.language);
                    this.widthManager.PrepForChange();
                    this.btnSave.Content = l.GetText(MsgCode.save);
                    this.btnCancel.Content = l.GetText(MsgCode.cancel);
                }
                catch (Exception ex) {
                    this.log.Exception(9999, "Languages_LanguageChanged", "", ex);
                }
            });
        }

        #endregion

    }
}
