namespace CareSource.WC.Services.Document.Models.v6
{
	using System;
	using Newtonsoft.Json;

	public class ExceptionResponse<TResult> : BaseResponse<TResult>
	{
		public ExceptionResponse(
			string title,
			int status,
			Guid correlationGuid,
			TResult result)
			: base(title,
				status,
				correlationGuid,
				result)
		{ }

		public ExceptionResponse(
			string title,
			int status,
			Guid correlationGuid)
			: base(title,
				status,
				correlationGuid)
		{ }

		[JsonProperty("Result")]
		public override TResult ResponseData { get; set; }
	}
}