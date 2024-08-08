using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WC.Services.Hplc
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