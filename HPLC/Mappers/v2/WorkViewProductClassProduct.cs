// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2021. All rights reserved.
// 
//   WC.Services.Hplc
//   WorkViewProductClassProduct.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace WC.Services.Hplc.Mappers.v2
{
	using System.Collections.Generic;
	using System.Linq;
	using System.Runtime.Caching;
	using Hyland.Unity.WorkView;
	using WC.Services.Hplc.Models.v2;

	public class WorkViewProductClassProduct : BaseWorkViewObjectModelMapper<Product>
	{
		private readonly IModelMapper<WorkViewObject, State> _stateMapper;
		private readonly MemoryCache _memoryCache;

		public WorkViewProductClassProduct(
			IModelMapper<WorkViewObject, State> stateMapper,
			MemoryCache memoryCache)
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
			MainObject = original;

			WorkViewObject wvo = base.GetMappedModel(GetRelatedObject(Constants.Products.Product));

			Object state = GetRelatedObject(Constants.Products.RelationshipToState);

			if (state != null)
			{
				wvo.Related = new List<WorkViewObject> { _stateMapper.GetMappedModel(state) };
			}

			return wvo;
		}
	}
}