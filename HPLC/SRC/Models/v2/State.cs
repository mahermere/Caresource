// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2021. All rights reserved.
// 
//   WC.Services.Hplc
//   State.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace WC.Services.Hplc.Models.v2
{
	using Newtonsoft.Json;

	public class State: BaseWorkViewEntity
	{
		public State()
		{
			ClassName = Constants.State.ClassName;
		}

		[WorkViewName(Constants.State.Abbreviation)]
		[JsonProperty("State")]
		public string Abbreviation { get; set; }

		[WorkViewName(Constants.State.Name)]
		public new string Name { get; set; }
	}
}