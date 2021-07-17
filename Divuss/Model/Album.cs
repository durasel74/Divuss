using System;
using System.Collections.ObjectModel;

namespace Divuss.Model
{
	public class Album : NotifyPropertyChanged
	{
		private const int MAX_ALBUM_NAME_LENGTH = 50;

		private string albumName;
		private Picture currentElement;

		public Album(string albumName)
		{
			AlbumName = albumName;
			Elements = new ObservableCollection<Picture>();
		}

		public ObservableCollection<Picture> Elements { get; }

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

		public Picture CurrentElement
		{
			get { return currentElement; }
			set
			{
				currentElement = value;
				OnPropertyChanged("CurrentElement");
			}
		}




		public void AddPictures(Picture[] pictures)
		{
			foreach (Picture picture in pictures)
			{
				Elements.Add(picture);
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
				Elements.Remove(picture);
			}
		}
	}
}
