using System;
using System.IO;

namespace Divuss.Model
{
	public class Picture : MediaElement
	{
		private string imagePath;


		public Picture(string imagePath)
		{
			ImageExists(imagePath);
			ImagePath = imagePath;
		}


		public string ImagePath
		{
			get
			{
				ImageExists(imagePath);
				return imagePath;
			}
			private set
			{
				var newPath = value;
				ImageExists(newPath);
				imagePath = newPath;
			}
		}

		private void ImageExists(string path)
		{
			if (!File.Exists(path))
				throw new Exception();
		}
	}
}
