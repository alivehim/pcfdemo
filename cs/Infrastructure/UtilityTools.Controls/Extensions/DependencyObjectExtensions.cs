using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Windows;
using System.Windows.Media;

namespace UtilityTools.Controls.Extensions
{
    public static class DependencyObjectExtensions
    {
        public static bool TryFindDescendant<T>(this DependencyObject reference, [NotNullWhen(true)] out T descendant) where T : DependencyObject
        {
            var queue = new Queue<DependencyObject>();
            var parent = reference;

            while (parent != null)
            {
                int count = VisualTreeHelper.GetChildrenCount(parent);
                for (int i = 0; i < count; i++)
                {
                    var child = VisualTreeHelper.GetChild(parent, i);
                    if (child is T buffer)
                    {
                        descendant = buffer;
                        return true;
                    }
                    queue.Enqueue(child);
                }

                parent = (0 < queue.Count) ? queue.Dequeue() : null;
            }

            descendant = default;
            return false;
        }
    }
}
