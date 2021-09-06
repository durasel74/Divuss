using System;
using System.IO;
using System.Text.Json;

namespace Divuss.Service
{
	[Serializable]
	internal class LastSessionData
	{
		public LastSessionData()
		{
			LastPictures = new PictureData[0];
			Albums = new AlbumData[0];
		}

		public PictureData[] LastPictures { get; set; }
		public AlbumData[] Albums { get; set; }

		public async void Serialize(string filePath)
		{
			if (File.Exists(filePath) == false) return;

			using (FileStream fs = new FileStream(filePath, FileMode.Create))
			{
				var options = new JsonSerializerOptions() { WriteIndented = true };
				await JsonSerializer.SerializeAsync<LastSessionData>(fs, this, options);
			}
		}

		public static LastSessionData Deserialize(string filePath)
		{
			if (File.Exists(filePath) == false) return null;

			LastSessionData data;
			using (FileStream fs = new FileStream(filePath, FileMode.Open))
			{
				var options = new JsonSerializerOptions() { WriteIndented = true };
				data = JsonSerializer.DeserializeAsync<LastSessionData>(fs, options).Result;
			}
			return data;
		}
	}
}
