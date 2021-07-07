using System;
using System.Collections.Generic;

namespace Divuss.Model
{
	internal class User
	{
		private static User instance;

		private List<string> lastPictures = new List<string>();

		private User()
		{ }

		public static User GetInstance()
		{
			if (instance == null)
				instance = new User();
			return instance;
		}


		public void AddToLast(string[] picturePaths)
		{
			lastPictures.AddRange(picturePaths);
		}

		public void RemoveFromLast(string[] picturePaths)
		{
			foreach (string picturePath in picturePaths)
			{
				lastPictures.Remove(picturePath);
			}
		}
	}
}
