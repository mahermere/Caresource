// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2019. All rights reserved.
// 
//   Entities
//   NoDocumentsException.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace CareSource.WC.Entities.Exceptions
{
	public class NoDocumentsException : OnBaseExceptionBase
	{
		/// <summary>
		///    Initializes a new instance of the <see cref="NoDocumentsException" /> class.
		/// </summary>
		public NoDocumentsException(string id)
			: base(
				ErrorCode.NoDocuments,
				string.IsNullOrWhiteSpace(id)
					? "No documents found."
					: $"No documents found for Id: {id}.")
		{ }
	}
}