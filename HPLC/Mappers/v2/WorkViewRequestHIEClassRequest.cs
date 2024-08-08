// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2021. All rights reserved.
// 
//   WC.Services.Hplc
//   WorkViewRequestClassRequest .cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace WC.Services.Hplc.Mappers.v2
{
	using System.Collections.Generic;
	using System.Linq;
	using Hyland.Unity.WorkView;
	using WC.Services.Hplc.Models.v2;
	using ReqConst = Models.v2.Constants.Request;

	public class WorkViewRequestHieClassRequest
		: BaseWorkViewObjectModelMapper<Request>,
			IHieModelMapper<WorkViewObject, Request>
	{
		private readonly IHieModelMapper<WorkViewObject, Provider> _providerMapper;

		public WorkViewRequestHieClassRequest(
			IHieModelMapper<WorkViewObject, Provider> providerMapper)
			=> _providerMapper = providerMapper;

		public override Request GetMappedModel(WorkViewObject original)
		{
			MainClassObject = original;

			Request request = base.GetMappedModel(original);

			request.CareSourceReceivedDate = GetDateValue(ReqConst.CareSourceReceivedDate);
			request.ContactEmail = GetStringValue(ReqConst.ContactEmail);
			request.ChangeEffectiveDate = GetDateValue(ReqConst.ChangeEffectiveDate);
			request.ContactName = GetStringValue(ReqConst.ContactName);
			request.ContactPhone = GetStringValue(ReqConst.ContactPhone);
			request.Date = GetDateTimeValue(ReqConst.Date).Value;
			request.EntityName = GetStringValue(ReqConst.EntityName);
			request.HealthPartnerAgreementType = GetStringValue(ReqConst.HealthPartnerAgreementType);
			request.Id = MainClassObject.Id;
			request.Notes = GetTextValue(ReqConst.Notes);
			request.PrimaryState = GetStringValue(ReqConst.PrimaryState);
			request.SignatoryEmail = GetStringValue(ReqConst.SignatoryEmail);
			request.SignatoryName = GetStringValue(ReqConst.SignatoryName);
			request.SignatoryTitle = GetStringValue(ReqConst.SignatoryTitle);
			request.Source = GetStringValue(ReqConst.Source);
			request.Status = GetStringValue(ReqConst.Status);
			request.Type = GetStringValue(ReqConst.Type);
			request.ApplicationId = GetLongValue(ReqConst.ApplicationNumber);


			if (original.Related.SafeAny())
			{
				IEnumerable<WorkViewObject> providers = original.Related.Where(
					wvo => wvo.ClassName.Equals(Constants.Provider.ClassName));

				request.Providers = providers.SafeAny()
					? providers
						.Select(wvo => _providerMapper.GetMappedModel(wvo))
					: new List<Provider>();
			}

			return request;
		}

		public override WorkViewObject GetMappedModel(Request original)
		{
			WorkViewObject wvo = base.GetMappedModel(original);

			List<WorkViewObject> related = new List<WorkViewObject>();

			related.AddRange(original.Providers.Select(p => _providerMapper.GetMappedModel(p)));
			

			wvo.Related = related;
			return wvo;
		}

		public override WorkViewObject GetMappedModel(Object original)
		{
			WorkViewObject wvo = base.GetMappedModel(original);

			DependentObjectList dependentObjects = GetDependentObjects();

			if (dependentObjects.SafeAny())
			{
				List<WorkViewObject> listDependentObjects = new List<WorkViewObject>();

				listDependentObjects.AddRange(
					from o in dependentObjects
					where o.Class.Name.Equals(Constants.Provider.ClassName)
					select _providerMapper.GetMappedModel(o.Class.GetObjectByID(o.ID)));

				wvo.Related = listDependentObjects;
			}

			return wvo;
		}
	}
}