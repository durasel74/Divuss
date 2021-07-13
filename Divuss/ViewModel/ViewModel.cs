using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Collections.ObjectModel;
using System.Windows;
using Microsoft.Win32;

namespace Divuss.ViewModel
{
	internal class ViewModel : INotifyPropertyChanged
	{
        public ViewModel()
        {
            PhotosTab = Photos.GetInstance();
            AlbumsTab = Albums.GetInstance();
		}

        public Photos PhotosTab { get; }
        public Albums AlbumsTab { get; }

		private ButtonCommand pictureSwitchCommand;
		public ButtonCommand PictureSwitchCommand
		{
			get
			{
				return pictureSwitchCommand ??
				  (pictureSwitchCommand = new ButtonCommand(obj =>
				  {
					  PhotosTab.PictureSwitch(obj);
				  }));
			}
		}

		private ButtonCommand pictureOpenCommand;
		public ButtonCommand PictureOpenCommand
		{
			get
			{
				return pictureOpenCommand ??
				  (pictureOpenCommand = new ButtonCommand(obj =>
				  {
					  OpenFileDialog openFileDialog = new OpenFileDialog();
					  openFileDialog.Filter = "Image files (*.png;*.jpg)|*.png;*.jpg";
					  if (openFileDialog.ShowDialog() == true)
					  {
						  var imagePath = openFileDialog.FileName;
						  PhotosTab.AddImageToLast(imagePath);
						  PhotosTab.OpenPictureView();
					  }
				  }));
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;
		public void OnPropertyChanged([CallerMemberName] string prop = "")
		{
			if (PropertyChanged != null)
				PropertyChanged(this, new PropertyChangedEventArgs(prop));
		}
	}
}
