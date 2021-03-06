using System;
using System.Collections.ObjectModel;
using Microsoft.Win32;
using Divuss.Model;
using Divuss.Service;

using System.Windows;

namespace Divuss.ViewModel
{
	internal class CommandModel
	{
		private ViewModel viewModel;
		private string imageFilter = "" +
			"Image files (*.png;*.jpg;*.jpeg;*.tiff;*.tif;*.bmp;*.gif)" +
			"|*.png;*.jpg;*.jpeg;*.tiff;*.tif;*.bmp;*.gif";

		public CommandModel(ViewModel viewModel)
		{
			this.viewModel = viewModel;
		}

		private Section CurrentSection => viewModel.CurrentSection;
		private PictureView PictureView => viewModel.PictureView;
		public Photos Photos => (Photos)viewModel.PhotosTab;
		private Albums Albums => (Albums)viewModel.AlbumsTab;
		private Slider Slider => (Slider)viewModel.SliderTab;

		private PictureCommand pictureSwitchCommand;
		public PictureCommand PictureSwitchCommand
		{
			get
			{
				return pictureSwitchCommand ??
				  (pictureSwitchCommand = new PictureCommand(obj =>
				  {
					  var picture = obj as Picture;
					  var pictures = ObjectToObservablePicturesList(obj);

					  if (pictures == null && picture != null)
					  {
						  if (CurrentSection is Photos)
							  pictures = Photos.LastPictures;
						  else if (CurrentSection is Albums)
							  pictures = Albums.CurrentAlbum.Elements;
					  }
					  PictureSwitch(picture, pictures);
				  }));
			}
		}

		private PictureCommand albumSwitchCommand;
		public PictureCommand AlbumSwitchCommand
		{
			get
			{
				return albumSwitchCommand ??
				  (albumSwitchCommand = new PictureCommand(obj =>
				  {
					  if (CurrentSection is Albums)
						  AlbumsAlbumSwitch(obj);
				  }));
			}
		}

		private ButtonCommand pictureOpenCommand;
		public ButtonCommand PictureOpenCommand
		{
			get
			{
				return pictureOpenCommand ??
				  (pictureOpenCommand = new ButtonCommand(obj =>
				  {
					  Logger.LogTrace("Нажата кнопка открытия изображений");
					  OpenFileDialog openFileDialog = new OpenFileDialog();
					  openFileDialog.Multiselect = true;
					  openFileDialog.Filter = imageFilter;

					  Logger.LogTrace("Открыто окно выбора файлов...");
					  if (openFileDialog.ShowDialog() == true)
					  {
						  var imagesPaths = openFileDialog.FileNames;
						  Array.Reverse(imagesPaths);
						  Logger.LogTrace($"Выбрано файлов: {imagesPaths.Length}");
						  if (CurrentSection is Photos)
							  PhotosPictureOpen(imagesPaths);
						  else if (CurrentSection is Albums)
							  AlbumsPictureOpen(imagesPaths);
					  }
					  else Logger.LogTrace("Файл не был выбран");
				  }));
			}
		}

		private ButtonCommand pictureDeleteCommand;
		public ButtonCommand PictureDeleteCommand
		{
			get
			{
				return pictureDeleteCommand ??
				  (pictureDeleteCommand = new ButtonCommand(obj =>
				  {
					  var picture = obj as Picture;
					  var pictures = obj as ObservableCollection<object>;

					  if (CurrentSection is Photos)
					  {
						  if (picture != null)
							  PhotosDeletePicture(picture);
						  else if (pictures != null)
							  PhotosDeletePictures(pictures);
					  }
					  else if (CurrentSection is Albums)
					  {
						  if (picture != null)
							  AlbumsDeletePicture(picture);
						  else if (pictures != null)
							  AlbumsDeletePictures(pictures);
					  }
				  }));
			}
		}

