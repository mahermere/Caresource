// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2019. All rights reserved.
// 
//   WC.Services.OnBase
//   BaseModel.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace WC.Services.ImportProcessor.Dotnet8.Models.v1.OnBase
{
	public abstract class BaseModel
	{
		public long Id { get; set; }

		public string Name { get; set; }
	}
}