using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;

namespace AIUtils {
    public static class XamlExtensions {

        public static void toggleVisibility(this FrameworkElement self) {
            if (self.Visibility == Visibility.Visible) {
                self.Visibility = Visibility.Collapsed;
            }
            else {
                self.Visibility = Visibility.Visible;
            }
        }

        public static void deselectAll(this ListViewBase self) {
            ItemIndexRange iir = new ItemIndexRange(0, Convert.ToUInt32(self.Items.Count));
            self.DeselectRange(iir);
        }
    }
}
