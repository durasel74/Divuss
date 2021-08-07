using System;

namespace Divuss.Exceptions
{
	public class ApplicationFolderCreateException : ApplicationException
	{
		public ApplicationFolderCreateException()
				: base("Could not find or create application data directory.")
		{ }
	}
}
