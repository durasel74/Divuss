using System;
using System.Collections.ObjectModel;
using Divuss.Service;

namespace Divuss.Model
{
	public class Photos : Section
	{
		//private int maxElementCount = 1000;

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

		public void AddPictureToLast(string path)
		{
			int indexInLastPictures = FindPictureWithPath(path);
			if (indexInLastPictures >= 0)
				LastPictures.RemoveAt(indexInLastPictures);
			LastPictures.Insert(0, new Picture(path));
			Logger.LogTrace($"({SectionName}) Добавлена картинка: {path}");
		}

		public void UpdatePictureInLast(Picture picture)
		{
			if (LastPictures.Contains(picture))
			{
				LastPictures.Remove(picture);
				LastPictures.Insert(0, picture);
				Logger.LogTrace($"({SectionName}) Обновлена картинка: " +
					$"{picture.ImagePath}");
			}
		}

		public void RemovePicturesFromLast(Picture[] pictures)
		{
			Logger.LogTrace($"({SectionName}) Удаление последних картинок...");
			int picturesCount = pictures.Length;

			foreach (var picture in pictures)
			{
				LastPictures.Remove(picture);
				Logger.LogTrace($"({SectionName}) Удалена из списка картинка: {picture.ImagePath}");
			}
			Logger.LogTrace($"({SectionName}) Удалено из списка картинок: {picturesCount}");
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
	}
}
