using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Collections.ObjectModel;

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

		public event PropertyChangedEventHandler PropertyChanged;
		public void OnPropertyChanged([CallerMemberName] string prop = "")
		{
			if (PropertyChanged != null)
				PropertyChanged(this, new PropertyChangedEventArgs(prop));
		}
	}
}
