namespace CareSource.WC.Entities.Exceptions
{
	public class InvalidFilterException : OnBaseExceptionBase
	{
		/// <summary>
		///    Initializes a new instance of the <see cref="T:InvalidFilterException" /> class.
		/// </summary>
		/// <param name="attributeName">Type of the document.</param>
		/// <inheritdoc />
		public InvalidFilterException(
			string attributeName)
			: base(
				ErrorCode.InvalidWorkViewFilterName,
				$"Invalid WorkView Filter Name: '{attributeName}'")
		{ }
	}
}