using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Collections.ObjectModel;
using Microsoft.Win32;
using Divuss.Model;

namespace Divuss.ViewModel
{
	internal class ViewModel : INotifyPropertyChanged
	{
		private Section currentSection;

		public ViewModel()
        {
			Sections = new ObservableCollection<Section>();
			Sections.Add(Photos.GetInstance());
			Sections.Add(Albums.GetInstance());
			CurrentSection = Sections[0];
		}

		public ObservableCollection<Section> Sections { get; }

		public Section CurrentSection
		{
			get { return currentSection; }
			set
			{
				currentSection = value;
				OnPropertyChanged("CurrentSection");
			}
		}

		private ButtonCommand pictureSwitchCommand;
		public ButtonCommand PictureSwitchCommand
		{
			get
			{
				return pictureSwitchCommand ??
				  (pictureSwitchCommand = new ButtonCommand(obj =>
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
					  OpenFileDialog openFileDialog = new OpenFileDialog();
					  openFileDialog.Filter = "Image files (*.png;*.jpg)|*.png;*.jpg";
					  if (openFileDialog.ShowDialog() == true)
					  {
						  var imagePath = openFileDialog.FileName;
						  if (CurrentSection is Photos)
							  PhotosPictureOpen(imagePath);
					  }
				  }));
			}
		}

		private void PhotosPictureSwitch(object obj)
		{
			Photos photos = (Photos)CurrentSection;

			var picture = obj as Picture;
			if (picture != null)
				photos.OpenPicture(picture);
			else
				photos.ClosePicture();
		}

		private void PhotosPictureOpen(string path)
		{
			Photos photos = (Photos)CurrentSection;
			photos.AddPictureToLast(path);
			photos.OpenPicture(photos.LastPictures[0]);
		}

		public event PropertyChangedEventHandler PropertyChanged;
		public void OnPropertyChanged([CallerMemberName] string prop = "")
		{
			if (PropertyChanged != null)
				PropertyChanged(this, new PropertyChangedEventArgs(prop));
		}
	}
}
