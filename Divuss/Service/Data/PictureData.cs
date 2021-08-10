using System;
using Divuss.Model;

namespace Divuss.Service
{
	internal struct PictureData
	{
		public PictureData(string path)
		{
			ImagePath = path;
		}

		public string ImagePath { get; set; }

		public Picture CreatePicture()
		{
			return new Picture(ImagePath);
		}
	}
}
