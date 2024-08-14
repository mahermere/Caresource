namespace CareSource.WC.Entities.Exceptions
{
	public class InvalidApplicationNameException : OnBaseExceptionBase
	{
		/// <summary>
		///    Initializes a new instance of the <see cref="T:InvalidApplicationNameException" /> class.
		/// </summary>
		/// <param name="applicationName">Type of the document.</param>
		/// <inheritdoc />
		public InvalidApplicationNameException(
			string applicationName)
			: base(
				ErrorCode.InvalidWorkViewApplicationName,
				$"Invalid WorkView Application Name: '{applicationName}'")
		{ }
	}
}