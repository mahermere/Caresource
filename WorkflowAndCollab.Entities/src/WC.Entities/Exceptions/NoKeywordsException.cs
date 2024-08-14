namespace CareSource.WC.Entities.Exceptions
{
	public class NoKeywordsException : OnBaseExceptionBase
	{
		/// <summary>
		///    Initializes a new instance of the <see cref="NoKeywordsException" /> class.
		/// </summary>
		public NoKeywordsException()
			: base(
				ErrorCode.NoKeywords,
				"No Keywords were provided, please add at least one Keyword.")
		{ }
	}
}