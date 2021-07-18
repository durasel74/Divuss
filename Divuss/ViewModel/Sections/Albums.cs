using System;
using System.Collections.ObjectModel;
using Divuss.Model;
using Divuss.Service;

namespace Divuss.ViewModel
{
	public class Albums : Section
	{
		private Album currentAlbum;
		private bool pictureListIsVisibility;
		private int nameNoveltyCounter;

		#region Singleton конструктор
		private static Albums instance;
		private Albums()
		{
			SectionName = "Albums";
			nameNoveltyCounter = 1;
			AlbumsList = new ObservableCollection<Album>();
			PictureListIsVisibility = false;

			Album testAlbum = new Album("TestAlbum");
			testAlbum.AddPictures(new Picture[]
			{
				new Picture(@"D:\закачки\картинки\Gradients\Gradient_Biruz.jpg"),
				new Picture(@"D:\закачки\картинки\Gradients\Gradient_Blue.jpg"),
				new Picture(@"D:\закачки\картинки\Gradients\Gradient_Violet.jpg")
			});
			AlbumsList = new ObservableCollection<Album>()
			{
				testAlbum,
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
		public int NameNoveltyCounter { get => nameNoveltyCounter++;  }

		public Album CurrentAlbum
		{
			get { return currentAlbum; }
			set
			{
				currentAlbum = value;
				OnPropertyChanged("CurrentAlbum");
			}
		}

		public bool PictureListIsVisibility
		{
			get { return pictureListIsVisibility; }
			set
			{
				pictureListIsVisibility = value;
				OnPropertyChanged("PictureListIsVisibility");
			}
		}

		public void OpenAlbum(Album album)
		{
			CurrentAlbum = album;
			PictureListIsVisibility = true;
			Logger.LogTrace($"({SectionName}) Открыт просмотр альбома: {album.AlbumName}");
		}

		public void CloseAlbum()
		{
			PictureListIsVisibility = false;
			CurrentAlbum = null;
			Logger.LogTrace($"({SectionName}) Просмотр альбома закрыт");
		}

		public void CreateAlbum()
		{
			string newAlbumName = $"New Album {NameNoveltyCounter}";
			var newAlbum = new Album(newAlbumName);
			AlbumsList.Add(newAlbum);
			Logger.LogTrace($"({SectionName}) Создан альбом: {newAlbumName}");
		}

		public void DeleteAlbums(Album[] albums)
		{
			Logger.LogTrace($"({SectionName}) Удаление альбомов...");
			int albumsCount = albums.Length;

			foreach (var album in albums)
			{
				AlbumsList.Remove(album);
				Logger.LogTrace($"({SectionName}) Удален альбом: {album.AlbumName}");
			}
			Logger.LogTrace($"({SectionName}) Удалено альбомов: {albumsCount}");
		}
	}
}
