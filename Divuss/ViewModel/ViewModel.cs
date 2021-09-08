using System;
using Divuss.Model;
using Divuss.Service;

using System.Windows;

namespace Divuss.ViewModel
{
	internal enum CommandMode
	{
		None,
		Single,
		Two,
		Multiplicity
	}

	internal enum PictureBufferMode
	{
		Photos,
		Albums
	}

	internal class ViewModel : NotifyPropertyChanged
	{
		private Section currentSection;
		private PictureBufferMode pictureBufferMode;
		private CommandMode commandMode;
		private bool selectionMode;

		public ViewModel()
		{
			DataContext = this;
			CommandModel = new CommandModel(this);
			PictureView = PictureView.GetInstance();
			PhotosTab = Photos.GetInstance();
			AlbumsTab = Albums.GetInstance();
			SliderTab = Slider.GetInstance();
			CurrentSection = PhotosTab;
		}

		public delegate void SelectionClear();
		public static event SelectionClear SelectionClearEventHandler;

		public static ViewModel DataContext { get; private set; }
		public CommandModel CommandModel { get; }
		public PictureView PictureView { get; }
		public Section PhotosTab { get; }
		public Section AlbumsTab { get; }
		public Section SliderTab { get; }

		public Section CurrentSection
		{
			get { return currentSection; }
			set
			{
				currentSection = value;
				OnPropertyChanged("CurrentSection");
				SelectionMode = false;
				ClearAllSelection();
				UpdateCurrentSectionProperty();
				Logger.LogTrace($"Выбран раздел: {currentSection.SectionName}");
			}
		}

		public PictureBufferMode PictureBufferMode
		{
			get { return pictureBufferMode; }
			private set
			{
				pictureBufferMode = value;
				OnPropertyChanged("PictureBufferMode");
			}
		}

		public CommandMode CommandMode
		{
			get { return commandMode; }
			set
			{
				var nextMode = value;
				if (nextMode != commandMode)
				{
					commandMode = nextMode;
					OnPropertyChanged("CommandMode");
				}
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
					selectionMode = nextMode;
					OnPropertyChanged("SelectionMode");
					if (selectionMode) 
						Logger.LogTrace("Режим выделения элементов включен");
					else 
						Logger.LogTrace("Режим выделения элементов выключен");
				}
			}
		}

		public static void ClearAllSelection()
		{
			SelectionClearEventHandler.Invoke();
		}

		private void UpdateCurrentSectionProperty()
		{
			if (CurrentSection is Photos)
				PictureBufferMode = PictureBufferMode.Photos;
			else if (CurrentSection is Albums)
				PictureBufferMode = PictureBufferMode.Albums;
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

		private SelectionCommand selectionCountCommand;
		public SelectionCommand SelectionCountCommand
		{
			get
			{
				return selectionCountCommand ??
					  (selectionCountCommand = new SelectionCommand(obj =>
					  {
						  int selectedItemsCount = (int)obj;
						  switch (selectedItemsCount)
						  {
							  case 0:
								  CommandMode = CommandMode.None;
								  break;
							  case 1:
								  CommandMode = CommandMode.Single;
								  break;
							  case 2:
								  CommandMode = CommandMode.Two;
								  break;
							  default:
								  CommandMode = CommandMode.Multiplicity;
								  break;
						  }
					  }));
			}
		}
	}
}
