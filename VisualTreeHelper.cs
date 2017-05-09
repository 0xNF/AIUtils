using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace AIUtils {
    public static class RemoveChildHelper {
        public static void disconnectFromParent(this UIElement self) {
            DependencyObject parent = VisualTreeHelper.GetParent(self);
            parent.RemoveChild(self);
        }


        public static void RemoveChild(this DependencyObject parent, UIElement child) {
            var panel = parent as Panel;
            if (panel != null) {
                panel.Children.Remove(child);
                return;
            }

            var contentPresenter = parent as ContentPresenter;
            if (contentPresenter != null) {
                if (contentPresenter.Content == child) {
                    contentPresenter.Content = null;
                }
                return;
            }

            var contentControl = parent as ContentControl;
            if (contentControl != null) {
                if (contentControl.Content == child) {
                    contentControl.Content = null;
                }
                return;
            }

            // maybe more
        }


        public static IEnumerable<T> FindVisualChildren<T>(this DependencyObject depObj) where T : DependencyObject {
            if (depObj != null) {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++) {
                    DependencyObject child = VisualTreeHelper.GetChild(depObj, i);
                    if (child != null && child is T) {
                        yield return (T)child;
                    }

                    foreach (T childOfChild in FindVisualChildren<T>(child)) {
                        yield return childOfChild;
                    }
                }
            }
        }
    }

    public static class VisualTreeHelperExtensions {
        public static T GetFirstDescendantOfType<T>(this DependencyObject start) where T : DependencyObject {
            return start.GetDescendantsOfType<T>().FirstOrDefault();
        }

        public static IEnumerable<T> GetDescendantsOfType<T>(this DependencyObject start) where T : DependencyObject {
            return start.GetDescendants().OfType<T>();
        }

        public static IEnumerable<DependencyObject> GetDescendants(this DependencyObject start) {
            var queue = new Queue<DependencyObject>();
            var count = VisualTreeHelper.GetChildrenCount(start);

            for (int i = 0; i < count; i++) {
                var child = VisualTreeHelper.GetChild(start, i);
                yield return child;
                queue.Enqueue(child);
            }

            while (queue.Count > 0) {
                var parent = queue.Dequeue();
                var count2 = VisualTreeHelper.GetChildrenCount(parent);

                for (int i = 0; i < count2; i++) {
                    var child = VisualTreeHelper.GetChild(parent, i);
                    yield return child;
                    queue.Enqueue(child);
                }
            }
        }

        public static T GetFirstAncestorOfType<T>(this DependencyObject start) where T : DependencyObject {
            return start.GetAncestorsOfType<T>().FirstOrDefault();
        }

        public static IEnumerable<T> GetAncestorsOfType<T>(this DependencyObject start) where T : DependencyObject {
            return start.GetAncestors().OfType<T>();
        }

        public static IEnumerable<DependencyObject> GetAncestors(this DependencyObject start) {
            var parent = VisualTreeHelper.GetParent(start);

            while (parent != null) {
                yield return parent;
                parent = VisualTreeHelper.GetParent(parent);
            }
        }

        public static bool IsInVisualTree(this DependencyObject dob) {
            return Window.Current.Content != null && dob.GetAncestors().Contains(Window.Current.Content);
        }

        public static Windows.Foundation.Rect GetBoundingRect(this FrameworkElement dob, FrameworkElement relativeTo = null) {
            if (relativeTo == null) {
                relativeTo = Window.Current.Content as FrameworkElement;
            }

            if (relativeTo == null) {
                throw new InvalidOperationException("Element not in visual tree.");
            }

            if (dob == relativeTo)
                return new Windows.Foundation.Rect(0, 0, relativeTo.ActualWidth, relativeTo.ActualHeight);

            var ancestors = dob.GetAncestors().ToArray();

            if (!ancestors.Contains(relativeTo)) {
                throw new InvalidOperationException("Element not in visual tree.");
            }

            var pos =
                dob
                    .TransformToVisual(relativeTo)
                    .TransformPoint(new Windows.Foundation.Point());
            var pos2 =
                dob
                    .TransformToVisual(relativeTo)
                    .TransformPoint(
                        new Windows.Foundation.Point(
                            dob.ActualWidth,
                            dob.ActualHeight));

            return new Windows.Foundation.Rect(pos, pos2);
        }

        public static ScrollViewer GetScrollViewer(this DependencyObject element) {
            if (element is ScrollViewer) {
                return (ScrollViewer)element;
            }
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(element); i++) {
                var child = VisualTreeHelper.GetChild(element, i);

                var result = GetScrollViewer(child);
                if (result == null) {
                    continue;
                }
                else {
                    return result;
                }
            }

            return null;
        }
    }
}



