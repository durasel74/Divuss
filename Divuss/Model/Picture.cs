using System;
using System.IO;
using System.Drawing;
using Divuss.Service;

namespace Divuss.Model
{
	public class Picture
	{
		private string imagePath;
		private Size imageSize;

		public Picture(string imagePath)
		{
			ImagePath = imagePath;
			Image image = Image.FromFile(imagePath);
			imageSize = image.Size;
		}

		public string ImagePath
		{
			get
			{
				if (!PathExists())
					throw new MediaElementNotFoundException(imagePath);
				return imagePath;
			}
			private set
			{
				var newPath = value;
				if (!PathExists(value))
					throw new MediaElementNotFoundException(newPath);
				imagePath = newPath;
			}
		}

		public string GetPictureInfo()
		{
			FileInfo fileInfo = new FileInfo(imagePath);
			var length = ConvertBytesToSuitableString(fileInfo.Length);
			var creationTime = fileInfo.CreationTime;
			var lastAccessTime = fileInfo.LastAccessTime;
			string size = $"{imageSize.Width}x{imageSize.Height}";

			string output =
			$"Название файла: {ImagePath}\n" +
			$"Размер файла: {length}\n" +
			$"Дата создания: {creationTime}\n" +
			$"Дата открытия: {lastAccessTime}\n" +
			$"Размер изображения: {size}";

			Logger.LogTrace("Выведены сведения о картинке");
			return output;
		}

		public static bool PathExists(string path) => File.Exists(path);
		public bool PathExists() => File.Exists(imagePath);

		private string ConvertBytesToSuitableString(long bytes)
		{
			double preResult = bytes;
			long unitMultipler = 1024;
			string[] units = { "Б", "КБ", "МБ", "ГБ", "ТБ"};

			for (int i = 0; i < units.Length; i++)
			{
				if (preResult < unitMultipler) 
					return $"{Math.Round(preResult, 2)} {units[i]}";
				else 
					preResult /= unitMultipler;
			}
			return $"{preResult}";
		}
	}
}
