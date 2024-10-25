// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2022. All rights reserved.
// 
//   WC.Services.Document
//   InvalidKeywordException.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace WC.Services.Document.Dotnet8.Models.v6
{
	using System;

	public class InvalidKeywordException : Exception
	{
		public InvalidKeywordException(string keywordName)
		: base(keywordName){ }
	}
}