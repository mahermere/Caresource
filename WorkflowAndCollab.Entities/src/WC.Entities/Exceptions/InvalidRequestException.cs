namespace CareSource.WC.Entities.Exceptions
{
	public class InvalidRequestException : OnBaseExceptionBase
	{
		/// <summary>
		///    Initializes a new instance of the <see cref="NoKeywordsException" /> class.
		/// </summary>
		public InvalidRequestException()
			: base(
				ErrorCode.InvalidRequest,
				"At least one Filter, DocumentType, or a Date Range is required.")
		{ }
	}
}