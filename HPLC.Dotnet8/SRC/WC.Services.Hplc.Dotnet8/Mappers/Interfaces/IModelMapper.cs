﻿// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2021. All rights reserved.
// 
//   WC.Services.Hplc
//   IModelMapper.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace WC.Services.Hplc.Dotnet8.Mappers.Interfaces
{
    using System.Collections.Generic;
    using Hyland.Unity.WorkView;
    using WC.Services.Hplc.Dotnet8.Models;

    public interface IModelMapper<TMappedModel1, TMappedModel2>
    {
        TMappedModel2 GetMappedModel(TMappedModel1 original);

        TMappedModel1 GetMappedModel(TMappedModel2 original);

       // TMappedModel1 GetMappedModel(object original);
        WorkViewObject GetMappedModel(object original);
    }
}