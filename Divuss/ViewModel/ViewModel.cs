using System;
using Divuss.Model;
using Divuss.Service;

using System.Windows;

namespace Divuss.ViewModel
{
	internal class ViewModel : NotifyPropertyChanged
	{
		private Section currentSection;
		private bool selectionMode;

		public ViewModel()
		{
			SelectionMode = false;
			CommandModel = new CommandModel(this);
			PictureView = PictureView.GetInstance();
			PhotosTab = Photos.GetInstance();
			AlbumsTab = Albums.GetInstance();
			CurrentSection = PhotosTab;
		}

		public CommandModel CommandModel { get; }
		public PictureView PictureView { get; }
		public Section PhotosTab { get; }
		public Section AlbumsTab { get; }

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
