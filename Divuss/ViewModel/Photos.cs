using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Collections.ObjectModel;
using Divuss.Model;

namespace Divuss.ViewModel
{
	internal class Photos
	{
        public ObservableCollection<Picture> LastPictures { get; set; }

        public Photos()
        {
            LastPictures = new ObservableCollection<Picture>() 
            {
				new Picture(@"D:\закачки\новые\Gradietns\Gradient_Biruz.jpg"),
				new Picture(@"D:\закачки\новые\Gradietns\Gradient_Blue.jpg"),
				new Picture(@"D:\закачки\новые\Gradietns\Gradient_Lighting.jpg"),
				new Picture(@"D:\закачки\новые\Gradietns\Gradient_Violet.jpg")
			};
		}

		public void AddToLast(string[] picturePaths)
		{
			foreach (string picturePath in picturePaths)
			{
				LastPictures.Add(new Picture(picturePath));
			}
		}

		//public void RemoveFromLast(string[] picturePaths)
		//{
		//	foreach (string picturePath in picturePaths)
		//	{
		//		LastPictures.Remove(picturePath);
		//	}
		//}


		public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
