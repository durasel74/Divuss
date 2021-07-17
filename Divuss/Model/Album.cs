using System;
using System.Collections.ObjectModel;


namespace Divuss.Model
{
	public class Album : NotifyPropertyChanged
	{
		private const int MAX_ALBUM_NAME_LENGTH = 50;

		private string albumName;
		private ObservableCollection<Picture> elements;

		public Album(string albumName)
		{
			AlbumName = albumName;
			elements = new ObservableCollection<Picture>();
		}

		public string AlbumName
		{
			get { return albumName; }
			set
			{
				string newName = value;
				if (newName.Length <= MAX_ALBUM_NAME_LENGTH)
				{
					albumName = newName;
					OnPropertyChanged("AlbumName");
				}
			}
		}

		public void AddPictures(Picture[] pictures)
		{
			foreach (Picture picture in pictures)
			{
				elements.Add(picture);
			}
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
	}
}
