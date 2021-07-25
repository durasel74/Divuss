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
		private int picturesCount;

		public Album(string albumName)
		{
			AlbumName = albumName;
			Elements = new ObservableCollection<Picture>();
			UpdatePicturesCount();
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

		public int PicturesCount
		{
			get { return picturesCount; }
			set
			{
				picturesCount = value;
				OnPropertyChanged("PicturesCount");
			}
		}

		public void AddPictureFromFile(string path)
		{
			int indexInLastPictures = FindPictureWithPath(path);
			if (indexInLastPictures >= 0)
				Elements.RemoveAt(indexInLastPictures);
			Elements.Insert(0, new Picture(path));
			UpdatePicturesCount();
			Logger.LogTrace($"(Альбом {albumName}) Импортирована картинка: {path}");
		}





		public void AddPictures(Picture[] pictures)
		{
			foreach (Picture picture in pictures)
			{
				Elements.Insert(0, picture);
			}
			UpdatePicturesCount();
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






		public void DeletePicture(Picture picture)
		{
			Elements.Remove(picture);
			UpdatePicturesCount();
			Logger.LogTrace($"(Альбом {albumName}) Удалена картинка: " +
				$"{picture.ImagePath}");
		}

		public void DeletePictures(Picture[] pictures)
		{
			Logger.LogTrace($"(Альбом {albumName}) Удаление картинок...");
			int picturesCount = pictures.Length;

			foreach (var picture in pictures)
			{
				Elements.Remove(picture);
				Logger.LogTrace($"(Альбом {albumName}) Удалена картинка: " +
					$"{picture.ImagePath}");
			}
			UpdatePicturesCount();
			Logger.LogTrace($"(Альбом {albumName}) Удалено картинок: " +
				$"{picturesCount}");
		}

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

		private void UpdatePicturesCount()
		{
			PicturesCount = Elements.Count;
		}
	}
}
