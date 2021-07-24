using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
using Divuss.ViewModel;

namespace Divuss.View
{
	public partial class MainWindow : Window
	{
		private static ViewModel.ViewModel dataContext;
		private static MainWindow thisWindow;

		public MainWindow()
		{
			try
			{
				Logger.Initialize();
				InitializeComponent();
				thisWindow = this;
				DataContext = new ViewModel.ViewModel();
			}
			catch (Exception e)
			{
				Logger.LogError(e.Message);
				Logger.ForcedClose();
				Application.Current.Shutdown();
			}
			dataContext = (ViewModel.ViewModel)this.DataContext;
		}
		internal static MainWindow GetWindow()
		{
			if (thisWindow != null)
				return thisWindow;
			return null;
		}

		private void Window_Closed(object sender, EventArgs e)
		{
			Logger.Close();
		}

		private void SectionsList_SelectionChanged(object sender, 
			SelectionChangedEventArgs e)
		{
			var tabItem = (sender as TabControl)?.SelectedItem as TabItem;
			var userControl = tabItem?.Content as UserControl;
			Model.Section section = userControl?.DataContext as Model.Section;
			if (section != null && e.OriginalSource is TabControl)
				dataContext.CurrentSection = section;
		}

		private void IsKeyDown(object sender, KeyEventArgs e)
		{
			if (IsPressedControl(e.Key))
				dataContext.SelectionModeCommand.Execute(true);
			else if (IsPressedShift(e.Key))
				dataContext.SelectionModeCommand.Execute(true);
		}
		private void IsKeyUp(object sender, KeyEventArgs e)
		{
			if (IsPressedControl(e.Key))
				dataContext.SelectionModeCommand.Execute(false);
			else if (IsPressedShift(e.Key))
				dataContext.SelectionModeCommand.Execute(false);
		}
		private bool IsPressedControl(Key key)
		{
			return key == Key.LeftCtrl || key == Key.RightCtrl;
		}
		private bool IsPressedShift(Key key)
		{
			return key == Key.LeftShift || key == Key.RightShift;
		}
	}
}
