using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Divuss.Service;

namespace Divuss
{
	public partial class App : Application
	{
		private void Application_DispatcherUnhandledException(object sender, 
			System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
		{
			BootLoader.ApplicationCrash(e.Exception);
			e.Handled = true;
		}
	}
}
