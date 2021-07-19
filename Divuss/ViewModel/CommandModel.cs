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
					  if (CurrentSection is Photos)
						  PhotosPictureSwitch(obj);
					  else if (CurrentSection is Albums)
						  AlbumsPictureSwitch(obj);
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

		private void PhotosPictureSwitch(object obj)
		{
			var picture = obj as Picture;
			if (picture != null)
			{
				PictureView.OpenPicture(picture);
				Photos.UpdatePictureInLast(picture);
			}
			else PictureView.ClosePicture();
		}

		private void AlbumsPictureSwitch(object obj)
		{
			var picture = obj as Picture;
			if (picture != null)
			{
				PictureView.OpenPicture(picture);
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
			var selectedList = (ObservableCollection<object>)obj;
			Picture[] selectedPictures = new Picture[selectedList.Count];

			if (selectedList.Count == 0) return;
			for (int i = 0; i < selectedPictures.Length; i++)
				selectedPictures[i] = (Picture)selectedList[i];

			Photos.RemovePicturesFromLast(selectedPictures);
		}

		private void AlbumsCreateAlbum()
		{
			var albums = (Albums)CurrentSection;
			albums.CreateAlbum();
		}

		private void AlbumsDeleteAlbums(object obj)
		{
			var albums = (Albums)CurrentSection;
			var selectedList = (ObservableCollection<object>)obj;
			Album[] selectedAlbums = new Album[selectedList.Count];

			if (selectedList.Count == 0) return;
			for (int i = 0; i < selectedAlbums.Length; i++)
				selectedAlbums[i] = (Album)selectedList[i];

			albums.DeleteAlbums(selectedAlbums);
		}
	}
}
