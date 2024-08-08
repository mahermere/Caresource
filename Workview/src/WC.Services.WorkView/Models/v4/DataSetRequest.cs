// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2020. All rights reserved.
// 
//   Workview
//   DataSetRequest.cs
// </copyright>
// ------------------------------------------------------------------------------------------------
namespace CareSource.WC.Services.WorkView.Models.v4
{
	using System;
	using System.ComponentModel.DataAnnotations;

	public class DataSetRequest : WorkViewRequest, IWorkViewDataSetRequest
	{
		public DataSetRequest()
		{ }

		public DataSetRequest(IWorkViewRequest request, string dataSetName)
		{
			ApplicationName = request.ApplicationName;
			ClassName = request.ClassName;
			CorrelationGuid = request.CorrelationGuid;
			RequestDateTime = request.RequestDateTime;
			SourceApplication = request.SourceApplication;
			UserId = request.UserId;
			DataSetName = dataSetName;
		}

		[Required]
		public string DataSetName { get; set; }
	}
}