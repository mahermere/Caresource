// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2019. All rights reserved.
// 
//   CareSource.WC.Entities.WC.Core
//   ErrorCode.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace CareSource.WC.Entities.Exceptions
{
	public enum ErrorCode
	{
		Success = 0,
		InvalidKeyword = 1,
		InvalidDocumentType = 2,
		NoKeywords = 3,
		NoDocuments = 4,
		DocTypeKeywordConflict = 5,
		InvalidWorkViewApplicationName = 101,
		InvalidWorkViewClassName = 102,
		InvalidWorkViewAttributeName = 103,
		InvalidWorkViewFilterName = 104,
		InvalidRequest = 105,
		UnknownError = 999
	}
}