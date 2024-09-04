// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2021. All rights reserved.
// 
//   WC.Services.Hplc
//   WorkViewRequestClassRequest .cs
// </copyright>
// ------------------------------------------------------------------------------------------------

using WC.Services.Hplc.Dotnet8;
using WC.Services.Hplc.Dotnet8.Models;

namespace WC.Services.Hplc.Dotnet8.Mappers
{
    using Hyland.Unity.WorkView;
    using System.Collections.Generic;
    using System.Linq;
  //  using Hyland.Unity.WorkView;
    using WC.Services.Hplc.Dotnet8.Mappers.Interfaces;
    using Attribute = Models.Attribute;
    using ReqConst = Constants.Request;

    public class WorkViewRequestClassRequest : BaseWorkViewObjectModelMapper<Request>
    {
        private readonly IModelMapper<WorkViewObject, Product> _productMapper;
        private readonly IModelMapper<WorkViewObject, Provider> _providerMapper;
        private readonly IModelMapper<WorkViewObject, Tin> _tinMapper;

        public WorkViewRequestClassRequest(
            IModelMapper<WorkViewObject, Provider> providerMapper,
            IModelMapper<WorkViewObject, Product> productMapper,
            IModelMapper<WorkViewObject, Tin> tinMapper)
        {
            _providerMapper = providerMapper;
            _productMapper = productMapper;
            _tinMapper = tinMapper;
        }

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
                IEnumerable<WorkViewObject> products = original.Related.Where(
                    wvo => wvo.ClassName.Equals(Constants.Products.Product));

                request.Products = products.SafeAny()
                    ? products
                        .Select(wvo => _productMapper.GetMappedModel(wvo))
                    : new List<Product>();

                IEnumerable<WorkViewObject> providers = original.Related.Where(
                    wvo => wvo.ClassName.Equals(Constants.Provider.ClassName));

                request.Providers = providers.SafeAny()
                    ? providers
                        .Select(wvo => _providerMapper.GetMappedModel(wvo))
                    : new List<Provider>();

                IEnumerable<WorkViewObject> tins = original.Related.Where(
                    wvo => wvo.ClassName.Equals(Constants.Tin.ClassName));

                request.EntityTin = tins.SafeAny()
                    ? _tinMapper.GetMappedModel(tins.First())
                        .EntityTin
                    : null;
            }

            return request;
        }

        public override WorkViewObject GetMappedModel(Request original)
        {
            WorkViewObject wvo = base.GetMappedModel(original);

            List<WorkViewObject> related = new List<WorkViewObject>();

            related.AddRange(original.Providers.Select(p => _providerMapper.GetMappedModel(p)));
            related.AddRange(original.Products.Select(p => _productMapper.GetMappedModel(p)));

            if (!String.IsNullOrWhiteSpace(original.EntityTin))
            {
                related.Add(
                    new WorkViewObject
                    {
                        ClassName = Constants.Tin.ClassName,
                        Attributes = new List<Attribute>
                        {
                            new Attribute
                            {
                                Name = Constants.Tin.EntityTin,
                                Value = original.EntityTin
                            }
                        }
                    });
            }

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
                    where o.Class.Name.Equals(Constants.Tin.ClassName)
                    select _tinMapper.GetMappedModel(o.Class.GetObjectByID(o.ID)));

                listDependentObjects.AddRange(
                    from o in dependentObjects
                    where o.Class.Name.Equals(Constants.Provider.ClassName)
                    select _providerMapper.GetMappedModel(o.Class.GetObjectByID(o.ID)));

                listDependentObjects.AddRange(
                    from o in dependentObjects
                    where o.Class.Name.Equals(Constants.Products.ClassName)
                    select _productMapper.GetMappedModel(o.Class.GetObjectByID(o.ID)));

                wvo.Related = listDependentObjects;
            }

            return wvo;
        }
    }
}