		private ButtonCommand albumCreateCommand;
		public ButtonCommand AlbumCreateCommand
		{
			get
			{
				return albumCreateCommand ??
					  (albumCreateCommand = new ButtonCommand(obj =>
					  {
						  if (CurrentSection is Albums)
							  AlbumsCreateAlbum();
					  }));
			}
		}

		private ButtonCommand albumDeleteCommand;
		public ButtonCommand AlbumDeleteCommand
		{
			get
			{
				return albumDeleteCommand ??
					  (albumDeleteCommand = new ButtonCommand(obj =>
					  {
						  var album = obj as Album;
						  var albums = obj as ObservableCollection<object>;

						  if (CurrentSection is Albums)
						  {
							  if (album != null)
								  AlbumsDeleteAlbum(album);
							  else if (albums != null)
								  AlbumsDeleteAlbums(albums);
						  }
					  }));
			}
		}

		private ButtonCommand pictureViewPreviousMoveCommand;
		public ButtonCommand PictureViewPreviousMoveCommand
		{
			get
			{
				return pictureViewPreviousMoveCommand ??
					  (pictureViewPreviousMoveCommand = new ButtonCommand(obj =>
					  { PictureView.MoveBuffer(BufferMove.Previous); },
					  (obj) =>
					  {
						  var buffer = PictureView.PicturesBuffer;
						  if (buffer.Count == 0)
							  return false;
						  else if (buffer.IndexOf(PictureView.CurrentPicture) == 0)
							  return false;
						  return true;
					  }));
			}
		}

		private ButtonCommand pictureViewNextMoveCommand;
		public ButtonCommand PictureViewNextMoveCommand
		{
			get
			{
				return pictureViewNextMoveCommand ??
					  (pictureViewNextMoveCommand = new ButtonCommand(obj =>
					  { PictureView.MoveBuffer(BufferMove.Next); },
					  (obj) =>
					  {
						  var buffer = PictureView.PicturesBuffer;
						  if (buffer.Count == 0)
							  return false;
						  else if (buffer.IndexOf(PictureView.CurrentPicture)
							== buffer.Count - 1)
							  return false;
						  return true;
					  }));
			}
		}

		private ButtonCommand pictureGetInfoCommand;
		public ButtonCommand PictureGetInfoCommand
		{
			get
			{
				return pictureGetInfoCommand ??
					  (pictureGetInfoCommand = new ButtonCommand(obj =>
					  {
						  MessageBox.Show(PictureView.CurrentPicture.GetPictureInfo());
						  Logger.LogTrace("Выведены сведения о картинке");
					  }));
			}
		}

		private ButtonCommand albumGetInfoCommand;
		public ButtonCommand AlbumGetInfoCommand
		{
			get
			{
				return albumGetInfoCommand ??
						  (albumGetInfoCommand = new ButtonCommand(obj =>
						  {
							  var album = obj as Album;
							  if (album == null) return;
							  MessageBox.Show(album.GetAlbumInfo());
						  }));
			}
		}

		private ButtonCommand pictureAddToAlbumCommand;
		public ButtonCommand PictureAddToAlbumCommand
		{
			get
			{
				return pictureAddToAlbumCommand ??
					  (pictureAddToAlbumCommand = new ButtonCommand(obj =>
					  {
						  PicturesAddToAlbum(obj);
					  }));
			}
		}

		private ButtonCommand confirmAddToAlbumCommand;
		public ButtonCommand ConfirmAddToAlbumCommand
		{
			get
			{
				return confirmAddToAlbumCommand ??
					  (confirmAddToAlbumCommand = new ButtonCommand(obj =>
					  {
						  if (CurrentSection is Photos)
							  PhotosConfirmAddToAlbum(obj);
						  else if (CurrentSection is Albums)
							  AlbumsConfirmAddToAlbum(obj);
					  }, obj => { return obj is Album; }));
			}
		}

		private ButtonCommand albumRenameCommand;
		public ButtonCommand AlbumRenameCommand
		{
			get
			{
				return albumRenameCommand ??
					  (albumRenameCommand = new ButtonCommand(obj =>
					  {
						  AlbumRename(obj);
					  }));
			}
		}

