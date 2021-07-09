using System;
using System.Collections.ObjectModel;
using Divuss.Model;

namespace Divuss.ViewModel
{
	internal class Albums : Section
	{
		private static Albums instance;

		private Albums()
		{
			SectionName = "Альбомы";
			AlbumsList = new ObservableCollection<Album>();
		}

		public override string SectionName { get; }

		public ObservableCollection<Album> AlbumsList { get; }

		public static Albums GetInstance()
		{
			if (instance == null)
				instance = new Albums();
			return instance;
		}
	}
}
