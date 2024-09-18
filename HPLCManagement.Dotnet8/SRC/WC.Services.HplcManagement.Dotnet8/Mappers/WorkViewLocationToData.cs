// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2023. All rights reserved.
// 
//   WC.Services.HplcManagement
//   WorkViewLocationToData.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace WC.Services.HplcManagement.Dotnet8.Mappers
{
    using System.Collections.Generic;
    using System.Linq;
    using Hyland.Unity.WorkView;
    using WC.Services.HplcManagement.Dotnet8.Mappers.Interfaces;
    using WC.Services.HplcManagement.Dotnet8.Models;

    public class WorkViewLocationToData : BaseDataObjectModelMapper
    {
        public override void PopulateRelated(Data data)
        {
            DependentObjectList dependentObjects = GetDependentObjects();

            WorkViewPhoneToData phoneMapper = new WorkViewPhoneToData();

            if (dependentObjects.SafeAny())
            {
                List<Data> listDependentObjects = new List<Data>();

                listDependentObjects.AddRange(
                    from o in dependentObjects
                    where o.Class.Name.Equals(Constants.Phone.ClassName)
                    select phoneMapper.GetMappedModel(o.Class.GetObjectByID(o.ID)));

                data.Related = listDependentObjects;
            }
        }
    }
}