using System;
using System.Collections.ObjectModel;
using Divuss.Model;

namespace Divuss.ViewModel
{
	internal class Photos : Section
	{
		private static Photos instance;
		//private int maxElementCount = 1000;

		private Picture currentPicture;

		private Photos()
		{
			SectionName = "Фотографии";
			LastPictures = new ObservableCollection<Picture>()
			{
				new Picture(@"D:\закачки\новые\Gradietns\Gradient_Biruz.jpg"),
				new Picture(@"D:\закачки\новые\Gradietns\Gradient_Blue.jpg"),
				new Picture(@"D:\закачки\новые\Gradietns\Gradient_Lighting.jpg"),
				new Picture(@"D:\закачки\новые\Gradietns\Gradient_Violet.jpg")
			};
			CurrentPicture = LastPictures[1];
		}

		public override string SectionName { get; }

		public ObservableCollection<Picture> LastPictures { get; }

		public Picture CurrentPicture
		{
			get { return currentPicture; }
			set
			{
				currentPicture = value;
				OnPropertyChanged("CurrentPicture");
			}
		}


		public static Photos GetInstance()
		{
			if (instance == null)
				instance = new Photos();
			return instance;
		}
	}
}
