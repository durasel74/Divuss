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
using Divuss.Service;

namespace Divuss.Resources.Controls
{
	public partial class PictureView : UserControl
	{
		private static bool fullscreenMode;
		private static FullscreenParametersSave parametersSave;
		private static Thickness screenAreaThicknessSave;
		private static Grid screenArea;

		public PictureView()
		{
			InitializeComponent();
			screenArea = ScreenArea;
		}

		private void FullscreenButton_Click(object sender, RoutedEventArgs e)
		{
			FullscreenSwitch();
		}

		public static void FullscreenSwitch()
		{
			if (!fullscreenMode)
				FullscreenOpen();
			else
				FullscreenClose();
		}

		public static void FullscreenOpen()
		{
			if (fullscreenMode == true)
				return;

			var window = View.MainWindow.GetWindow();
			fullscreenMode = true;
			parametersSave = new FullscreenParametersSave(window);
			screenAreaThicknessSave = screenArea.Margin;
			window.WindowStyle = WindowStyle.None;
			window.WindowState = WindowState.Normal;
			window.WindowState = WindowState.Maximized;
			window.ResizeMode = ResizeMode.NoResize;
			window.Left = 0;
			window.Top = 0;
			window.Width = SystemParameters.PrimaryScreenWidth;
			window.Height = SystemParameters.PrimaryScreenHeight;
			screenArea.Margin = new Thickness(6);
			Logger.LogTrace("Переход в полноэкранный режим");
		}

		public static void FullscreenClose()
		{
			if (fullscreenMode == false)
				return;

			var window = View.MainWindow.GetWindow();
			fullscreenMode = false;
			window.WindowStyle = parametersSave.WindowStyle;
			window.WindowState = parametersSave.WindowState;
			window.ResizeMode = parametersSave.ResizeMode;
			window.Left = parametersSave.Left;
			window.Top = parametersSave.Top;
			window.Width = parametersSave.Width;
			window.Height = parametersSave.Height;
			screenArea.Margin = screenAreaThicknessSave;
			Logger.LogTrace("Выход из полноэкранного режима");
		}

		private struct FullscreenParametersSave
		{
			public FullscreenParametersSave(View.MainWindow window)
			{
				WindowStyle = window.WindowStyle;
				WindowState = window.WindowState;
				ResizeMode = window.ResizeMode;
				Left = window.Left;
				Top = window.Top;
				Width = window.Width;
				Height = window.Height;
			}

			public WindowStyle WindowStyle { get; set; }
			public WindowState WindowState { get; set; }
			public ResizeMode ResizeMode { get; set; }
			public double Left { get; set; }
			public double Top { get; set; }
			public double Width { get; set; }
			public double Height { get; set; }
		}
	}
}