		private ButtonCommand sliderAddPictureCommand;
		public ButtonCommand SliderAddPictureCommand
		{
			get
			{
				return sliderAddPictureCommand ??
					  (sliderAddPictureCommand = new ButtonCommand(obj =>
					  {
						  var pictures = obj as ObservableCollection<object>;

						  if (CurrentSection is Photos)
						  {
							  if (pictures != null && pictures.Count == 2)
								SliderAddPictures(pictures);
						  }
					  }));
			}
		}

		private ButtonCommand sliderRestartCommand;
		public ButtonCommand SliderRestartCommand
		{
			get
			{
				return sliderRestartCommand ??
					  (sliderRestartCommand = new ButtonCommand(obj =>
					  {
						  Slider.RestartView();
					  }));
			}
		}

		private ButtonCommand sliderSwapCommand;
		public ButtonCommand SliderSwapCommand
		{
			get
			{
				return sliderSwapCommand ??
					  (sliderSwapCommand = new ButtonCommand(obj =>
					  {
						  Slider.SwapPictures();
					  }));
			}
		}

		private void PictureSwitch(Picture picture, 
			ObservableCollection<Picture> pictures)
		{
			if (picture != null && pictures != null)
			{
				PictureView.AddPicturesToBuffer(pictures);
				PictureView.OpenPicture(picture);
			}
			else if (pictures != null)
			{
				PictureView.AddPicturesToBuffer(pictures);
				PictureView.OpenPicture(PictureView.PicturesBuffer[0]);
			}
			else PictureView.ClosePicture();
		}

		private void AlbumsAlbumSwitch(object obj)
		{
			var album = obj as Album;
			if (album != null) Albums.OpenAlbum(album);
			else Albums.CloseAlbum();
		}

		private void PhotosPictureOpen(string[] paths)
		{
			Photos.AddPicturesToLast(paths);
			var buffer = new ObservableCollection<Picture>();
			for (int i = 0; i < paths.Length; i++)
				buffer.Add(Photos.LastPictures[i]);
			PictureView.AddPicturesToBuffer(buffer);
			PictureView.OpenPicture(Photos.LastPicture);
			BootLoader.SaveData();
		}

		private void AlbumsPictureOpen(string[] paths)
		{
			Albums.CurrentAlbum.AddPicturesFromFile(paths);
			BootLoader.SaveAlbums();
		}

		private void PhotosDeletePicture(Picture picture)
		{
			Photos.RemovePictureFromLast(picture);
			PictureView.RemoveCurrentPictureFromBuffer();
			BootLoader.SaveData();
		}

		private void PhotosDeletePictures(ObservableCollection<object> pictures)
		{
			Picture[] selectedPictures = ObservalbeObjectToPicturesArray(pictures);
			if (selectedPictures == null) return;
			Photos.RemovePicturesFromLast(selectedPictures);
			BootLoader.SaveData();
		}

		private void AlbumsDeletePicture(Picture picture)
		{
			Albums.CurrentAlbum.DeletePicture(picture);
			PictureView.RemoveCurrentPictureFromBuffer();
			BootLoader.SaveAlbums();
		}

		private void AlbumsDeletePictures(ObservableCollection<object> pictures)
		{
			Picture[] selectedPictures = ObservalbeObjectToPicturesArray(pictures);
			if (selectedPictures == null) return;
			Albums.CurrentAlbum.DeletePictures(selectedPictures);
			BootLoader.SaveAlbums();
		}

		private void AlbumsCreateAlbum()
		{
			Albums.CreateAlbum();
			var newAlbum = Albums.AlbumsBuffer[0];
			BootLoader.SaveAlbums();
			AlbumRenameCommand.Execute(newAlbum);
		}

		private void AlbumsDeleteAlbum(Album album)
		{
			Albums.DeleteAlbum(album);
			BootLoader.SaveAlbums();
		}

