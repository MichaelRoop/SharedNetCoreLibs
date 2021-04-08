using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace WpfCustomControlLib.Core.Sliders {

    public partial class UC_SliderBool : Slider {

        // TODO Dependency properties to set on and off color in XAML

        public UC_SliderBool() {
            // TODO - check if can be made inaccessible
            this.IsSnapToTickEnabled = true;
            this.Minimum = 0;
            this.Maximum = 1;
            this.TickFrequency = 1;
            this.ValueChanged += UC_SliderBool_ValueChanged;
        }


        private void UC_SliderBool_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> args) {
            // Change color on event
            this.Foreground = (args.NewValue == 0)
                ? new SolidColorBrush(Colors.Red)
                : new SolidColorBrush(Colors.YellowGreen);
            // Allow the event to be passed up to others subscribing to ValueChanged
            args.Handled = false;
        }

    }

}
