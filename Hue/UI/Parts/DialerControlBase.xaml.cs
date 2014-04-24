using Hue.API.Hue;
using Hue.API.Media;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
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
    public partial class DialerControlBase : UserControl
    {
        public static readonly DependencyProperty HSBColorSourceProperty = DependencyProperty.Register(
       "HSBColorSource",
       typeof(HSBColor),
       typeof(DialerControlBase),
       new PropertyMetadata(null, OnHSBColorSourcePropertyChanged));

        public HSBColor HSBColorSource
        {
            get { return (HSBColor)GetValue(HSBColorSourceProperty); }
            set { SetValue(HSBColorSourceProperty, value); }
        }

        private static void OnHSBColorSourcePropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var target = (DialerControlBase)sender;
            target.OnHSBColorSourceChanged();
        }

        
        protected virtual void OnHSBColorSourceChanged()
        {
            DialerImage.Source = GetDialerImage();
        }

        protected virtual BitmapImage GetDialerImage()
        {
            return null;
        }

        protected virtual List<GradientStop> GetBorderGradient()
        {
            return new List<GradientStop>();
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public DialerControlBase()
        {
            this.InitializeComponent();
            
            // Initialize properties
            IsInfiniteScrollingEnabled = true;
            
            GestureCaptureCanvas.ManipulationStarted += OnDialerDragStart;
            GestureCaptureCanvas.ManipulationCompleted += OnDialerDragEnd;
            GestureCaptureCanvas.ManipulationDelta += OnDialerDragDelta;
        }

        private double currentY;
        private double rotationStep = 2;
        private double anglePerStep;
        private double previousStepY = 0;

        protected int baseIndex;

        public bool IsInfiniteScrollingEnabled { get; set; }

        public int CurrentValue { get; set; }

        private List<int> _supportedValues;
        public List<int> SupportedValues
        {
            get
            {
                return _supportedValues;
            }
            set
            {
                _supportedValues = value;
                if (_supportedValues.Count > 0)
                {
                    anglePerStep = 360 / _supportedValues.Count;
                    baseIndex = 0;
                    CurrentIndex = baseIndex;
                }
            }
        }

        public int CurrentIndex { get; set; }

        // Events
        public EventHandler DragBegin;
        public EventHandler DragEnd;
        public EventHandler ValueChanged;
        public EventHandler ValueChanging;

        protected void OnDialerDragStart(object sender, ManipulationStartedRoutedEventArgs e)
        {
            DialerTransform.CenterX = Width / 2;
            DialerTransform.CenterY = Height / 2;

            previousStepY = currentY;

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
            if (Math.Abs(e.Delta.Translation.X) > Math.Abs(e.Delta.Translation.Y))
            {
                currentY += e.Delta.Translation.X;
            }
            else
            {
                currentY += e.Delta.Translation.Y;
            }

            double accumatedDist = currentY - previousStepY;

            if (Math.Abs(accumatedDist) >= rotationStep)
            {
                if (accumatedDist < 0)
                {
                    // Moving up
                    CurrentIndex++;

                    if (CurrentIndex >= SupportedValues.Count)
                    {
                        CurrentIndex = IsInfiniteScrollingEnabled ? 0 : SupportedValues.Count - 1;
                    }
                }
                else
                {
                    // Moving down
                    CurrentIndex--;

                    if (CurrentIndex < 0)
                    {
                        CurrentIndex = IsInfiniteScrollingEnabled ? SupportedValues.Count - 1 : 0;
                    }
                }

                DialerTransform.Angle = (CurrentIndex - baseIndex) * anglePerStep;

                previousStepY = currentY;
                CurrentValue = SupportedValues[CurrentIndex];

                // Show ticks
                var percent = CurrentIndex / ((float)SupportedValues.Count - 1);
                if (ValueChanging != null)
                {
                    ValueChanging(this, null);
                }
            }
        }

        protected void RotateToCurrentValue()
        {
            DialerTransform.CenterX = Width / 2;
            DialerTransform.CenterY = Height / 2;

            DialerTransform.Angle = (CurrentIndex - baseIndex) * anglePerStep;
        }

    }
}
