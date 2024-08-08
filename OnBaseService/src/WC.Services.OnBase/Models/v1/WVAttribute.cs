using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CareSource.WC.Services.OnBase.Models.v1
{
	using CareSource.WC.Services.OnBase.Models.Base;
	using Hyland.Unity.WorkView;
	using Newtonsoft.Json;
	using Newtonsoft.Json.Converters;
	using Attribute = System.Attribute;

	public class WVAttribute : BaseModel
	{
		public WVAttribute(Hyland.Unity.WorkView.Attribute a)
		{
			Id = a.ID;
			Name = a.Name;
			AttributeType = a.AttributeType;
			DataLength = a.DataLength;
		}

		[JsonConverter(typeof(StringEnumConverter))]
		public AttributeType AttributeType { get; set; }

		public long DataLength { get; set; }

		public string DataSetName { get; set; }
	}
}