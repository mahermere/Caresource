namespace HplcManagement
{
	using System.Collections.Specialized;
	using System.Runtime.Caching;

	public class DataSetMemoryCache : MemoryCache
	{
		public DataSetMemoryCache(string name,
			NameValueCollection config = null)
			: base(name,
				config)
		{ }

		public DataSetMemoryCache(string name,
			NameValueCollection config,
			bool ignoreConfigSection)
			: base(name,
				config,
				ignoreConfigSection)
		{ }
	}
}