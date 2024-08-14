namespace CareSource.WC.Entities.Requests.Base
{
	using CareSource.WC.Entities.Requests.Base.Interfaces;

	public abstract class PagingRequest : BaseRequest, IPagingRequest
	{
		public Paging Paging { get; set; } = new Paging();
	}
}