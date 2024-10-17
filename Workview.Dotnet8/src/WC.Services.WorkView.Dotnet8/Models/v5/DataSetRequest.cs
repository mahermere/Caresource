// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2022. All rights reserved.
// 
//   Workview
//   DataSetRequest.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace WC.Services.WorkView.Dotnet8.Models.v5
{
	using System.ComponentModel.DataAnnotations;

	public class DataSetRequest : WorkViewRequest, IWorkViewDataSetRequest
	{
		public DataSetRequest()
		{ }

		public DataSetRequest(IWorkViewRequest request, string dataSetName)
		{
			//TODO: finish
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