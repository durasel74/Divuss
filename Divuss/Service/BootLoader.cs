using System;
using System.IO;
using System.Windows;

namespace Divuss.Service
{
	internal static class BootLoader
	{
		private const string APPLICATION_TITLE = "Divuss";
		private const string ALBUMS_FOLDER_NAME = "Albums";
		private const string CONFIG_FOLDER_NAME = "Config";

		private static string appDataPath;
		private static string applicationDataPath;
		private static string albumsFolderPath;
		private static string configFolderPath;

		static BootLoader()
		{
			appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\";
			applicationDataPath = appDataPath + APPLICATION_TITLE;
			albumsFolderPath = applicationDataPath + "\\" + ALBUMS_FOLDER_NAME;
			configFolderPath = applicationDataPath + "\\" + CONFIG_FOLDER_NAME;
		}
		
		public static void DataCheck()
		{
			CreateAppFolder();
			Logger.CompleteLaunch();
		}

		public static void ApplicationCrash(Exception exception)
		{
			if (exception == null) Application.Current.Shutdown();

			Logger.LogError(exception.Message);
			Logger.ForcedClose();
			MessageBox.Show("Error: " + exception.Message, "Error", 
				MessageBoxButton.OK, MessageBoxImage.Error);
			Application.Current.Shutdown();
		}

		private static void CreateAppFolder()
		{
			if (Directory.Exists(applicationDataPath) == false)
			{
				Directory.CreateDirectory(applicationDataPath);
				AppFolderCheck();
				Logger.LogTrace("Директория данных приложения создана, " +
					"так как не была найдена");
			}
			CreateAlbumsFolder();
			CreateConfigFolder();
		}

		private static void CreateAlbumsFolder()
		{
			if (Directory.Exists(albumsFolderPath) == false)
			{
				Directory.CreateDirectory(albumsFolderPath);
				Logger.LogTrace($"Создана директория: {ALBUMS_FOLDER_NAME}");
			}
		}

		private static void CreateConfigFolder()
		{
			if (Directory.Exists(configFolderPath) == false)
			{
				Directory.CreateDirectory(configFolderPath);
				Logger.LogTrace($"Создана директория: {CONFIG_FOLDER_NAME}");
			}
		}

		private static void AppFolderCheck()
		{
			if (Directory.Exists(applicationDataPath) == false)
				throw new Exception("Ok");
		}
	}
}
