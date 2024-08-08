namespace CareSource.WC.Services.Document.Models
{
	using System.Collections.Generic;
	using System.ComponentModel.DataAnnotations;

	public class CreateDocumentFileLink
	{
		[Required]
		public string FileName { get; set; }

		[Required]
		public string DocumentType { get; set; }

		[Required]
		public IDictionary<string, string> Keywords { get; set; }
	}
}