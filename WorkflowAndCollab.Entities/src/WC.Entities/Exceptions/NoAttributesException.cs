namespace CareSource.WC.Entities.Exceptions
{
	public class NoAttributesException : OnBaseExceptionBase
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="NoAttributesException"/> class.
		/// </summary>
		public NoAttributesException()
			: base(
				ErrorCode.NoKeywords,
				"No Attributes were provided, please add at least one Attribute.")
		{ }
	}
}