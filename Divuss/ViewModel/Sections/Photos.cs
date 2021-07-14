using System;
using System.Collections.ObjectModel;
using Divuss.Model;
using Divuss.Service;

//using System.IO;

namespace Divuss.ViewModel
{
	public class Photos : Section
	{
		//private int maxElementCount = 1000;
		private bool pictureViewIsVisibility;
		private Picture currentPicture;

		#region Singleton конструктор
		private static Photos instance;
		private Photos()
		{
			SectionName = "Photos";
			PictureViewIsVisibility = false;

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

		public ObservableCollection<Picture> LastPictures { get; }
		public override string SectionName { get; }

		/// <summary>
		/// Определяет, открыто ли в данный момент изображение в режиме просмотра.
		/// </summary>
		public bool PictureViewIsVisibility
		{
			get { return pictureViewIsVisibility; }
			set
			{
				pictureViewIsVisibility = value;
				OnPropertyChanged("PictureViewIsVisibility");
			}
		}

		/// <summary>
		/// The current image is open in view mode.
		/// </summary>
		public Picture CurrentPicture
		{
			get { return currentPicture; }
			set
			{
				var newLastPicture = value;
				currentPicture = newLastPicture;
				OnPropertyChanged("CurrentPicture");
				UpdatePictureInLast(newLastPicture);
			}
		}

		public void OpenPicture(Picture picture)
		{
			CurrentPicture = picture;
			PictureViewIsVisibility = true;
			Logger.LogTrace($"({SectionName}) Открыт просмотр картинки: {picture.ImagePath}");
		}

		public void ClosePicture()
		{
			PictureViewIsVisibility = false;
			CurrentPicture = null;
			Logger.LogTrace($"({SectionName}) Просмотр картинки закрыт");
		}

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

		public void RemovePictureFromLast(Picture picture)
		{
			LastPictures.Remove(picture);
			Logger.LogTrace($"({SectionName}) Удалена картинка: " +
					$"{picture.ImagePath}");
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
