// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2021. All rights reserved.
// 
//   WC.Services.Hplc
//   Adapter.Requests.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace WC.Services.Hplc.Adapters.v2.WorkView
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using Hyland.Unity.WorkView;
	using WC.Services.Hplc.Models.v2;
	using Attribute = WC.Services.Hplc.Models.v2.Attribute;
	using Object = Hyland.Unity.WorkView.Object;

	public partial class Adapter
	{
		/// <summary>
		///    Creates a Request in the WorkView HPLC Management Application.
		/// </summary>
		/// <remarks>
		///    Method [post]s data to the WorkView application and returns the object id of the created
		///    object
		/// </remarks>
		/// <param name="request">The request.</param>
		/// <returns>
		///    The object Id of the created Request in WorkView
		/// </returns>
		public long CreateRequest(WorkViewObject request)
		{
			Class requestClass = _workViewApplication.Classes.Find(Constants.Request.ClassName);

			long requestId = SaveClassData(
				requestClass,
				request);

			return requestId;
		}

		private long SaveClassData(
			Class wvClass,
			WorkViewObject wvo)
		{
			Object wvObject = wvClass.CreateObject(false);
			AttributeValueModifier attModifier = wvObject.CreateAttributeValueModifier();
			wvo.Attributes.ForEach(
				a => PopulateAttributeModifier(
					wvClass.Attributes.Find(a.Name),
					a,
					attModifier));

			attModifier.ApplyChanges();
			wvObject.Activate();
			return wvObject.ID;
		}

		private void PopulateAttributeModifier(
			Hyland.Unity.WorkView.Attribute attribute,
			Attribute value,
			AttributeValueModifier modifer)
		{
			if (!value.HasValue)
			{
				return;
			}

			IEnumerable<string> dataSet = GetDataSet(
				attribute.Class.ID,
				attribute.ID);

			if (dataSet.SafeAny())
			{
				if (value.HasBooleanValue)
				{
					if (value.GetBooleanValue)
					{
						if (dataSet.Any(
							v =>
								v.ToLower().Equals("y")
								|| v.ToLower().Equals("yes")))
						{
							modifer.SetAttributeValue(
							attribute.Name,
							dataSet.First(v =>
								v.ToLower().Equals("y")
								|| v.ToLower().Equals("yes")));
							return;
						}
					}

					if (dataSet.Any(
								v =>
									v.ToLower().Equals("n")
									|| v.ToLower().Equals("no")))
					{
						modifer.SetAttributeValue(
						attribute.Name,
						dataSet.First(v =>
							v.ToLower().Equals("n")
							|| v.ToLower().Equals("no")));
						return;
					}
				}

			}

			switch (attribute.AttributeType)
			{
				case AttributeType.Alphanumeric:
					modifer.SetAttributeValue(
						attribute,
						value.GetStringValue);
					break;

				case AttributeType.Boolean:
					modifer.SetAttributeValue(
						attribute,
						value.GetBooleanValue);
					break;

				case AttributeType.Currency:
					modifer.SetAttributeValue(
						attribute,
						value.GetDecimalValue);
					break;

				case	AttributeType.Date:
					modifer.SetAttributeValue(
						attribute,
						value.GetDateValue);
					break;

				case AttributeType.DateTime:
					modifer.SetAttributeValue(
						attribute,
						value.GetDateTimeValue);
					break;

				case AttributeType.Decimal:
					modifer.SetAttributeValue(
						attribute,
						value.GetDecimalValue);
					break;

				case AttributeType.Document:
					modifer.SetAttributeValue(
						attribute,
						value.GetLongValue);
					break;

				case AttributeType.EncryptedAlphanumeric:
					modifer.SetAttributeValue(
						attribute,
						value.GetStringValue);
					break;

				case AttributeType.Float:
					modifer.SetAttributeValue(
						attribute,
						value.GetDoubleValue);
					break;

				case AttributeType.FormattedText:
					modifer.SetAttributeValue(
						attribute,
						value.GetStringValue);
					break;

				case AttributeType.Integer:
					modifer.SetAttributeValue(
						attribute,
						value.GetLongValue);
					break;
				
				case AttributeType.Text:
					modifer.SetAttributeValue(
						attribute,
						value.GetStringValue);
					break;

				case AttributeType.Relation:
					modifer.SetAttributeValue(
						attribute,
						value.GetLongValue);
					break;

				default:
					throw new ArgumentOutOfRangeException();
			}

		}

		/// <summary>
		///    Gets the object.
		/// </summary>
		/// <param name="objectId">The object identifier.</param>
		/// <returns></returns>
		public WorkViewObject GetObject(long objectId)
		{
			Class requestClass = _workViewApplication.Classes.Find(Constants.Request.ClassName);

			Object request = requestClass.GetObjectByID(objectId);

			return _requestMapper.GetMappedModel(request);
		}

		/// <summary>
		///    Searches the specified class name.
		/// </summary>
		/// <param name="className">The class name.</param>
		/// <param name="filterName">The filter name.</param>
		/// <param name="attributes"></param>
		/// <param name="filters">The filters.</param>
		/// <returns></returns>
		public IEnumerable<WorkViewObject> HieSearch(
			string className,
			string filterName,
			string[] attributes,
			IDictionary<string, string> filters)
		{
			Class requestClass  = _workViewApplication.Classes.Find(Constants.Request.ClassName);

			DynamicFilterQuery requestQuery = requestClass.CreateDynamicFilterQuery();
			foreach (KeyValuePair<string, string> filter in filters)
			{
				requestQuery.AddConstraint(
					filter.Key,
					Operator.Equal,
					filter.Value);
			}
			
			FilterQueryResultItemList searchResult = requestQuery.Execute(10000);

			return searchResult.Select(
				o => _hieRequestMapper
					.GetMappedModel(requestClass.GetObjectByID(o.ObjectID)));
		}

		/// <summary>
		///    Searches the specified class name.
		/// </summary>
		/// <param name="className">The class name.</param>
		/// <param name="filterName">The filter name.</param>
		/// <param name="attributes"></param>
		/// <param name="filters">The filters.</param>
		/// <returns></returns>
		public IEnumerable<WorkViewObject> Search(
			string className,
			string filterName,
			string[] attributes,
			IDictionary<string, string> filters)
		{
			Class requestClass = _workViewApplication.Classes.Find(Constants.Request.ClassName);

			DynamicFilterQuery requestQuery = requestClass.CreateDynamicFilterQuery();
			foreach (KeyValuePair<string, string> filter in filters)
			{
				requestQuery.AddConstraint(
					filter.Key,
					Operator.Equal,
					filter.Value);
			}

			FilterQueryResultItemList searchResult = requestQuery.Execute(10000);

			return searchResult.Select(
				o => _requestMapper
					.GetMappedModel(requestClass.GetObjectByID(o.ObjectID)));
		}

		/// <summary>
		///    Updates a Request in the WorkView HPLC Management Application.
		/// </summary>
		/// <remarks>
		///    Method [put]s data to the WorkView application and returns the object id of the created
		///    object
		/// </remarks>
		/// <param name="request">The request.</param>
		/// <returns>
		///    The object Id of the updated Request in WorkView
		/// </returns>
		public long UpdateRequest(HplcServiceRequest request)
			=> 0;
	}
}