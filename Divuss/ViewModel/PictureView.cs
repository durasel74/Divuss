using System;
using System.Collections.ObjectModel;
using Divuss.Model;
using Divuss.Service;

namespace Divuss.ViewModel
{
	internal enum BufferMove
	{ 
		Previous,
		Next
	}

	internal class PictureView : NotifyPropertyChanged
	{
		private bool pictureViewIsVisibility;
		private Picture currentPicture;

		#region Singleton конструктор
		private static PictureView instance;
		private PictureView()
		{
			SectionName = "PictureView";
			PicturesBuffer = new ObservableCollection<Picture>();
			PictureViewIsVisibility = false;
		}
		public static PictureView GetInstance()
		{
			if (instance == null)
				instance = new PictureView();
			return instance;
		}
		#endregion

		public string SectionName { get; }
		public ObservableCollection<Picture> PicturesBuffer { get; set; }

		/// <summary>
		/// Определяет, открыто ли в данный момент изображение в режиме просмотра.
		/// </summary>
		public bool PictureViewIsVisibility
		{
			get { return pictureViewIsVisibility; }
			private set
			{
				pictureViewIsVisibility = value;
				OnPropertyChanged("PictureViewIsVisibility");
			}
		}
		
		public Picture CurrentPicture
		{
			get { return currentPicture; }
			private set
			{
				currentPicture = value;
				OnPropertyChanged("CurrentPicture");
			}
		}

		public void AddPicturesToBuffer(ObservableCollection<Picture> pictures)
		{
			PicturesBuffer = pictures;
		}

		public void OpenPicture(Picture picture)
		{
			if (picture == null || !PicturesBuffer.Contains(picture))
				return;

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

		public void MoveBuffer(BufferMove move)
		{
			switch (move)
			{
				case BufferMove.Previous:
					MovePrevious();
					break;
				case BufferMove.Next:
					MoveNext();
					break;
			}
		}

		private void MovePrevious()
		{
			var currentPictureIndex = PicturesBuffer.IndexOf(CurrentPicture);
			if (currentPictureIndex == 0) return;
			OpenPicture(PicturesBuffer[currentPictureIndex - 1]);
		}

		private void MoveNext()
		{
			var currentPictureIndex = PicturesBuffer.IndexOf(CurrentPicture);
			if (currentPictureIndex == PicturesBuffer.Count - 1) return;
			OpenPicture(PicturesBuffer[currentPictureIndex + 1]);
		}
	}
}
