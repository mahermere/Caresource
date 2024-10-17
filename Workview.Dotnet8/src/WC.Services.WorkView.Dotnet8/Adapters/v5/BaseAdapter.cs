// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2022. All rights reserved.
// 
//   Workview
//   BaseAdapter.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace WC.Services.WorkView.Dotnet8.Adapters.v5
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	//using CareSource.WC.OnBase.Core.Connection.Interfaces;
	//using CareSource.WC.OnBase.Core.ExtensionMethods;
	using WC.Services.WorkView.Dotnet8.Mappers.v5;
	using WC.Services.WorkView.Dotnet8.Models.v5;
	using Hyland.Unity.WorkView;
	using Application = Hyland.Unity.Application;
	using Attribute = Hyland.Unity.WorkView.Attribute;
	using Microsoft.Extensions.Logging;
    using WC.Services.WorkView.Dotnet8.Repository;
    using WC.Services.WorkView.Dotnet8.Extensions;
    using WC.Services.WorkView.Dotnet8.Mappers.v5.Interfaces;

    public abstract class BaseAdapter : IBaseAdapter
	{
		private readonly (bool, string) _validatedTrue = (true, string.Empty);

        private readonly IRepository _repo;
        protected readonly IModelMapper<WorkViewObject, WorkViewBaseObject> ModelMapper;
		protected Hyland.Unity.WorkView.Application WvApplication;
		protected readonly log4net.ILog Logger;
		private readonly IDataSetAdapter _dataSetAdapter;


		protected BaseAdapter(
			IModelMapper<WorkViewObject, WorkViewBaseObject> modelMapper,
			IRepository repo,
			IDataSetAdapter dataSetAdapter,
			log4net.ILog logger)
		{
			ModelMapper = modelMapper;
			_repo = repo;
			_dataSetAdapter = dataSetAdapter;
			Logger = logger;
		}

		protected (bool isValid, string message) AttributeIsValid(
			Class wvClass,
			Attribute attribute,
			string value)
		{
            if (string.IsNullOrWhiteSpace(value))
            {
				return _validatedTrue;
			}

			IEnumerable<string> dataSet = _dataSetAdapter.GetDataSet(
				WvApplication.Name,
				wvClass,
				attribute.ID);

			if (dataSet.SafeAny())
			{
				if(!dataSet.SafeAny(dsv => dsv.Equals(value, StringComparison.CurrentCultureIgnoreCase)))
				{
					return (false,
						$"Value '{value}' is not valid for the dataset '{wvClass.Name}.{attribute.Name}'");
				}
				else
				{
					return _validatedTrue;
				}
				
			}

			switch (attribute.AttributeType)
			{
				case AttributeType.FormattedText:
				case AttributeType.EncryptedAlphanumeric:
				case AttributeType.Alphanumeric:
					return attribute.DataLength >= value.Trim().Length
						? (true, string.Empty)
						: (false, $"Value '{value}' is too long for {attribute.Name}" +
						          $", max length is [{attribute.DataLength}].");

				case AttributeType.Boolean:

					return value.ToSafeBool().HasValue
						? _validatedTrue
						: (false, $"Value '{value}' is not a valid Boolean value.");

				case AttributeType.Currency:
					return value.ToSafeDecimal().HasValue
						? _validatedTrue
						: (false, $"Value '{value}' is not a valid Currency value.");

				case AttributeType.Date:
					return value.ToSafeDateTime().HasValue
						? _validatedTrue
						: (false, $"Value '{value}' is not a valid Date value.");

				case AttributeType.DateTime:
					return value.ToSafeDateTime().HasValue
						? _validatedTrue
						: (false, $"Value '{value}' is not a valid DateTime value.");

				case AttributeType.Decimal:
					return value.ToSafeDecimal().HasValue
						? _validatedTrue
						: (false, $"Value '{value}' is not a valid Decimal value.");

				case AttributeType.Document:
					return value.ToSafeLong().HasValue
						? _validatedTrue
						: (false, $"Value '{value}' is not a valid Document Id.");

				case AttributeType.Float:
					return value.ToSafeDouble().HasValue
						? _validatedTrue
						: (false, $"Value '{value}' is not a valid Floating Point value.");

				case AttributeType.Relation:
					return value.ToSafeLong().HasValue
						? _validatedTrue
						: (false, $"Value '{value}' is not a valid Relationship Id value.");

				case AttributeType.Integer:
					return value.ToSafeLong().HasValue
						? _validatedTrue
						: (false, $"Value '{value}' is not a valid Integer value.");

				case AttributeType.Text:
					return _validatedTrue;

				default:
					throw new ArgumentOutOfRangeException();
			}
		}

		protected IEnumerable<Class> GetWorkViewClasses()
			=> WvApplication.Classes.ToList();

		protected Class GetWvClass(string className)
		{
			if (WvApplication.Classes.Any(c =>
					c.Name.Equals(
						className,
						StringComparison.InvariantCultureIgnoreCase)))

			{
				return WvApplication.Classes.First(cn =>
					cn.Name.Equals(
						className,
						StringComparison.InvariantCultureIgnoreCase));
			}

			return null;
		}

		/// <summary>Loads the wv application.</summary>
		/// <param name="applicationName">Name of the application.</param>
		/// <returns></returns>
		/// <exception cref="System.ArgumentException">
		///    Could not find WorkView Application Name
		///    '{applicationName}'.
		/// </exception>
		protected void SetWorkViewApplication(string applicationName)
		{
			Hyland.Unity.WorkView.Application wvApplication = _repo.Application.WorkView.Applications.Find(applicationName);

			WvApplication = wvApplication ?? throw new ArgumentException(
				$"Could not find WorkView Application Name '{applicationName}'.");
		}
	}
}