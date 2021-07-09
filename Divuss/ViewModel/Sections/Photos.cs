using System;
using System.Collections.ObjectModel;
using Divuss.Model;

namespace Divuss.ViewModel
{
	internal class Photos : Section
	{
		private static Photos instance;
		//private int maxElementCount = 1000;

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
		}

		public override string SectionName { get; }

		public ObservableCollection<Picture> LastPictures { get; }

		public static Photos GetInstance()
		{
			if (instance == null)
				instance = new Photos();
			return instance;
		}
	}
}
