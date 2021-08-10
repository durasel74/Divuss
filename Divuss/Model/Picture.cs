using System;
using System.IO;
using System.Drawing;
using Divuss.Exceptions;
using Divuss.Service;

namespace Divuss.Model
{
	/// <summary>
	/// Класс картинки, предоставляет путь до изображения. Содержит 
	/// проверку существования и информацию о файле.
	/// </summary>
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

		/// <summary>
		/// Путь к файлу картинки.
		/// </summary>
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

		/// <summary>
		/// Выдает информацию о файле картинки и ее разрешение.
		/// </summary>
		/// <returns>Строка с информацией.</returns>
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

			return output;
		}

		public static bool PathExists(string path) => File.Exists(path);
		public bool PathExists() => File.Exists(imagePath);

		internal PictureData GetPictureData()
		{
			return new PictureData(ImagePath);
		}

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
