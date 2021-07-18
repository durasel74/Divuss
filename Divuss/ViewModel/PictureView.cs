using System;
using System.Collections.ObjectModel;

using Divuss.Model;
using Divuss.Service;

namespace Divuss.ViewModel
{
	public class PictureView : NotifyPropertyChanged
	{
		private bool pictureViewIsVisibility;
		private Picture currentPicture;

		#region Singleton конструктор
		private static PictureView instance;
		private PictureView()
		{
			SectionName = "PictureView";
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

		/// <summary>
		/// The current image is open in view mode.
		/// </summary>
		public Picture CurrentPicture
		{
			get { return currentPicture; }
			private set
			{
				currentPicture = value;
				OnPropertyChanged("CurrentPicture");
			}
		}

		public void OpenPicture(Picture picture)
		{
			if (picture == null)
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
	}
}
