using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WC.Services.OnBase.Dotnet8.Models.v1
{

	using Hyland.Unity.WorkView;
	using Newtonsoft.Json;
	using Newtonsoft.Json.Converters;
    using WC.Services.OnBase.Dotnet8.Models.Base;
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