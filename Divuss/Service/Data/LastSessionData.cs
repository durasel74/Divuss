using System;
using System.IO;
using System.Text.Json;

namespace Divuss.Service
{
	internal class LastSessionData
	{
		public LastSessionData()
		{
			LastPictures = new PictureData[0];
			//LastPictures = new PictureData[]
			//{
			//	new PictureData(@"D:\закачки\картинки\Gradients\Gradient_Biruz.jpg"),
			//	new PictureData(@"D:\закачки\картинки\Gradients\Gradient_Blue.jpg"),
			//	new PictureData(@"D:\закачки\картинки\Gradients\Gradient_Lighting.jpg"),
			//	new PictureData(@"D:\закачки\картинки\Gradients\Gradient_Violet.jpg"),
			//};

			//Albums = new ObservableCollection<Album>();
		}

		public PictureData[] LastPictures { get; set; }
		//public ObservableCollection<Album> Albums { get; set; }

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
