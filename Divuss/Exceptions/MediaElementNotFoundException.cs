using System;

namespace Divuss.Exceptions
{
	public class MediaElementNotFoundException : ApplicationException
	{
		public MediaElementNotFoundException(string path) 
			: base($"Could not find item in path: {path}.")
		{ }
	}
}