		private void AlbumsDeleteAlbums(ObservableCollection<object> albums)
		{
			Album[] selectedAlbums = ObservableObjectToAlbumsArray(albums);
			if (selectedAlbums == null) return;
			Albums.DeleteAlbums(selectedAlbums);
			BootLoader.SaveAlbums();
		}

		private void PicturesAddToAlbum(object obj)
		{
			var window = new View.AddToAlbumDialog(viewModel);
			Picture[] pictures;

			var picture = obj as Picture;
			var objectPictures = obj as ObservableCollection<object>;
			if (picture != null)
				pictures = new[] { picture };
			else
				pictures = ObservalbeObjectToPicturesArray(objectPictures);

			Albums.AlbumsBuffer = CopyAlbumsList();
			if (CurrentSection is Albums)
				Albums.AlbumsBuffer.Remove(Albums.CurrentAlbum);
			Albums.AddBuffer = pictures;
			Albums.IsCopyMove = false;
			window.ShowDialog();
		}

		private void PhotosConfirmAddToAlbum(object obj)
		{
			var album = obj as Album;
			if (album != null) album.AddPictures(Albums.AddBuffer);
			BootLoader.SaveAlbums();
		}

		private void AlbumsConfirmAddToAlbum(object obj)
		{
			var album = obj as Album;
			if (album == null) return;

			if (Albums.IsCopyMove)
			{
				Albums.CurrentAlbum.CopyPicture(Albums.AddBuffer, album);
			}
			else
			{
				Albums.CurrentAlbum.MovePictures(Albums.AddBuffer, album);
				PictureView.RemoveCurrentPictureFromBuffer();
			}
			BootLoader.SaveAlbums();
		}

		private void AlbumRename(object obj)
		{
			var album = obj as Album;

			if (album != null)
			{
				album.IsRenaming = true;
				Albums.RenameBuffer = album.AlbumName;
				Albums.AlbumsBuffer = new ObservableCollection<Album>() { album };
			}
			else
			{
				album = Albums.AlbumsBuffer[0];
				album.IsRenaming = false;
				album.AlbumName = Albums.RenameBuffer;
				Albums.RenameBuffer = "";
				Albums.AlbumsBuffer = null;
			}
			BootLoader.SaveAlbums();
		}

		private void SliderAddPictures(ObservableCollection<object> pictures)
		{
			var picturesArray = ObservalbeObjectToPicturesArray(pictures);
			Slider.AddPicturesToSlider(picturesArray);
		}

		private ObservableCollection<Album> CopyAlbumsList()
		{
			ObservableCollection<Album> newList = new ObservableCollection<Album>();
			foreach (var album in Albums.AlbumsList) newList.Add(album);
			return newList;
		}

		private Picture[] ObservalbeObjectToPicturesArray(
			ObservableCollection<object> objectPictures)
		{
			if (objectPictures != null && objectPictures.Count != 0)
			{
				Picture[] pictures = new Picture[objectPictures.Count];
				for (int i = 0; i < objectPictures.Count; i++)
					pictures[i] = (Picture)objectPictures[i];
				return pictures;
			}
			return null;
		}

		private Album[] ObservableObjectToAlbumsArray(object obj)
		{
			var objectAlbums = obj as ObservableCollection<object>;
			if (objectAlbums != null && objectAlbums.Count != 0)
			{
				Album[] albums = new Album[objectAlbums.Count];
				for (int i = 0; i < objectAlbums.Count; i++)
					albums[i] = (Album)objectAlbums[i];
				return albums;
			}
			return null;
		}

		private ObservableCollection<Picture> ObjectToObservablePicturesList(object obj)
		{
			var objectPictures = obj as ObservableCollection<object>;
			if (objectPictures != null && objectPictures.Count != 0)
			{
				ObservableCollection<Picture> pictures = new ObservableCollection<Picture>();
				foreach (var picture in objectPictures)
					pictures.Add((Picture)picture);
				return pictures;
			}
			return null;
		}
	}
}
