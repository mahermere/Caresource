// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2021. All rights reserved.
// 
//   ImportProcessor
//   CPSManifest.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace WC.Services.ImportProcessor.Dotnet8.Models.v1

{
	public class CpsManifest
	{
		public Document[] documents { get; set; }
	}

	public class Document
	{
		public DocumentAttr document { get; set; }
	}

	public class DocumentAttr
	{
		public string fileName { get; set; }
		public string documentTypeNumber { get; set; }
		public Keyword[] keywords { get; set; }
		public Claim[] claims { get; set; }
	}

	public class Keyword
	{
		public string Name { get; set; }
		public string Value { get; set; }
	}

	public class Claim
	{
		public Attribute[] attributes { get; set; }
	}

	public class Attribute
	{
		public string Name { get; set; }
		public string Value { get; set; }
	}
}