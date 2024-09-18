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

    public class WorkViewProviderToData : BaseDataObjectModelMapper
    {
        public override void PopulateRelated(Data data)
        {
            DependentObjectList dependentObjects = GetDependentObjects();
            WorkViewLocationToData locationMapper = new WorkViewLocationToData();

            if (dependentObjects.SafeAny())
            {
                List<Data> listDependentObjects = new List<Data>();

                listDependentObjects.AddRange(
                    from o in dependentObjects
                    where o.Class.Name.Equals(Constants.Location.ClassName)
                    select locationMapper.GetMappedModel(o.Class.GetObjectByID(o.ID)));

                data.Related = listDependentObjects;
            }
        }
    }
}