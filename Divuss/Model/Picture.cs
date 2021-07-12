using System;
using System.IO;

namespace Divuss.Model
{
	public class Picture
	{
		private string imagePath;

		public Picture(string imagePath)
		{
			ImagePath = imagePath;
		}

		public string ImagePath
		{
			get
			{
				if (!PathExists())
					throw new MediaElementNotFoundException(imagePath);
				return imagePath;
			}
			private set
			{
				var newPath = value;
				if (!PathExists(value))
					throw new MediaElementNotFoundException(newPath);
				imagePath = newPath;
			}
		}

		public static bool PathExists(string path) => File.Exists(path);
		public bool PathExists() => File.Exists(imagePath);
	}
}
