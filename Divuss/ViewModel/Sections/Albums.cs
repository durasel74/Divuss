using System;
using System.Collections.ObjectModel;
using Divuss.Model;

namespace Divuss.ViewModel
{
	public class Albums : Section
	{
		private Album currentAlbum;

		#region Singleton конструктор
		private static Albums instance;
		private Albums()
		{
			SectionName = "Albums";
			AlbumsList = new ObservableCollection<Album>();

			AlbumsList = new ObservableCollection<Album>()
			{
				new Album("Album1"),
				new Album("Album2"),
				new Album("Album3"),
				new Album("Album4"),
				new Album("Album5")
			};
		}
		public static Albums GetInstance()
		{
			if (instance == null)
				instance = new Albums();
			return instance;
		} 
		#endregion

		public override string SectionName { get; }
		public ObservableCollection<Album> AlbumsList { get; }

		public Album CurrentAlbum
		{
			get { return currentAlbum; }
			set
			{
				currentAlbum = value;
				OnPropertyChanged("CurrentAlbum");
			}
		}

	}
}
