// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2019. All rights reserved.
// 
//   CreateGrievanceAppeals
//   OBFile.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace CareSource.WC.Services.CreateGrievanceAppeals.Models.Data
{
	using System.Collections.Generic;

	public class OBFile
	{
		public string FileName { get; set; }

		public List<string> PageData { get; set; }
	}
}