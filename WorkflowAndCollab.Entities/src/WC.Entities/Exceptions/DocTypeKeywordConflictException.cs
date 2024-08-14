namespace CareSource.WC.Entities.Exceptions
{
	public class DocTypeKeywordConflictException : OnBaseExceptionBase
	{
		/// <summary>
		///    Initializes a new instance of the <see cref="DocTypeKeywordConflictException" /> class.
		/// </summary>
		public DocTypeKeywordConflictException()
			: base(
				ErrorCode.DocTypeKeywordConflict,
				"The combination of Document Types and Keywords did not produce any queryable records")
		{ }
	}
}