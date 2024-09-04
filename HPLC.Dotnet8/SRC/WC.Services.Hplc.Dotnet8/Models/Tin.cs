// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2021. All rights reserved.
// 
//   WC.Services.Hplc
//   Tin.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

using WC.Services.Hplc.Dotnet8.Models;

namespace WC.Services.Hplc.Dotnet8.Models
{
    public class Tin : BaseWorkViewEntity
    {
        public Tin()
        {
            ClassName = Constants.Tin.ClassName;
        }
        [WorkViewName(Constants.Tin.EntityTin)]
        public string EntityTin { get; set; }

    }
}