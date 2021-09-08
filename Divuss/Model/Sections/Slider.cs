using System;

namespace Divuss.Model
{
	public class Slider : Section
	{
		private Picture firstPicture;
		private Picture secondPicture;

		#region Singleton конструктор
		private static Slider instance;
		private Slider()
		{
			SectionName = "Slider";
		}
		public static Slider GetInstance()
		{
			if (instance == null)
				instance = new Slider();
			return instance;
		}
		#endregion

		public override string SectionName { get; }

		public Picture FirstPicture
		{
			get { return firstPicture; }
			set
			{
				firstPicture = value;
				OnPropertyChanged("FirstPicture");
			}
		}

		public Picture SecondPicture
		{
			get { return secondPicture; }
			set
			{
				secondPicture = value;
				OnPropertyChanged("SecondPicture");
			}
		}

		public void AddPicturesToSlider(Picture[] pictures)
		{
			if (pictures.Length == 2)
			{
				FirstPicture = new Picture(pictures[0].ImagePath);
				SecondPicture = new Picture(pictures[1].ImagePath);
			}
		}

		public void RestartView()
		{
			FirstPicture = null;
			SecondPicture = null;
		}

		public void SwapPictures()
		{
			var temp = FirstPicture;
			FirstPicture = SecondPicture;
			SecondPicture = temp;
		}
	}
}
