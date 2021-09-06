using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using Divuss.Service;

namespace Divuss.Model
{
	public class Album : NotifyPropertyChanged
	{
		private const int MAX_ALBUM_NAME_LENGTH = 50;

		private string albumsDirPath;
		private string albumName;
		private DirectoryInfo albumDir;
		private Picture currentElement;
		private int picturesCount;
		private bool isRenaming;

		public Album(string albumName, string albumPath)
		{
			albumsDirPath = BootLoader.GetAlbumsFolderPath();
			AlbumName = albumName;
			AlbumPath = albumPath;

			if (!Directory.Exists(albumPath)) throw new Exception("Альбома нет"); // <------
			albumDir = new DirectoryInfo(albumPath);

			Elements = new ObservableCollection<Picture>();
			UpdatePicturesCount();
		}

		public string AlbumPath { get; private set; }
		public ObservableCollection<Picture> Elements { get; set; }

		public string AlbumName
		{
			get { return albumName; }
			set
			{
				string newName = value;
				if (newName.Length == 0 || newName == albumName) return;
				Logger.LogTrace($"Альбом {albumName} переименован в: {newName}");
				if (newName.Length <= MAX_ALBUM_NAME_LENGTH)
					albumName = newName;
				else
					albumName = newName.Substring(0, MAX_ALBUM_NAME_LENGTH);

				albumDir?.MoveTo(albumsDirPath + "\\" + albumName);
				AlbumPath = albumDir?.FullName;
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

		public void AddPicturesFromFile(string[] paths)
		{
			Picture picture;
			foreach (var path in paths)
			{
				picture = new Picture(path);
				AddPictureToAlbum(picture, this);
			}
			UpdatePicturesCount();
			Logger.LogTrace($"(Альбом {albumName}) Импортировано картинок: {paths.Length}");
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

		public void CopyPicture(Picture[] pictures, Album otherAlbum)
		{
			Logger.LogTrace($"(Альбом {albumName}) Копирование картинок в " +
				$"альбом: {otherAlbum.AlbumName}...");
			int picturesCount = pictures.Length;

			foreach (Picture picture in pictures)
			{
				AddPictureToAlbum(picture, otherAlbum);
				Logger.LogTrace($"(Альбом {albumName}) Копирована картинка: " +
					$"{picture.ImagePath}");
			}
			UpdatePicturesCount();
			otherAlbum.UpdatePicturesCount();
			Logger.LogTrace($"(Альбом {albumName}) Копировано {picturesCount} " +
				$"картинок в альбом {otherAlbum.AlbumName}");
		}

		public void MovePictures(Picture[] pictures, Album otherAlbum)
		{
			Logger.LogTrace($"(Альбом {albumName}) Перемещение картинок в " +
				$"альбом: {otherAlbum.AlbumName}...");
			int picturesCount = pictures.Length;

			foreach (Picture picture in pictures)
			{
				AddPictureToAlbum(picture, otherAlbum);
				Logger.LogTrace($"(Альбом {albumName}) Перемещена картинка: " +
					$"{picture.ImagePath}");
			}
			foreach (var picture in pictures)
			{
				Elements.Remove(picture);
				//File.Delete(picture.ImagePath);
			}

			UpdatePicturesCount();
			otherAlbum.UpdatePicturesCount();
			Logger.LogTrace($"(Альбом {albumName}) Перемещено {picturesCount} " +
				$"картинок в альбом {otherAlbum.AlbumName}");
		}

		public void DeletePicture(Picture picture)
		{
			if (!Elements.Contains(picture)) throw new Exception("Этой картинки нет в альбоме"); // <-----------------

			Elements.Remove(picture);
			//File.Delete(picture.ImagePath);
			UpdatePicturesCount();
			Logger.LogTrace($"(Альбом {albumName}) Удалена картинка: " +
				$"{picture.ImagePath}");
		}

		public void DeletePictures(Picture[] pictures)
		{
			Logger.LogTrace($"(Альбом {albumName}) Удаление картинок...");
			int picturesCount = pictures.Length;
			foreach (var picture in pictures) DeletePicture(picture);
			Logger.LogTrace($"(Альбом {albumName}) Удалено {picturesCount} " +
				$"картинок");
		}

		/// <summary>
		/// Выдает информацию о альбоме.
		/// </summary>
		/// <returns>Строка с информацией.</returns>
		public string GetAlbumInfo()
		{
			DirectoryInfo directoryInfo = new DirectoryInfo(AlbumPath);
			var creationTime = directoryInfo.CreationTime;
			var lastAccessTime = directoryInfo.LastAccessTime;
			var picturesCount = Elements.Count;

			string output =
			$"Название альбома: {AlbumPath}\n" +
			$"Дата создания: {creationTime}\n" +
			$"Дата открытия: {lastAccessTime}\n" +
			$"Количество изображений: {picturesCount}";

			Logger.LogTrace("Выведены сведения о альбоме");
			return output;
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

		internal AlbumData GetAlbumData()
		{
			return new AlbumData(AlbumName, AlbumPath, ElementsToData());
		}

		private static void AddPictureToAlbum(Picture picture, Album album)
		{
			int indexInElements = album.FindPictureWithPath(picture.ImagePath);
			if (indexInElements >= 0)
			{
				var sameElement = album.Elements[indexInElements];
				album.Elements.Remove(sameElement);
			}
			album.SavePictureToUserDir(picture, album);
			album.Elements.Insert(0, picture);
		}

		private void UpdatePicturesCount()
		{
			PicturesCount = Elements.Count;
		}

		private PictureData[] ElementsToData()
		{
			PictureData[] pictureData = new PictureData[Elements.Count];
			for (int i = 0; i < Elements.Count; i++)
				pictureData[i] = Elements[i].GetPictureData();
			return pictureData;
		}

		private Picture SavePictureToUserDir(Picture picture, Album album)
		{
			var fileInfo = new FileInfo(picture.ImagePath);
			//string newPath = album.AlbumPath + "\\" + fileInfo.Name;
			//File.Copy(picture.ImagePath, newPath);
			//var newPicture = new Picture(newPath);
			//return newPicture;
			return picture;
		}
	}
}
