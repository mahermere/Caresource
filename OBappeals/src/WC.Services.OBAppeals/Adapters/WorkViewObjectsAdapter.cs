// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2019. All rights reserved.
// 
//   WorkFlowAndCollab.Integrations
//   WorkViewObjectsBroker.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace CareSource.WC.Services.OBAppeals.Adapters
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using CareSource.WC.Entities.Appeals;
	using CareSource.WC.Entities.Appeals.Interfaces;
	using CareSource.WC.Entities.Common;
	using CareSource.WC.Entities.Common.Interfaces;
	using CareSource.WC.Entities.Exceptions;
	using CareSource.WC.OnBase.Core.Connection.Interfaces;
	using CareSource.WC.OnBase.Core.ExtensionMethods;
	using CareSource.WC.Services.OBAppeals.Adapters.Interfaces;
	using Hyland.Unity.WorkView;
	using Application = Hyland.Unity.Application;
	using Attribute = Hyland.Unity.WorkView.Attribute;
	using WorkViewApplication = Hyland.Unity.WorkView.Application;
	using Filter = CareSource.WC.Entities.Common.Filter;
	using Object = Hyland.Unity.WorkView.Object;

	public class WorkViewObjectsAdapter : IWorkViewObjectsAdapter<WorkViewObjectsHeader>
	{
		private Application HylandApp => _applicationConnectionAdapter.Application;

		private WorkViewApplication _workViewApplication;

		private readonly IApplicationConnectionAdapter<Application> _applicationConnectionAdapter;

		public WorkViewObjectsAdapter(
			IApplicationConnectionAdapter<Application> applicationConnectionAdapter)
		{
			_applicationConnectionAdapter = applicationConnectionAdapter;
		}

		public ISearchResults<WorkViewObjectsHeader> Search(IListWorkViewObjectsRequest request)
		{
			_workViewApplication =
				HylandApp.WorkView.Applications.Find(request.WorkViewApplicationName);

			Class wvClass = ValidateWorkviewApplicationAndClass(request.WorkViewClassName);

			DynamicFilterQuery dfq = wvClass.CreateDynamicFilterQuery();

			AddDisplayAttributes(request, wvClass, dfq);

			AddFilter(request, wvClass, dfq);

			FilterQueryResultItemList queryresults = dfq.Execute(100);

			SearchResults<WorkViewObjectsHeader> search = new SearchResults<WorkViewObjectsHeader>();
			search.TotalRecordCount = queryresults.Count;

			IEnumerable<FilterQueryResultItem> items = queryresults
				.Skip((request.Paging.PageNumber - 1) * request.Paging.PageSize)
				.Take(request.Paging.PageSize);

			List<WorkViewObjectsHeader> results = new List<WorkViewObjectsHeader>(items.Count());

			foreach (FilterQueryResultItem item in items)
			{
				if (wvClass.Attributes.Any(a => a.AttributeType.Equals(AttributeType.Text)))
				{
					_wvObject = wvClass.GetObjectByID(item.ObjectID);
				}

				results.Add(new WorkViewObjectsHeader
				{
					ObjectId = item.ObjectID,
					DisplayAttributes = request.DisplayColumns
						.ToDictionary(
							dc => dc,
							dc => GetAttributeValue(
								item,
								dc,
								string.Empty))
				});
			}

			search.Results = results.AsEnumerable();

			return search;
		}

		public WorkViewObjectsHeader GetWVObject(long objectId, WorkviewObjectItemRequest request)
		{
			_workViewApplication =
				HylandApp.WorkView.Applications.Find(request.WorkViewApplicationName);

			Class wvClass = ValidateWorkviewApplicationAndClass(request.WorkViewClassName);

			Object workviewobject = wvClass.GetObjectByID(objectId);

			if (workviewobject == null)
			{
				throw new NoResultsFoundException(objectId.ToString());
			}

			string noteClassName = request.WorkViewClassName == "Grievance"
				? "Note"
				: "Notes";

			Class noteClass = ValidateWorkviewApplicationAndClass(noteClassName);

			List<AllNotes> intakeNotes = GetNotes(
				objectId,
				noteClass,
				request.WorkViewClassName
				);

			WorkViewObjectsHeader appeal = new WorkViewObjectsHeader
			{
				ObjectId = workviewobject.ID,
				DisplayAttributes = request.DisplayColumns
						.ToDictionary(
							dc => dc,
							dc => GetWVObjectAttributeValue(
								workviewobject,
								dc,
								string.Empty))
			};

			appeal.DisplayAttributes.Add("AllNotes", intakeNotes);

			return appeal;
		}

		// Private methods
		private Class ValidateWorkviewApplicationAndClass(
			string workViewClassName)
		{
			// Access particular Class by providing the Class Name
			Class wvClass = _workViewApplication.Classes.Find(workViewClassName);

			// Verify Class
			if (wvClass == null)
			{
				throw new InvalidClassNameException(workViewClassName);
			}

			return wvClass;
		}

		private static void AddDisplayAttributes(
			IListWorkViewObjectsRequest request,
			Class wvClass,
			DynamicFilterQuery dfq)
		{
			foreach (string attributeName in request.DisplayColumns)
			{
				// In case there is a dotted address, the attribute will be truncated
				// so it will pass verification check.
				string attrName = attributeName.Split('.')[0];

				// Verify Display Attributes
				Attribute wvAttribute = wvClass.Attributes.Find(attrName);

				if (wvAttribute == null)
				{
					throw new InvalidAttributeNameException(attrName);
				}

				dfq.AddFilterColumn(attributeName);
			}
		}

		private static void AddFilter(
			IListWorkViewObjectsRequest request,
			Class wvClass,
			DynamicFilterQuery dfq)
		{
			foreach (Filter filter in request.Filters)
			{
				string attrName = filter.Name.Split('.')[0];

				// "classname" is system attribute that is set as "Grievance" for normal
				// Grievances and set as "QICases" for Clinical Grievances.
				if (attrName != "classname")
				{
					// Verify Filters (Search Criteria)
					Attribute attribute = wvClass.Attributes.Find(attrName);

					if (attribute == null)
					{
						throw new InvalidAttributeNameException(attrName);
					}

					if (attribute.AttributeType == AttributeType.Date
						|| attribute.AttributeType == AttributeType.DateTime)
					{
						if (!filter.Value.IsNullOrWhiteSpace()
							&& !DateTime.TryParse(filter.Value, out _))
						{
							throw new Exception(
								$"Filter Name {filter.Name}'s Value: ({filter.Value}) is not in a known format.");
						}
					}
				}

				dfq.AddConstraint(
					filter.Name,
					MapToWorkViewOperator(filter.CompareOperator),
					filter.Value);
			}
		}

		private static Operator MapToWorkViewOperator(CompareOperator compareOperator)
		{
			switch (compareOperator)
			{
				case CompareOperator.LessThan:
					return Operator.LessThan;

				case CompareOperator.GreaterThan:
					return Operator.GreaterThan;

				case CompareOperator.LessThanEqual:
					return Operator.LessThanEqual;

				case CompareOperator.GreaterThanEqual:
					return Operator.GreaterThanEqual;

				case CompareOperator.Like:
					return Operator.Like;

				case CompareOperator.NotLike:
					return Operator.NotLike;

				case CompareOperator.Null:
					return Operator.Null;

				case CompareOperator.NotNull:
					return Operator.NotNull;

				default:
					return Operator.Equal;
			}
		}

		private Object _wvObject;
		

		private object GetAttributeValue(
			FilterQueryResultItem item,
			string columnName,
			string defaultValue)
		{
			AttributeValue attvalue = _wvObject?.AttributeValues.Find(columnName);

			if (attvalue != null
				&& attvalue.Attribute.AttributeType == AttributeType.Text)
			{
				return attvalue.HasValue
					? attvalue.TextValue
					: defaultValue;
			}

			FilterColumnValue column = item.GetFilterColumnValue(columnName);

			return column == null
				|| !column.HasValue
					? defaultValue
					: column.Value;
		}

		private object GetWVObjectAttributeValue(
			Object item,
			string columnName,
			string defaultValue)
		{
			//AttributeValue attvalue = _wvObject?.AttributeValues.Find(columnName);
			AttributeValue attvalue = item?.AttributeValues.Find(columnName);

			if (attvalue != null
				&& attvalue.Attribute.AttributeType == AttributeType.Text)
			{
				return attvalue.HasValue
					? attvalue.TextValue
					: defaultValue;
			}

			if (columnName.Length >= 6 && columnName.Substring(0,6) == "linkto")
			{
				Class linktoObjectClass = null;
				string linktoAttrName = columnName.Split('.')[0];

				linktoObjectClass =
					ValidateWorkviewApplicationAndClass(linktoAttrName == "linktoAssociatedClaim"
					? "AssociatedClaim"
					: "ProviderInformation");

				AttributeValue linktoColumn = item.AttributeValues.Find((linktoAttrName));

				if (linktoColumn == null
					|| !linktoColumn.HasValue)
				{
					return defaultValue;
				}

				long linktoObjectId = linktoColumn.RelationshipValue;
				Object linktoObject = linktoObjectClass.GetObjectByID(linktoObjectId);
				linktoAttrName = columnName.Split('.')[1];
				linktoColumn = linktoObject.AttributeValues.Find((linktoAttrName));

				return linktoColumn == null
					|| !linktoColumn.HasValue
						? defaultValue
						: linktoColumn.ToString();

			}

			AttributeValue column = item.AttributeValues.Find(columnName);

			return column == null
				|| !column.HasValue
					? defaultValue
					: column.ToString();
		}

		private List<AllNotes> GetNotes(long objectId, Class noteClass, string className)
		{
			DynamicFilterQuery dnotesfq = noteClass.CreateDynamicFilterQuery();

			switch (className)
			{
				case "Grievance":
					dnotesfq.AddConstraint($"Grievance.objectID", Operator.Equal, objectId);
					break;
				case "Case":
					dnotesfq.AddConstraint($"linktoCase.objectID", Operator.Equal, objectId);
					break;
			}

			FilterQueryResultItemList noteQueryResults = dnotesfq.Execute(20);

			List<AllNotes> notesList = new List<AllNotes>(noteQueryResults.Count);

			foreach (FilterQueryResultItem note in noteQueryResults)
			{
				Object wvNote = noteClass.GetObjectByID(note.ObjectID);
				if (wvNote != null)
				{
					switch (className)
					{
						case "Grievance":
							notesList.Add(
								new AllNotes()
								{
									CreatedBy = wvNote.AttributeValues != null
										&& wvNote.AttributeValues[1]
											.HasValue
											? wvNote.AttributeValues[1]
												.ToString() : "",
									NoteType = wvNote.AttributeValues != null
										&& wvNote.AttributeValues[2]
											.HasValue
											? wvNote.AttributeValues[2]
												.ToString() : "",
									Note = wvNote.AttributeValues != null
										&& wvNote.AttributeValues[0]
											.HasValue
											? wvNote.AttributeValues[0]
												.TextValue : "",
									NoteDate = wvNote.CreatedDate
								});
							break;

						case "Case":
							int indexCreatedBy, indexNoteType, indexNote = 0;
							
							if (wvNote.AttributeValues[1].HasValue
								&& wvNote.AttributeValues[1]
								.ToString() == "WFTIMER")
							{
								indexCreatedBy = 1;
								indexNoteType = 0;
								indexNote = 4;
							}
							else
							{
								indexCreatedBy = 2;
								indexNoteType = 1;
								indexNote = 0;
							}

							notesList.Add(
								new AllNotes()
								{
									CreatedBy = wvNote.AttributeValues[indexCreatedBy] != null
										&& wvNote.AttributeValues[indexCreatedBy]
											.HasValue
											? wvNote.AttributeValues[indexCreatedBy].ToString() : "",
									NoteType = wvNote.AttributeValues[indexNoteType] != null
										&& wvNote.AttributeValues[indexNoteType]
											.HasValue
											? wvNote.AttributeValues[indexNoteType].ToString() : "",
									Note = wvNote.AttributeValues[indexNote] != null
										&& wvNote.AttributeValues[indexNote]
											.HasValue
												? wvNote.AttributeValues[indexNote].ToString() : "",
									NoteDate = wvNote.CreatedDate
								});
							break;
					}
				}
			}

			return notesList;
		}
	}
}