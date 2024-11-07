// ------------------------------------------------------------------------------------------------
//  <copyright>
//    Copyright (c) CareSource, 2020-2022.  All rights reserved.
// 
//    Claims
//    FacetsQueueMessage.cs
//  </copyright>
//  ------------------------------------------------------------------------------------------------

namespace Claims.Models
{
	using System;

	public class FacetsQueueMessage
	{
		public int? QueueId
		{
			get;
			set;
		}

		public string QueueName
		{
			get;
			set;
		}

		public string MessageId
		{
			get;
			set;
		}

		public int? MessageType
		{
			get;
			set;
		}

		public int? RoleId
		{
			get;
			set;
		}

		public string RoleName
		{
			get;
			set;
		}

		public int? Status
		{
			get;
			set;
		}

		public DateTime? TargetDate
		{
			get;
			set;
		}

		public DateTime? AdjustedTargetDate
		{
			get;
			set;
		}

		public DateTime? ReceivedDate
		{
			get;
			set;
		}
	}
}