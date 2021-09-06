using System;
using System.Collections.ObjectModel;
using Divuss.Model;

namespace Divuss.Service
{
	internal struct AlbumData
	{
		public AlbumData(string albumName, string albumPath,
			PictureData[] elements)
		{
			AlbumName = albumName;
			AlbumPath = albumPath;
			Elements = elements;
		}

		public string AlbumName { get; set; }
		public string AlbumPath { get; set; }
		public PictureData[] Elements { get; set; }

		public Album CreateAlbum()
		{
			var album = new Album(AlbumName, AlbumPath);
			album.Elements = DataToElements();
			return album;
		}

		private ObservableCollection<Picture> DataToElements()
		{
			ObservableCollection<Picture> elements = new ObservableCollection<Picture>();
			foreach (var element in Elements)
				elements.Add(element.CreatePicture());
			return elements;
		}
	}
}
