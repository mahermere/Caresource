namespace CareSource.WC.Entities.Exceptions
{
	public class InvalidDocumentTypeException : OnBaseExceptionBase
	{
		/// <summary>
		///    Initializes a new instance of the <see cref="T:InvalidDocumentTypeException" /> class.
		/// </summary>
		/// <param name="documentType">Type of the document.</param>
		/// <inheritdoc />
		public InvalidDocumentTypeException(
			string documentType)
			: base(
				ErrorCode.InvalidDocumentType,
				$"Invalid Document Type: '{documentType}'")
		{ }
	}
}