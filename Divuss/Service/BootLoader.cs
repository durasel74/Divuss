using System;
using System.IO;
using System.Text.Json;
using Divuss.Exceptions;

using System.Windows;

namespace Divuss.Service
{
	internal static class BootLoader
	{
		private const string APPLICATION_TITLE = "Divuss";
		private const string ALBUMS_FOLDER_NAME = "Albums";
		private const string CONFIG_FOLDER_NAME = "Config";
		private const string LAST_SESSION_FILE_NAME = "LastSession.json";

		private static string appDataPath;
		private static string applicationDataPath;
		private static string albumsFolderPath;
		private static string configFolderPath;
		private static string lastSessionFilePath;

		static BootLoader()
		{
			appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\";
			applicationDataPath = appDataPath + APPLICATION_TITLE;
			albumsFolderPath = applicationDataPath + "\\" + ALBUMS_FOLDER_NAME;
			configFolderPath = applicationDataPath + "\\" + CONFIG_FOLDER_NAME;
			lastSessionFilePath = configFolderPath + "\\" + LAST_SESSION_FILE_NAME;
		}

		public delegate void SaveHandler();
		public static event SaveHandler SaveDataEventHandler;
		public static event SaveHandler SaveAlbumsEventHandler;

		public static LastSessionData LastSessionData { get; set; }

		public static void Startup()
		{
			CheckData();
			LoadData();
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

		public static void CheckData()
		{
			CheckAppFolder();
			CheckAlbumsFolder();
			CheckConfigFolder();
			CheckLastSessionFile();
		}

		public static void SaveData()
		{
			CheckData();
			SaveDataEventHandler.Invoke();
			LastSessionData.Serialize(lastSessionFilePath);
		}

		public static void SaveAlbums()
		{
			CheckData();
			//CheckAlbums();
			SaveAlbumsEventHandler.Invoke();
			//LastSessionData.Serialize(lastSessionFilePath);
		}

		public static string GetAlbumsFolderPath()
		{
			return albumsFolderPath;
		}

		private static void LoadData()
		{
			LastSessionData = LastSessionData.Deserialize(lastSessionFilePath);
		}

		private static void AppFolderCheck()
		{
			if (Directory.Exists(applicationDataPath) == false)
				throw new ApplicationFolderCreateException();
		}

		private static void CheckAppFolder()
		{
			if (Directory.Exists(applicationDataPath) == false)
			{
				Directory.CreateDirectory(applicationDataPath);
				AppFolderCheck();
				Logger.LogTrace("Директория данных приложения создана, " +
					"так как не была найдена");
			}
		}

		private static void CheckAlbumsFolder()
		{
			if (Directory.Exists(albumsFolderPath) == false)
			{
				Directory.CreateDirectory(albumsFolderPath);
				Logger.LogTrace($"Создана директория: {ALBUMS_FOLDER_NAME}");
			}
		}

		private static void CheckConfigFolder()
		{
			if (Directory.Exists(configFolderPath) == false)
			{
				Directory.CreateDirectory(configFolderPath);
				Logger.LogTrace($"Создана директория: {CONFIG_FOLDER_NAME}");
			}
		}

		private static void CheckLastSessionFile()
		{
			if (File.Exists(lastSessionFilePath) == false)
			{
				LastSessionData = new LastSessionData();
				var fs = File.Create(lastSessionFilePath);
				var options = new JsonSerializerOptions() { WriteIndented = true };
				JsonSerializer.SerializeAsync<LastSessionData>(fs, LastSessionData, options);
				fs.Close();
				Logger.LogTrace($"Создан файл: {LAST_SESSION_FILE_NAME}");
			}
		}

		private static void CheckAlbums()
		{
			
		}
	}
}
