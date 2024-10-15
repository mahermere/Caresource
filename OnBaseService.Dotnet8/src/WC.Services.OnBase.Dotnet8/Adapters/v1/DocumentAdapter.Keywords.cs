// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2019. All rights reserved.
// 
//   WC.Services.OnBase
//   DocumentAdapter.Keywords.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace WC.Services.OnBase.Dotnet8.Adapters.v1
{
    using global::WC.Services.OnBase.Dotnet8.Models.v1;
    using System;
	using System.Collections.Generic;
	using System.Linq;
	using DocumentType = Hyland.Unity.DocumentType;


	public partial class DocumentAdapter
	{
		public IEnumerable<Keyword> Keywords()
			=> _repo.Application.Core.KeywordTypes
				.Select(
					kw => new Keyword
					{
						Name = kw.Name,
						Id = kw.ID,
						DataType = kw.DataType.ToString(),
						Mask = kw.MaskedString,
						MaxLength = kw.DataLength
					});

		public IEnumerable<Keyword> Keywords(long documentTypeId)
		{
			DocumentType doctype = _repo.Application.Core.DocumentTypes
				.FirstOrDefault(dt => dt.ID == documentTypeId);

			List<Keyword> keywords =new List<Keyword>();
			List<Keyword> hiddenKeywords = new List<Keyword>();
			List<Keyword> readOnlyKeywords = new List<Keyword>();
			List<Keyword> requiredKeywords = new List<Keyword>();

			if (doctype != null)
			{

				hiddenKeywords.AddRange(
					doctype.HiddenKeywordTypes.Select(
						hidden => new Keyword
						{
							Id = hidden.ID,
							Name = hidden.Name,
							DataType = hidden.DataType.ToString(),
							MaxLength = hidden.DataLength,
							Mask = hidden.MaskedString,
							Hidden = true
						}));

				readOnlyKeywords.AddRange(
					doctype.ReadOnlyKeywordTypes.Select(
						hidden => new Keyword
						{
							Id = hidden.ID,
							Name = hidden.Name,
							DataType = hidden.DataType.ToString(),
							MaxLength = hidden.DataLength,
							Mask = hidden.MaskedString,
							Hidden = true
						}));

				requiredKeywords.AddRange(
					doctype.KeywordTypesRequiredForArchival.Select(
						hidden => new Keyword
						{
							Id = hidden.ID,
							Name = hidden.Name,
							DataType = hidden.DataType.ToString(),
							MaxLength = hidden.DataLength,
							Mask = hidden.MaskedString,
							Hidden = true
						}));

				foreach (Hyland.Unity.KeywordRecordType krt in doctype.KeywordRecordTypes)
				{
					keywords.AddRange(
						krt.KeywordTypes
							.Where(kt => keywords.All(ext => ext.Name != kt.Name))
							.Select(
							kt => new Keyword
							{
								Id = kt.ID,
								Name = kt.Name,
								DataType = kt.DataType.ToString(),
								MaxLength = kt.DataLength,
								Mask = kt.MaskedString,
								Hidden = hiddenKeywords.Any( k => k.Id == kt.ID),
								ReadOnly = readOnlyKeywords.Any(k => k.Id == kt.ID),
								Required = requiredKeywords.Any(k => k.Id == kt.ID)
							}));
				}
			}

			return keywords.OrderBy(k => k.Name);
		}
	}
}