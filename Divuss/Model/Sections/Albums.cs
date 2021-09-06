using System;
using System.IO;
using System.Collections.ObjectModel;
using Divuss.Service;

namespace Divuss.Model
{
	public class Albums : Section
	{
		private string albumsDirPath;
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
			albumsDirPath = BootLoader.GetAlbumsFolderPath();
			SectionName = "Albums";
			nameNoveltyCounter = 1;
			PictureListIsVisibility = false;
			IsCopyMove = false;
			AlbumsList = new ObservableCollection<Album>();
			UpdateAlbumsCount();
			BootLoader.SaveAlbumsEventHandler += SaveAlbums;
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

			string albumPath = albumsDirPath + "\\" + newAlbumName;
			Directory.CreateDirectory(albumPath);
			
			var newAlbum = new Album(newAlbumName, albumPath);
			AlbumsList.Add(newAlbum);
			UpdateAlbumsCount();
			AlbumsBuffer = new ObservableCollection<Album>() { newAlbum };
			Logger.LogTrace($"({SectionName}) Создан альбом: {newAlbumName}");
		}

		public void DeleteAlbum(Album album)
		{
			AlbumsList.Remove(album);

			Directory.Delete(album.AlbumPath);

			UpdateAlbumsCount();
			Logger.LogTrace($"({SectionName}) Удален альбом: {album.AlbumName}");
		}

		public void DeleteAlbums(Album[] albums)
		{
			Logger.LogTrace($"({SectionName}) Удаление альбомов...");
			int albumsCount = albums.Length;

			foreach (var album in albums)
				DeleteAlbum(album);
			UpdateAlbumsCount();
			Logger.LogTrace($"({SectionName}) Удалено {albumsCount} альбомов");
		}

		private void UpdateAlbumsCount()
		{
			AlbumsCount = AlbumsList.Count;
		}

		private void SaveAlbums()
		{
			AlbumData[] albumsData = new AlbumData[AlbumsCount];
			AlbumData albumData;
			for (int i = 0; i< albumsData.Length; i++)
			{
				albumData = AlbumsList[i].GetAlbumData();
				if (Directory.Exists(albumData.AlbumPath))
					albumsData[i] = albumData;
			}
			BootLoader.LastSessionData.Albums = albumsData;
		}
	}
}
