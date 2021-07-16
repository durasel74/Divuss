using System;
using System.Collections.ObjectModel;
using Divuss.Model;

namespace Divuss.ViewModel
{
	public class Albums : Section
	{

		#region Singleton конструктор
		private static Albums instance;
		private Albums()
		{
			SectionName = "Albums";
			AlbumsList = new ObservableCollection<Album>();
		}
		public static Albums GetInstance()
		{
			if (instance == null)
				instance = new Albums();
			return instance;
		} 
		#endregion

		public override string SectionName { get; }
		public ObservableCollection<Album> AlbumsList { get; }


	}
}
