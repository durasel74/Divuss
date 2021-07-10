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

namespace Divuss.View
{
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();
			DataContext = new ViewModel.ViewModel();
		}

		private void Button_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
		{
			PicturePanel.Visibility = Visibility.Visible;
		}

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			PicturePanel.Visibility = Visibility.Collapsed;
		}
	}
}
