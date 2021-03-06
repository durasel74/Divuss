using System;
using System.Collections.ObjectModel;
using Divuss.Service;

namespace Divuss.Model
{
	public class Photos : Section
	{
		//private int maxElementCount = 1000;
		private int picturesCount;

		#region Singleton конструктор
		private static Photos instance;
		private Photos()
		{
			SectionName = "Photos";
			LastPictures = new ObservableCollection<Picture>();
			LoadLastPictures();
			UpdatePicturesCount();
			BootLoader.SaveDataEventHandler += SaveLastPictures;
		}
		public static Photos GetInstance()
		{
			if (instance == null)
				instance = new Photos();
			return instance;
		} 
		#endregion

		public override string SectionName { get; }
		public ObservableCollection<Picture> LastPictures { get; private set; }
		public Picture LastPicture => LastPictures != null ? LastPictures[0] : null;

		public int PicturesCount
		{
			get { return picturesCount; }
			set
			{
				picturesCount = value;
				OnPropertyChanged("PicturesCount");
			}
		}

		public void AddPicturesToLast(string[] paths)
		{
			foreach (var path in paths)
				AddPictureToLastPictures(new Picture(path));
			UpdatePicturesCount();
			Logger.LogTrace($"({SectionName}) Добавлено картинок: {paths.Length}");
		}

		//public void UpdatePictureInLast(Picture picture)
		//{
		//	if (LastPictures.Contains(picture))
		//	{
		//		LastPictures.Remove(picture);
		//		LastPictures.Insert(0, picture);
		//		Logger.LogTrace($"({SectionName}) Обновлена картинка: " +
		//			$"{picture.ImagePath}");
		//	}
		//}

		public void RemovePictureFromLast(Picture picture)
		{
			LastPictures.Remove(picture);
			UpdatePicturesCount();
			Logger.LogTrace($"({SectionName}) Удалена из списка картинка: " +
				$"{picture.ImagePath}");
		}

		public void RemovePicturesFromLast(Picture[] pictures)
		{
			Logger.LogTrace($"({SectionName}) Удаление из последних картинок...");
			int picturesCount = pictures.Length;

			foreach (var picture in pictures)
			{
				LastPictures.Remove(picture);
				Logger.LogTrace($"({SectionName}) Удалена из списка картинка: " +
					$"{picture.ImagePath}");
			}
			UpdatePicturesCount();
			Logger.LogTrace($"({SectionName}) Удалено {picturesCount} картинок из списка");
		}

		/// <summary>
		/// Находит элемент с одинаковым путем к изображению.
		/// </summary>
		/// <param name="path">Путь к изображению.</param>
		/// <returns>Индекс элемента.</returns>
		public int FindPictureWithPath(string path)
		{
			string currentPath;
			for (int i = 0; i < LastPictures.Count; i++)
			{
				currentPath = LastPictures[i].ImagePath;
				if (currentPath == path)
					return i;
			}
			return -1;
		}

		private void AddPictureToLastPictures(Picture picture)
		{
			int indexInElements = FindPictureWithPath(picture.ImagePath);
			if (indexInElements >= 0) LastPictures.RemoveAt(indexInElements);
			LastPictures.Insert(0, picture);
		}

		private void UpdatePicturesCount()
		{
			PicturesCount = LastPictures.Count;
		}

		private void SaveLastPictures()
		{
			PictureData[] lastPicturesData = new PictureData[LastPictures.Count];
			for (int i = 0; i < lastPicturesData.Length; i++)
			{
				if (Picture.PathExists(LastPictures[i].ImagePath))
					lastPicturesData[i] = LastPictures[i].GetPictureData();
			}
			BootLoader.LastSessionData.LastPictures = lastPicturesData;
		}

		private void LoadLastPictures()
		{
			var lastPicturesData = BootLoader.LastSessionData.LastPictures;
			foreach (var pictureData in lastPicturesData)
			{
				if (Picture.PathExists(pictureData.ImagePath))
					LastPictures.Add(pictureData.CreatePicture());
			}
		}
	}
}
