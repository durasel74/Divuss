using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Divuss.ViewModel
{
	internal abstract class Section
	{
		public abstract string SectionName { get; }

		public event PropertyChangedEventHandler PropertyChanged;
		public void OnPropertyChanged([CallerMemberName] string prop = "")
		{
			if (PropertyChanged != null)
				PropertyChanged(this, new PropertyChangedEventArgs(prop));
		}
	}
}
