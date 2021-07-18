using System;
using System.Collections.ObjectModel;
using Divuss.Service;

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

		public void AddPictureFromFile(string path)
		{
			int indexInLastPictures = FindPictureWithPath(path);
			if (indexInLastPictures >= 0)
				Elements.RemoveAt(indexInLastPictures);
			Elements.Insert(0, new Picture(path));
			Logger.LogTrace($"(Альбом {albumName}) Импортирована картинка: {path}");
		}




		public void AddPictures(Picture[] pictures)
		{
			foreach (Picture picture in pictures)
			{
				Elements.Insert(0, picture);
			}
		}

		//public void MovePictures(Picture[] pictures, Album album)
		//{
		//	album.AddPictures(pictures);
		//	DeletePicture(pictures);
		//}

		//public void CopyPicture(Picture[] pictures, Album album)
		//{
		//	album.AddPictures(pictures);
		//}

		//public void DeletePicture(Picture[] pictures)
		//{
		//	foreach (Picture picture in pictures)
		//	{
		//		Elements.Remove(picture);
		//	}
		//}




		/// <summary>
		/// Находит элемент с одинаковым путем к изображению.
		/// </summary>
		/// <param name="path">Путь к изображению.</param>
		/// <returns>Индекс элемента.</returns>
		public int FindPictureWithPath(string path)
		{
			string currentPath;
			for (int i = 0; i < Elements.Count; i++)
			{
				currentPath = Elements[i].ImagePath;
				if (currentPath == path)
					return i;
			}
			return -1;
		}
	}
}
