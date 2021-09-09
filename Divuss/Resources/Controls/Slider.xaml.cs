using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Divuss.Resources.Controls
{
	public partial class Slider : UserControl
	{
		private readonly Brush preTransparentColor;

		private double maxWidth;
		private double actualWidth;
		private double normalize;
		private Brush lineColor;
		private bool isLineVisibility;

		public Slider()
		{
			InitializeComponent();
			preTransparentColor = new SolidColorBrush(Color.FromArgb(0, 0xFF, 0xFF, 0xFF));
			isLineVisibility = true;
			normalize = 0.5;
			GradientStop1.Offset = normalize;
			GradientStop2.Offset = normalize;
		}

		private void Spliter_DragDelta(object sender, 
			System.Windows.Controls.Primitives.DragDeltaEventArgs e)
		{
			SpliterChangePosition();
		}

		private void Spliter_SizeChanged(object sender, SizeChangedEventArgs e)
		{
			SpliterChangePosition();
		}

		private void SpliterChangePosition()
		{
			maxWidth = Spliter.ActualWidth;
			actualWidth = Spliter.ColumnDefinitions[0].ActualWidth;
			normalize = actualWidth / maxWidth;
			GradientStop1.Offset = normalize;
			GradientStop2.Offset = normalize;
		}

		private void SwitchLineClick(object sender, RoutedEventArgs e)
		{
			if (isLineVisibility)
			{
				lineColor = SliderLine.Background;
				SliderLine.Background = preTransparentColor;
				isLineVisibility = false;
			}
			else
			{
				SliderLine.Background = lineColor;
				isLineVisibility = true;
			}
		}
	}
}
