using System;
using System.Collections.Generic;

namespace Divuss.Model
{
	public class Album
	{
		private List<Picture> pictures = new List<Picture>();


		public Album(string albumName)
		{
			AlbumName = albumName;

		}


		public string AlbumName { get; set; }



		public void Add(Picture picture)
		{
			pictures.Add(picture);
		}

		public void MoveFile()
		{

		}

		public void DeleteFile()
		{
			
		}

	}
}
