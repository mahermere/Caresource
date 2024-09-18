// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2023. All rights reserved.
// 
//   WC.Services.HplcManagement
//   WorkViewDataClassRequest.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace WC.Services.HplcManagement.Dotnet8.Mappers
{
    using System.Collections.Generic;
    using System.Linq;
    using Hyland.Unity.WorkView;
    using WC.Services.HplcManagement.Dotnet8.Mappers.Interfaces;
    using WC.Services.HplcManagement.Dotnet8.Models;

    public class WorkViewProductToData : BaseDataObjectModelMapper
    {
        public override Data GetMappedModel(
            Object original,
            bool includeChildren = true)
        {
            MainObject = original;
            Data wvo = base.GetMappedModel(GetRelatedObject(Constants.Products.Product));

            PopulateRelated(wvo);
            return wvo;

        }

        public override void PopulateRelated(Data data)
        {
            WorkViewStateToData stateMapper = new WorkViewStateToData();
            data.Related =
                new[] { stateMapper.GetMappedModel(GetRelatedObject(Constants.Products.RelationshipToState)) };

        }
    }
}
