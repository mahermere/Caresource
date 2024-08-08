// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2022. All rights reserved.
// 
//   WC.Services.Document
//   InvalidKeywordException.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace CareSource.WC.Services.Document.Models.v6
{
	using System;

	public class InvalidKeywordException : Exception
	{
		public InvalidKeywordException(string keywordName)
		: base(keywordName){ }
	}
}