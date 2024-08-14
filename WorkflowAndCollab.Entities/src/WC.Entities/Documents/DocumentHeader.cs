// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2019. All rights reserved.
// 
//   CareSource.WC.Entities
//   DocumentHeader.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace CareSource.WC.Entities.Documents
{
	using System;
	using System.Collections.Generic;

	public class DocumentHeader
	{
		/// <summary>
		///    Gets or sets the display columns of the document header class.
		/// </summary>
		public IDictionary<string, object> DisplayColumns { get; set; } =
			new Dictionary<string, object>();

		/// <summary>
		///    Gets or sets the document date of the document header class.
		/// </summary>
		public DateTime DocumentDate { get; set; }

		/// <summary>
		///    Gets or sets the document identifier of the document header class.
		/// </summary>
		public long DocumentId { get; set; }

		/// <summary>
		///    Gets or sets the document name of the document header class.
		/// </summary>
		public string DocumentName { get; set; }

		/// <summary>
		///    Gets or sets the type of the document.
		/// </summary>
		public string DocumentType { get; set; }
	}
}