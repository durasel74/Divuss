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

			//string[] AllFiles = Directory.GetFiles(@"D:\закачки\картинки\New Wave", "*.*", SearchOption.AllDirectories);
			//foreach (string filename in AllFiles)
			//{
			//	LastPictures.Add(new Picture(filename));
			//}

			LastPictures = new ObservableCollection<Picture>()
			{
				new Picture(@"D:\закачки\картинки\Gradients\Gradient_Biruz.jpg"),
				new Picture(@"D:\закачки\картинки\Gradients\Gradient_Blue.jpg"),
				new Picture(@"D:\закачки\картинки\Gradients\Gradient_Lighting.jpg"),
				new Picture(@"D:\закачки\картинки\Gradients\Gradient_Violet.jpg")
			};

			UpdatePicturesCount();
		}
		public static Photos GetInstance()
		{
			if (instance == null)
				instance = new Photos();
			return instance;
		} 
		#endregion

		public override string SectionName { get; }
		public ObservableCollection<Picture> LastPictures { get; }
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
	}
}
