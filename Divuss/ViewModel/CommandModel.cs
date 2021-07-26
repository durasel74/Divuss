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

		public CommandModel(ViewModel viewModel)
		{
			this.viewModel = viewModel;
		}

		private Section CurrentSection => viewModel.CurrentSection;
		private PictureView PictureView => viewModel.PictureView;
		public Photos Photos => (Photos)viewModel.PhotosTab;
		private Albums Albums => (Albums)viewModel.AlbumsTab;

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
					  Logger.LogTrace("Нажата кнопка открытия изображения");
					  OpenFileDialog openFileDialog = new OpenFileDialog();
					  openFileDialog.Filter = "Image files (*.png;*.jpg)|*.png;*.jpg";

					  Logger.LogTrace("Открыто окно выбора файла...");
					  if (openFileDialog.ShowDialog() == true)
					  {
						  var imagePath = openFileDialog.FileName;
						  Logger.LogTrace($"Файл выбран: {imagePath}");
						  if (CurrentSection is Photos)
							  PhotosPictureOpen(imagePath);
						  else if (CurrentSection is Albums)
							  AlbumsPictureOpen(imagePath);
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
						  if (CurrentSection is Albums)
							  AlbumsDeleteAlbums(obj);
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
						  if (CurrentSection is Photos)
							  PhotosAddToAlbum(obj);
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
					  }, obj => { return obj is Album; }));
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
			if (album != null)
			{
				Albums.OpenAlbum(album);
			}
			else Albums.CloseAlbum();
		}

		private void PhotosPictureOpen(string path)
		{
			Photos.AddPictureToLast(path);
			PictureView.AddPicturesToBuffer(new ObservableCollection<Picture>() 
				{ Photos.LastPicture } );
			PictureView.OpenPicture(Photos.LastPicture);
			Photos.UpdatePictureInLast(Photos.LastPicture);
		}

		private void AlbumsPictureOpen(string path)
		{
			Albums.CurrentAlbum.AddPictureFromFile(path);
		}

		private void PhotosDeletePicture(Picture picture)
		{
			PictureView.PicturesBuffer.Remove(picture);
			Photos.RemovePictureFromLast(picture);
			PictureView.UpdateView();
		}

		private void PhotosDeletePictures(ObservableCollection<object> pictures)
		{
			Picture[] selectedPictures = ObservalbeObjectToPicturesArray(pictures);
			if (selectedPictures == null) return;
			Photos.RemovePicturesFromLast(selectedPictures);
		}

		private void AlbumsDeletePicture(Picture picture)
		{
			PictureView.PicturesBuffer.Remove(picture);
			Albums.CurrentAlbum.DeletePicture(picture);
			PictureView.UpdateView();
		}

		private void AlbumsDeletePictures(ObservableCollection<object> pictures)
		{
			Picture[] selectedPictures = ObservalbeObjectToPicturesArray(pictures);
			if (selectedPictures == null) return;
			Albums.CurrentAlbum.DeletePictures(selectedPictures);
		}

		private void AlbumsCreateAlbum()
		{
			Albums.CreateAlbum();
		}

		private void AlbumsDeleteAlbums(object obj)
		{
			var selectedList = (ObservableCollection<object>)obj;
			Album[] selectedAlbums = ObservableObjectToAlbumsArray(obj);
			if (selectedList == null) return;
			Albums.DeleteAlbums(selectedAlbums);
		}

		private void PhotosAddToAlbum(object obj)
		{
			var window = new View.AddToAlbumDialog(viewModel);
			Picture[] pictures;

			var picture = obj as Picture;
			var objectPictures = obj as ObservableCollection<object>;
			if (picture != null)
				pictures = new [] { picture };
			else
				pictures = ObservalbeObjectToPicturesArray(objectPictures);

			Albums.AddBuffer = pictures;
			window.ShowDialog();
		}

		private void PhotosConfirmAddToAlbum(object obj)
		{
			var album = obj as Album;
			if (album != null) album.AddPictures(Albums.AddBuffer);
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
