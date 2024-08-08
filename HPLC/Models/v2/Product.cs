// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2021. All rights reserved.
// 
//   WC.Services.Hplc
//   Product.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace WC.Services.Hplc.Models.v2
{
	using Newtonsoft.Json;

	/// <summary>
	/// Data and functions describing a WC.Services.Hplc.Models.v2.Product object.
	/// </summary>
	public class Product: BaseWorkViewEntity
	{
		public Product()
		{
			ClassName = Constants.Products.ClassName;
		}
		/// <summary>
		/// Gets or sets the Product Name
		/// </summary>
		[WorkViewName(Constants.Products.Name)]
		public new string Name { get; set; }

		/// <summary>
		/// Gets or sets the Product Code
		/// </summary>
		[JsonProperty("ProductCode")]
		[WorkViewName(Constants.Products.Code)]
		public string Code { get; set; }

		/// <summary>
		/// Gets or sets the Product Product
		/// </summary>
		[JsonProperty("Product")]
		[WorkViewName(Constants.Products.Product)]
		public string RootProduct { get; set; }

		/// <summary>
		/// Gets or sets the Product State
		/// </summary>
		//[WorkViewName(Constants.Products.RelationshipToState)]
		public State State { get; set; }

	}
}