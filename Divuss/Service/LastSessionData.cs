using System;
using System.IO;
using System.Text.Json;
using System.Collections.ObjectModel;
using Divuss.Model;

namespace Divuss.Service
{
	internal class LastSessionData
	{
		public LastSessionData()
		{
			LastPictures = new ObservableCollection<Picture>();
			LastPictures = new ObservableCollection<Picture>()
			{
				new Picture(@"D:\закачки\картинки\Gradients\Gradient_Biruz.jpg"),
				new Picture(@"D:\закачки\картинки\Gradients\Gradient_Blue.jpg"),
				new Picture(@"D:\закачки\картинки\Gradients\Gradient_Lighting.jpg"),
				new Picture(@"D:\закачки\картинки\Gradients\Gradient_Violet.jpg")
			};
			Albums = new ObservableCollection<Album>();
		}

		public ObservableCollection<Picture> LastPictures { get; set; }
		public ObservableCollection<Album> Albums { get; set; }

		public async void Serialize(string filePath)
		{
			if (File.Exists(filePath) == false) return;

			using (FileStream fs = new FileStream(filePath, FileMode.Open))
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
