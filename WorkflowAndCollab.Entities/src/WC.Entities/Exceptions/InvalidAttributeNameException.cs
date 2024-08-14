namespace CareSource.WC.Entities.Exceptions
{
	public class InvalidAttributeNameException : OnBaseExceptionBase
	{
		/// <summary>
		///    Initializes a new instance of the <see cref="T:InvalidAttributeNameException" /> class.
		/// </summary>
		/// <param name="attributeName">Type of the document.</param>
		/// <inheritdoc />
		public InvalidAttributeNameException(
			string attributeName)
			: base(
				ErrorCode.InvalidWorkViewAttributeName,
				$"Invalid WorkView Attribute Name: '{attributeName}'")
		{ }
	}
}