// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2019. All rights reserved.
// 
//   WC.Services.OnBase
//   WorkViewAdapter.Applications.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace WC.Services.OnBase.Dotnet8.Adapters.v1
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
    using global::WC.Services.OnBase.Dotnet8.Connection.Interfaces;
    using global::WC.Services.OnBase.Dotnet8.Models.v1;
    using Hyland.Unity.WorkView;
	using Application  = Hyland.Unity.Application;
	using Object = Hyland.Unity.WorkView.Object;

	public partial class WorkViewAdapter
	{
		public IEnumerable<WVApplication> GetApplications()
			=> _repo
                .Application
				.WorkView
				.Applications
				.Select(
					application => new WVApplication
					{
						Id = application.ID,
						Name = application.Name
					});

		public WVApplication GetApplicationById(long applicationId)
		{
			Hyland.Unity.WorkView.Application application = 
			 _repo
				.Application
				.WorkView
				.Applications
				.FirstOrDefault(a => a.ID.Equals(applicationId));

			if (application == null)
			{
				return null;
			}

			return new WVApplication
				{
					Id = application.ID,
					Name = application.Name,
					Classes = application.Classes.Select(c => new WVClass(c)),
					Filters = application.Filters.Select(f => new WVFilters(f))
				};
		}

		public WVApplication GetApplicationByName(string applicationName)
		{
			Hyland.Unity.WorkView.Application application =
				_repo
					.Application
					.WorkView
					.Applications
					.FirstOrDefault(a => a.Name.Equals(applicationName));

			if (application == null)
			{
				return null;
			}

			return new WVApplication
			{
				Id = application.ID,
				Name = application.Name,
				Classes = application.Classes.Select(c => new WVClass(c)),
				Filters = application.Filters.Select(f => new WVFilters(f))
			};
		}
	}
}