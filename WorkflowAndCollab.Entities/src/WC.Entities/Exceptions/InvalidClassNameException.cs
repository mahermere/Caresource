namespace CareSource.WC.Entities.Exceptions
{
	public class InvalidClassNameException : OnBaseExceptionBase
	{
		/// <summary>
		///    Initializes a new instance of the <see cref="T:InvalidClassNameException" /> class.
		/// </summary>
		/// <param name="className">Type of the document.</param>
		/// <inheritdoc />
		public InvalidClassNameException(
			string className)
			: base(
				ErrorCode.InvalidWorkViewClassName,
				$"Invalid WorkView Class Name: '{className}'")
		{ }
	}
}