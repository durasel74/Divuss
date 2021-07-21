using System;
using System.Collections.ObjectModel;
using Microsoft.Win32;
using Divuss.Model;
using Divuss.Service;

namespace Divuss.ViewModel
{
	internal class CommandModel
	{
		private ViewModel viewModel;

		public CommandModel(ViewModel viewModel)
		{
			this.viewModel = viewModel;
		}

		private Section CurrentSection =>  viewModel.CurrentSection;
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
					  if (CurrentSection is Photos)
						  PhotosDeletePictures(obj);
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
						  else if (buffer.IndexOf(PictureView.CurrentPicture)
							== 0)
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

		private void PictureSwitch(Picture picture, ObservableCollection<Picture> pictures)
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
			PictureView.OpenPicture(Photos.LastPicture);
			Photos.UpdatePictureInLast(Photos.LastPicture);
		}

		private void AlbumsPictureOpen(string path)
		{
			Albums.CurrentAlbum.AddPictureFromFile(path);
		}

		private void PhotosDeletePictures(object obj)
		{
			Picture[] selectedPictures = ObservalbeObjectToPicturesArray(obj);
			if (selectedPictures == null) return;
			Photos.RemovePicturesFromLast(selectedPictures);
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






		private Picture[] ObservalbeObjectToPicturesArray(object obj)
		{
			var objectPictures = obj as ObservableCollection<object>;
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
