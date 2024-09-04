// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2021. All rights reserved.
// 
//   WC.Services.Hplc
//   State.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

using WC.Services.Hplc.Dotnet8.Models;

namespace WC.Services.Hplc.Dotnet8.Models
{
    using System.Text.Json.Serialization;

    public class State : BaseWorkViewEntity
    {
        public State()
        {
            ClassName = Constants.State.ClassName;
        }

        [WorkViewName(Constants.State.Abbreviation)]
        [JsonPropertyName("State")]
        public string Abbreviation { get; set; }

        [WorkViewName(Constants.State.Name)]
        public new string Name { get; set; }
    }
}