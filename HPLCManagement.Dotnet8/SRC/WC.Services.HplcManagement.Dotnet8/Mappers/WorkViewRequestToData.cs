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
    using WC.Services.HplcManagement.Dotnet8.Models;

    public class WorkViewRequestToData : BaseDataObjectModelMapper
    {
        public override void PopulateRelated(Data data)
        {
            if (!IncludeChildren)
            {
                return;
            }

            DependentObjectList dependentItems = GetDependentObjects();

            if (!dependentItems.SafeAny())
            {
                return;
            }

            List<Data> related = new List<Data>();
            if (dependentItems.SafeAny(r => r.Class.Name == Constants.Products.ClassName))
            {
                WorkViewProductToData productMapper = new WorkViewProductToData();
                related.AddRange(
                    from o in dependentItems
                    where o.Class.Name.Equals(Constants.Products.ClassName)
                    select productMapper.GetMappedModel(o.Class.GetObjectByID(o.ID)));
            }

            if (dependentItems.SafeAny(r => r.Class.Name == Constants.Provider.ClassName))
            {
                WorkViewProviderToData providerMapper = new WorkViewProviderToData();
                related.AddRange(
                    from o in dependentItems
                    where o.Class.Name.Equals(Constants.Provider.ClassName)
                    select providerMapper.GetMappedModel(o.Class.GetObjectByID(o.ID)));
            }

            if (dependentItems.SafeAny(r => r.Class.Name == Constants.Tin.ClassName))
            {
                WorkViewTinToData tinMapper = new WorkViewTinToData();
                related.AddRange(
                    from o in dependentItems
                    where o.Class.Name.Equals(Constants.Provider.ClassName)
                    select tinMapper.GetMappedModel(o.Class.GetObjectByID(o.ID)));
            }

            data.Related = related;
        }
    }
}