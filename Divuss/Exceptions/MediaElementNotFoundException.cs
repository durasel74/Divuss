using System;

namespace Divuss
{
	public class MediaElementNotFoundException : ApplicationException
	{
		public MediaElementNotFoundException(string path) 
			: base($"Could not find item in path: {path}.")
		{ }
	}
}
