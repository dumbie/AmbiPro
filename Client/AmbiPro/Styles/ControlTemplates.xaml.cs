﻿using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace AmbiPro.Styles
{
    public partial class ControlTemplates : ResourceDictionary
    {
        //Handle horizontal scrollviewer scrolling
        void ScrollViewer_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            try
            {
                ScrollViewer scrollViewer = sender as ScrollViewer;
                if (e.Delta < 0) { scrollViewer.LineRight(); } else { scrollViewer.LineLeft(); }
            }
            catch { }
        }
    }
}