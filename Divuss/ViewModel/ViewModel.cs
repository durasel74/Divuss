using System;
using System.Collections.ObjectModel;
using Microsoft.Win32;
using Divuss.Model;
using Divuss.Service;

namespace Divuss.ViewModel
{
	internal class ViewModel : NotifyPropertyChanged
	{
		private Section currentSection;
		private bool selectionMode;

		public ViewModel()
		{
			SelectionMode = false;
			Sections = new ObservableCollection<Section>();
			Sections.Add(Photos.GetInstance());
			Sections.Add(Albums.GetInstance());
			PictureView = PictureView.GetInstance();
			CurrentSection = Sections[0];
		}

		public ObservableCollection<Section> Sections { get; }
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

		private void PhotosPictureOpen(string path)
		{
			var photos = (Photos)CurrentSection;
			photos.AddPictureToLast(path);
			PictureView.OpenPicture(photos.LastPicture);
			photos.UpdatePictureInLast(photos.LastPicture);
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
