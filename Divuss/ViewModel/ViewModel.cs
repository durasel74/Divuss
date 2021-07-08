using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Divuss.ViewModel
{
	internal class ViewModel : INotifyPropertyChanged
	{
        public Photos photos { get; set; } = new Photos();

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
