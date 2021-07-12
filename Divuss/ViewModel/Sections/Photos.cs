using System;
using System.Collections.ObjectModel;
using Divuss.Model;

using System.IO;

namespace Divuss.ViewModel
{
	internal class Photos : Section
	{
		//private int maxElementCount = 1000;
		private bool pictureViewIsVisibility;
		private Picture currentPicture;

		#region Singleton constructor
		private static Photos instance;
		private Photos()
		{
			SectionName = "Фотографии";
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
		/// Determines if the image is currently open in view mode.
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
				currentPicture = value;
				OnPropertyChanged("CurrentPicture");
			}
		}

		/// <summary>
		/// Continuation of the command to switch the picture view mode.
		/// </summary>
		/// <param name="obj">Command parameter.</param>
		public void PictureSwitch(object obj)
		{
			if (obj is Picture)
			{
				PictureViewIsVisibility = true;
				CurrentPicture = (Picture)obj;
			}
			else
			{
				PictureViewIsVisibility = false;
				CurrentPicture = null;
			}
		}
	}
}
