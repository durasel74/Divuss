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
	public partial class AlbumNameTextElement : UserControl
	{
		private bool isFirstChanged;

		public AlbumNameTextElement()
		{
			InitializeComponent();
			isFirstChanged = true;
		}

		private void TextBox_LostFocus(object sender, RoutedEventArgs e)
		{
			ViewModel.ViewModel.DataContext.CommandModel.AlbumRenameCommand.Execute(null);
		}

		private void TextBox_GotFocus(object sender, RoutedEventArgs e)
		{
			isFirstChanged = true;
		}

		private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
		{
			if (isFirstChanged == false) return;
			isFirstChanged = false;
			TextBox.SelectionStart = TextBox.Text.Length;
			TextBox.SelectAll();
		}
	}
}
