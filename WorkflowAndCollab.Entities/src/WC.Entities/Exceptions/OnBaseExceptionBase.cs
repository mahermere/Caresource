namespace CareSource.WC.Entities.Exceptions
{
	using System;

	public abstract class OnBaseExceptionBase : Exception
	{
		/// <summary>
		///    Initializes a new instance of the <see cref="OnBaseExceptionBase" /> class.
		/// </summary>
		/// <param name="errorcode">The errorcode.</param>
		/// <param name="message">The message.</param>
		protected OnBaseExceptionBase(
			ErrorCode errorcode,
			string message)
			: base(message) => ErrorCode = errorcode;

		/// <summary>
		///    Gets the error code of the on base exception base class.
		/// </summary>
		public ErrorCode ErrorCode { get; }
	}
}