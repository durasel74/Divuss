using System;
using System.Collections.Generic;
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
				if (!PathExists(imagePath))
					throw new MediaElementNotFoundException(imagePath);
				return imagePath;
			}
			private set
			{
				var newPath = value;
				if (!PathExists(imagePath))
					throw new MediaElementNotFoundException(imagePath);
				imagePath = newPath;
			}
		}

		public bool PathExists(string path)
		{
			if (File.Exists(path)) return true;
			else return false;
		}
	}
}
