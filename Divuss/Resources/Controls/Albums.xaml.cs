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
	public partial class Albums : UserControl
	{
		public Albums()
		{
			InitializeComponent();
		}

		private void AlbumList_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			if (!IsKeyModifiersDown())
				((ListBox)sender).SelectedItems.Clear();
		}

		private bool IsKeyModifiersDown()
		{
			if (Keyboard.IsKeyDown(Key.LeftCtrl) ||
					Keyboard.IsKeyDown(Key.RightCtrl))
				return true;
			else if (Keyboard.IsKeyDown(Key.LeftShift) ||
					Keyboard.IsKeyDown(Key.RightShift))
				return true;
			return false;
		}
	}
}
