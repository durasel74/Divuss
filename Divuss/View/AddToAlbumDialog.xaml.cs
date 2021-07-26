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
using System.Windows.Shapes;

namespace Divuss.View
{
	public partial class AddToAlbumDialog : Window
	{
		internal AddToAlbumDialog(ViewModel.ViewModel dataContext)
		{
			InitializeComponent();
			DataContext = dataContext;
		}

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			this.Close();
		}
	}
}
