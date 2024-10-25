namespace WC.Services.Document.Dotnet8.Models.v6
{
	using System;
	using System.Text.Json.Serialization;
    using WC.Services.Document.Dotnet8.Models.v6.Responses;

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

		[JsonPropertyName("Result")]
		public override TResult ResponseData { get; set; }
	}
}