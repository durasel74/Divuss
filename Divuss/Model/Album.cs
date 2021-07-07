using System;
using System.Collections.Generic;

namespace Divuss.Model
{
	public class Album
	{
		private List<Picture> elements = new List<Picture>();


		public Album(string albumName)
		{
			AlbumName = albumName;

		}


		public string AlbumName { get; set; }



		public void AddPictures(Picture[] pictures)
		{
			elements.AddRange(pictures);
		}

		public void MovePictures(Picture[] pictures, Album album)
		{
			album.AddPictures(pictures);
			DeletePicture(pictures);
		}

		public void CopyPicture(Picture[] pictures, Album album)
		{
			album.AddPictures(pictures);
		}

		public void DeletePicture(Picture[] pictures)
		{
			foreach (Picture picture in pictures)
			{
				elements.Remove(picture);
			}
		}
	}
}
