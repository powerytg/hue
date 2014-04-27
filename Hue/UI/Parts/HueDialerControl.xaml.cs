using Hue.API.Hue;
using Hue.API.Media;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace Hue.UI.Parts
{
    public partial class HueDialerControl : UserControl
    {
        public static readonly DependencyProperty HSBColorSourceProperty = DependencyProperty.Register(
       "HSBColorSource",
       typeof(HSBColor),
       typeof(HueDialerControl),
       new PropertyMetadata(null, OnHSBColorSourcePropertyChanged));

        public HSBColor HSBColorSource
        {
            get { return (HSBColor)GetValue(HSBColorSourceProperty); }
            set { SetValue(HSBColorSourceProperty, value); }
        }

        private static void OnHSBColorSourcePropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var target = (HueDialerControl)sender;
            target.OnHSBColorSourceChanged();
        }

        
        protected virtual void OnHSBColorSourceChanged()
        {
            // Rotate to current value
            CurrentValue = (int)HSBColorSource.H;
            RotateToCurrentValue();
            
            Color currentColor = HSBColor.FromHSB(HSBColorSource);
            ColorIndicatorBrush.Color = currentColor;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public HueDialerControl()
        {
            this.InitializeComponent();
            
            // Events
            GestureCaptureCanvas.ManipulationStarted += OnDialerDragStart;
            GestureCaptureCanvas.ManipulationCompleted += OnDialerDragEnd;
            GestureCaptureCanvas.ManipulationDelta += OnDialerDragDelta;
        }

        private double startAngle;
        private double rotateScaleFactor = 1.0;

        public int CurrentValue { get; set; }
       
        // Events
        public EventHandler DragBegin;
        public EventHandler DragEnd;
        public EventHandler ValueChanged;
        public EventHandler ValueChanging;

        protected void OnDialerDragStart(object sender, ManipulationStartedRoutedEventArgs e)
        {
            DialerTransform.CenterX = Width / 2;
            DialerTransform.CenterY = Height / 2;
            
            startAngle = DialerTransform.Angle;

            if (DragBegin != null)
            {
                DragBegin(this, null);
            }
        }

        protected void OnDialerDragEnd(object sender, ManipulationCompletedRoutedEventArgs e)
        {
            if (DragEnd != null)
            {
                DragEnd(this, null);
            }

            if (ValueChanged != null)
            {
                ValueChanged(this, null);
            }

        }

        protected void OnDialerDragDelta(object sender, ManipulationDeltaRoutedEventArgs e)
        {
            double accumatedDist;
            if (Math.Abs(e.Cumulative.Translation.X) > Math.Abs(e.Cumulative.Translation.Y))
            {
                accumatedDist = e.Cumulative.Translation.X;
            }
            else
            {
                accumatedDist = e.Cumulative.Translation.Y;
            }

            double angle = startAngle + accumatedDist * rotateScaleFactor;

            DialerTransform.Angle = angle;
            if (angle > 360)
            {
                angle = angle % 360;
            }

            CurrentValue = (int) ((angle / 360.0f) * Light.MaxHue);

            Color currentColor = HSBColor.FromHSB(CurrentValue, (int)HSBColorSource.S, (int)HSBColorSource.B);
            ColorIndicatorBrush.Color = currentColor;

            if (ValueChanging != null)
            {
                ValueChanging(this, null);
            }            
        }

        protected void RotateToCurrentValue()
        {
            DialerTransform.CenterX = Width / 2;
            DialerTransform.CenterY = Height / 2;
            DialerTransform.Angle = ((float)CurrentValue / Light.MaxHue) * 360;

        }

    }
}
