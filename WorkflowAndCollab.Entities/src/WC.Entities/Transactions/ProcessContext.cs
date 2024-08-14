// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2019. All rights reserved.
// 
//   Entities
//   ProcessContext.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace CareSource.WC.Entities.Transactions
{
	using System.Collections.Generic;
	using System.Runtime.Serialization;

	[DataContract]
	public class ProcessContext
	{
		public ActivityContext ActivityContext { get; set; }

		public string ActivityName { get; set; }

		public List<ContextList> ContextList { get; set; }

		public ExceptionContext Exception { get; set; }
	}
}