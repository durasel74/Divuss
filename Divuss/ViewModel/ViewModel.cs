using System;
using Microsoft.Win32;
using Divuss.Model;
using Divuss.Service;

using System.Windows;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Divuss.ViewModel
{
	internal class ViewModel : NotifyPropertyChanged
	{
		private Section currentSection;
		private bool selectionMode;

		public ViewModel()
		{
			SelectionMode = false;
			PhotosTab = Photos.GetInstance();
			AlbumsTab = Albums.GetInstance();
			PictureView = PictureView.GetInstance();
			CurrentSection = PhotosTab;
		}

		public Section PhotosTab { get; }
		public Section AlbumsTab { get; }
		public PictureView PictureView { get; }

		public Section CurrentSection
		{
			get { return currentSection; }
			set
			{
				currentSection = value;
				OnPropertyChanged("CurrentSection");
				SelectionMode = false;
				Logger.LogTrace($"Выбран раздел: {currentSection.SectionName}");
			}
		}

		public bool SelectionMode
		{
			get { return selectionMode; }
			set
			{
				var nextMode = value;
				if (nextMode != selectionMode)
				{
					selectionMode = value;
					OnPropertyChanged("SelectionMode");
					if (selectionMode) Logger.LogTrace("Режим выделения элементов включен");
					else Logger.LogTrace("Режим выделения элементов выключен");
				}
			}
		}

		private void PhotosPictureSwitch(object obj)
		{
			var photos = (Photos)CurrentSection;
			var picture = obj as Picture;
			if (picture != null)
			{
				PictureView.OpenPicture(picture);
				photos.UpdatePictureInLast(picture);
			}
			else PictureView.ClosePicture();
		}

		private void AlbumsPictureSwitch(object obj)
		{
			var albums = (Albums)CurrentSection;
			var picture = obj as Picture;
			if (picture != null)
			{
				PictureView.OpenPicture(picture);
			}
			else PictureView.ClosePicture();
		}

		private void AlbumsAlbumSwitch(object obj)
		{
			var albums = (Albums)CurrentSection;
			var album = obj as Album;
			if (album != null)
			{
				albums.OpenAlbum(album);
			}
			else albums.CloseAlbum();
		}

		private void PhotosPictureOpen(string path)
		{
			var photos = (Photos)CurrentSection;
			photos.AddPictureToLast(path);
			PictureView.OpenPicture(photos.LastPicture);
			photos.UpdatePictureInLast(photos.LastPicture);
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
					  }
					  else Logger.LogTrace("Файл не был выбран");
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

		private KeyCommand selectionModeCommand;
		public KeyCommand SelectionModeCommand
		{
			get
			{
				return selectionModeCommand ??
					  (selectionModeCommand = new KeyCommand(obj =>
					  {
						  if (CurrentSection is Photos)
							  SelectionMode = (bool)obj;
						  else if (CurrentSection is Albums)
							  SelectionMode = (bool)obj;
					  }));
			}
		}
	}
}
