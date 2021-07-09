using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Collections.Generic;

namespace Divuss.Model
{
	public class Album : INotifyPropertyChanged
	{
		private int maxAlbumNameLength = 50;

		private string albumName;
		private List<Picture> elements = new List<Picture>();

		public Album(string albumName)
		{
			AlbumName = albumName;
		}

		public string AlbumName 
		{ 
			get { return albumName; }
			set
			{
				string newName = value;
				if (newName.Length <= maxAlbumNameLength)
				{
					albumName = newName;
					OnPropertyChanged("AlbumName");
				}
			}
		}

		public void AddPictures(Picture[] pictures)
		{
			elements.AddRange(pictures);
		}

		public void MovePictures(Picture[] pictures, Album album)
		{
			album.AddPictures(pictures);
			DeletePicture(pictures);
		}

		public void CopyPicture(Picture[] pictures, Album album)
		{
			album.AddPictures(pictures);
		}

		public void DeletePicture(Picture[] pictures)
		{
			foreach (Picture picture in pictures)
			{
				elements.Remove(picture);
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
