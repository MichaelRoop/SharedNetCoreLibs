using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace WpfHelperClasses.Core {

    /// <summary>Handler for synchronization multiple button widths to the widest</summary>
    public class ButtonGroupSizeSyncManager {

        #region Data

        private List<ButtonEventInfo> buttonInfo = new List<ButtonEventInfo>();
        private Button[] buttonObjs = null;

        #endregion

        #region Constructors

        /// <summary>Constructor</summary>
        /// <param name="buttons">Buttons to manage for width synchronization</param>
        public ButtonGroupSizeSyncManager(params Button[] buttons) {
            this.Init(buttons);
        }

        #endregion

        #region Public

        /// <summary>Called before the managed buttons are to be changed</summary>
        public void PrepForChange() {
            WPF_ControlHelpers.ForceButtonMinMax(this.buttonObjs);
            this.buttonInfo.ForEach((x) => this.PrepForChange(x));
        }


        /// <summary>Disconnect all handlers before leaving Window</summary>
        public void Teardown() {
            this.buttonInfo.ForEach((x) => x.ButtonObj.SizeChanged -= this.SizeChangedHandler);
        }

        #endregion

        #region Private
        
        /// <summary>Initialise this object with a Button array</summary>
        /// <param name="buttons">The button array</param>
        private void Init(params Button[] buttons) {
            buttons.ToList().ForEach((x) => this.buttonInfo.Add(new ButtonEventInfo(x)));
            this.buttonObjs = buttons;
        }


        /// <summary>Handles the SizeChanged event for all manged buttons</summary>
        /// <remarks>
        /// If it is the last of manged buttons returning the event then all 
        /// buttons will be resized in width to the widest button
        /// </remarks>
        /// <param name="sender">The button sending the SizeChanged event</param>
        /// <param name="args">The SizeChanged information</param>
        private void SizeChangedHandler(object sender, SizeChangedEventArgs args) {
            if (this.AreAllButtonsSized(sender as Button)) { 
                this.buttonInfo.ForEach((x) => this.OnSizeChanged(x));
                WPF_ControlHelpers.ResizeToWidest(this.buttonObjs);
            }
        }


        /// <summary>Sets the relative button info as sized and checks if all set</summary>
        /// <param name="button">The button that sent the current SizeChanged event</param>
        /// <returns>true if all managed buttons have received the SizeChanged event</returns>
        private bool AreAllButtonsSized(Button button) {
            this.buttonInfo.ForEach((x) => x.SetSizedIfSame(button));
            return this.buttonInfo.FirstOrDefault((x) => x.IsSized == false) == null;
        }


        /// <summary>Resets values if the set is SizeChanged group is done</summary>
        /// <param name="info">The button info object to reset as well as the event handler</param>
        private void OnSizeChanged(ButtonEventInfo info) {
            info.ResetSized();
            info.ButtonObj.SizeChanged -= this.SizeChangedHandler;
        }


        /// <summary>Prepare the object for the next group SizeChanged events</summary>
        /// <param name="info">The button info object to reconnect event handlers</param>
        private void PrepForChange(ButtonEventInfo info) {
            info.ButtonObj.SizeChanged -= this.SizeChangedHandler;
            info.ButtonObj.SizeChanged += this.SizeChangedHandler;
        }


        #endregion

    }
}
