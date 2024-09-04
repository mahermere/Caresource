// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2021. All rights reserved.
// 
//   WC.Services.Hplc
//   WorkViewProductClassProduct.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace WC.Services.Hplc.Dotnet8.Mappers
{
    using System.Collections.Generic;
    using System.Linq;
    using Hyland.Unity.WorkView;
    using Microsoft.Extensions.Caching.Memory;
    using WC.Services.Hplc.Dotnet8.Mappers.Interfaces;
    using WC.Services.Hplc.Dotnet8.Models;
  

    public class WorkViewProductClassProduct : BaseWorkViewObjectModelMapper<Product>
    {
        private readonly IModelMapper<WorkViewObject, State> _stateMapper;
        private readonly IMemoryCache _memoryCache;

        public WorkViewProductClassProduct(
            IModelMapper<WorkViewObject, State> stateMapper,
            IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
            _stateMapper = stateMapper;
        }

        public override Product GetMappedModel(WorkViewObject original)
        {
            Product product = base.GetMappedModel(original);

            product.Code = GetStringValue(Constants.Products.Code);
            product.RootProduct = GetStringValue(Constants.Products.Product);
            product.Name = GetStringValue(Constants.Products.Name);

            WorkViewObject state = original.Related.FirstOrDefault(
                wvo => wvo.ClassName.Equals(Constants.State.ClassName));

            if (state != null)
            {
                product.State = _stateMapper.GetMappedModel(state);
            }

            return product;
        }

        public override WorkViewObject GetMappedModel(Product original)
        {
            WorkViewObject wvo = base.GetMappedModel(original);

            if (original.State != null)
            {
                wvo.Related = new List<WorkViewObject> { _stateMapper.GetMappedModel(original.State) };
            }

            return wvo;
        }

        public override WorkViewObject GetMappedModel(Object original)
        {
            MainObject = (Object)original;

            WorkViewObject wvo = base.GetMappedModel(GetRelatedObject(Constants.Products.Product));

            object state = GetRelatedObject(Constants.Products.RelationshipToState);

            if (state != null)
            {
                wvo.Related = new List<WorkViewObject> { _stateMapper.GetMappedModel(state) };
            }

            return wvo;
        }
    }
}