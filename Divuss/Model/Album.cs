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
		private bool isRenaming;

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
					albumName = newName;
				else
					albumName = newName.Substring(0, MAX_ALBUM_NAME_LENGTH);
				OnPropertyChanged("AlbumName");
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

		public bool IsRenaming
		{
			get { return isRenaming; }
			set
			{
				isRenaming = value;
				OnPropertyChanged("IsRenaming");
			}
		}

		public void AddPictureFromFile(string path)
		{
			AddPictureToAlbum(new Picture(path), this);
			UpdatePicturesCount();
			Logger.LogTrace($"(Альбом {albumName}) Импортирована картинка: {path}");
		}

		public void AddPictures(Picture[] pictures)
		{
			Logger.LogTrace($"(Альбом {albumName}) Добавление картинок в альбом...");
			int picturesCount = pictures.Length;

			foreach (Picture picture in pictures)
			{
				AddPictureToAlbum(picture, this);
				Logger.LogTrace($"(Альбом {albumName}) Добавлена картинка: " +
					$"{picture.ImagePath}");
			}
			UpdatePicturesCount();
			Logger.LogTrace($"(Альбом {albumName}) Добавлено {picturesCount} картинок");
		}

		public void MovePictures(Picture[] pictures, Album album)
		{
			Logger.LogTrace($"(Альбом {albumName}) Перемещение картинок в " +
				$"альбом: {album.AlbumName}...");
			int picturesCount = pictures.Length;

			foreach (Picture picture in pictures)
			{
				AddPictureToAlbum(picture, album);
				Logger.LogTrace($"(Альбом {albumName}) Перемещена картинка: " +
					$"{picture.ImagePath}");
			}
			foreach (var picture in pictures)
				Elements.Remove(picture);

			UpdatePicturesCount();
			album.UpdatePicturesCount();
			Logger.LogTrace($"(Альбом {albumName}) Перемещено {picturesCount} " +
				$"картинок в альбом {album.AlbumName}");
		}

		public void CopyPicture(Picture[] pictures, Album album)
		{
			Logger.LogTrace($"(Альбом {albumName}) Копирование картинок в " +
				$"альбом: {album.AlbumName}...");
			int picturesCount = pictures.Length;

			foreach (Picture picture in pictures)
			{
				AddPictureToAlbum(picture, album);
				Logger.LogTrace($"(Альбом {albumName}) Копирована картинка: " +
					$"{picture.ImagePath}");
			}
			UpdatePicturesCount();
			album.UpdatePicturesCount();
			Logger.LogTrace($"(Альбом {albumName}) Копировано {picturesCount} " +
				$"картинок в альбом {album.AlbumName}");
		}

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
			Logger.LogTrace($"(Альбом {albumName}) Удалено {picturesCount} " +
				$"картинок");
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

		private static void AddPictureToAlbum(Picture picture, Album album)
		{
			int indexInElements = album.FindPictureWithPath(picture.ImagePath);
			if (indexInElements >= 0) album.Elements.RemoveAt(indexInElements);
			album.Elements.Insert(0, picture);
		}

		private void UpdatePicturesCount()
		{
			PicturesCount = Elements.Count;
		}
	}
}
