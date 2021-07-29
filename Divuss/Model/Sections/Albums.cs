using System;
using System.Collections.ObjectModel;
using Divuss.Service;

namespace Divuss.Model
{
	public class Albums : Section
	{
		private Album currentAlbum;
		private bool pictureListIsVisibility;
		private int nameNoveltyCounter;
		private int albumsCount;
		private bool isCopyMove;
		private string renameBuffer;

		#region Singleton конструктор
		private static Albums instance;
		private Albums()
		{
			SectionName = "Albums";
			nameNoveltyCounter = 1;
			AlbumsList = new ObservableCollection<Album>();
			PictureListIsVisibility = false;
			IsCopyMove = false;

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
			UpdateAlbumsCount();
		}
		public static Albums GetInstance()
		{
			if (instance == null)
				instance = new Albums();
			return instance;
		} 
		#endregion

		public override string SectionName { get; }
		public int NameNoveltyCounter { get => nameNoveltyCounter++;  }
		public ObservableCollection<Album> AlbumsList { get; }
		public Picture[] AddBuffer { get; set; }
		public int AddBufferCount => AddBuffer.Length;
		public ObservableCollection<Album> AlbumsBuffer { get; set; }

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

		public int AlbumsCount
		{
			get { return albumsCount; }
			set
			{
				albumsCount = value;
				OnPropertyChanged("AlbumsCount");
			}
		}

		public bool IsCopyMove
		{
			get { return isCopyMove; }
			set
			{
				isCopyMove = value;
				OnPropertyChanged("IsCopyMove");
			}
		}

		public string RenameBuffer
		{
			get { return renameBuffer; }
			set
			{
				renameBuffer = value;
				OnPropertyChanged("RenameBuffer");
			}
		}

		public void OpenAlbum(Album album)
		{
			CurrentAlbum = album;
			PictureListIsVisibility = true;
			ViewModel.ViewModel.ClearAllSelection();
			Logger.LogTrace($"({SectionName}) Открыт просмотр альбома: {album.AlbumName}");
		}

		public void CloseAlbum()
		{
			PictureListIsVisibility = false;
			CurrentAlbum = null;
			ViewModel.ViewModel.ClearAllSelection();
			Logger.LogTrace($"({SectionName}) Просмотр альбома закрыт");
		}

		public void CreateAlbum()
		{
			string newAlbumName = $"New Album {NameNoveltyCounter}";
			var newAlbum = new Album(newAlbumName);
			AlbumsList.Add(newAlbum);
			UpdateAlbumsCount();
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
			UpdateAlbumsCount();
			Logger.LogTrace($"({SectionName}) Удалено {albumsCount} альбомов");
		}

		private void UpdateAlbumsCount()
		{
			AlbumsCount = AlbumsList.Count;
		}
	}
}
