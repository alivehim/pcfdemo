using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace UtilityTools.Core.Extensions
{
    public static class WindowExtensions
    {
        /// <summary>
        /// Static local variable to check whether it is the first instance
        /// </summary>
        private static bool m_firstTime = true;

        /// <summary>
        /// Centers the Window in screen.
        /// </summary>
        /// <param name="window">The window.</param>
        public static void CenterInScreen(this Window window)
        {
            double width = window.ActualWidth;
            double height = window.ActualHeight;

            if (m_firstTime) // First time ActualWidth and ActualHeight is not set
            {
                if (!Double.IsNaN(window.Width)) { width = window.Width; }
                if (!Double.IsNaN(window.Height)) { height = window.Height; }

                m_firstTime = false;
            }

            // Set Left and Top manually and calculate center of screen.
            window.Left = (SystemParameters.WorkArea.Width - width) / 2
                + SystemParameters.WorkArea.Left;
            window.Top = (SystemParameters.WorkArea.Height - height) / 2
                + SystemParameters.WorkArea.Top;
        }
    }
}
