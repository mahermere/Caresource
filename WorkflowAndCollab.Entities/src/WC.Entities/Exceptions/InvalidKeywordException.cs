namespace CareSource.WC.Entities.Exceptions
{
	public class InvalidKeywordException : OnBaseExceptionBase
	{
		/// <summary>
		///    Initializes a new instance of the <see cref="InvalidKeywordException" /> class.
		/// </summary>
		/// <param name="keyword">The keyword.</param>
		public InvalidKeywordException(
			string keyword)
			: base(
				ErrorCode.InvalidKeyword,
				$"Invalid Keyword or Display Column: '{keyword}'")
		{ }
	}
}