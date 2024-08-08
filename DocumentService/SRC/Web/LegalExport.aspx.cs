// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2023. All rights reserved.
// 
//   WC.Services.Document
//   LegalExport.aspx.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace CareSource.WC.Services.Document.Web
{
	using System;
	using System.Collections.Generic;
	using System.Configuration;
	using System.IO;
	using System.Linq;
	using System.Net.Http;
	using System.Net.Http.Headers;
	using System.Runtime.CompilerServices;
	using System.Text;
	using System.Threading.Tasks;
	using System.Web.UI;
	using System.Web.UI.WebControls;
	using CareSource.WC.Entities.Common;
	using CareSource.WC.OnBase.Core.ExtensionMethods;
	using CareSource.WC.Services.Document.Models.v6;
	using Newtonsoft.Json;
	using Newtonsoft.Json.Converters;

	public partial class LegalExport : Page
	{
		private const string ApplicationJson = "application/json";

		protected void Export_Click(
			object sender,
			EventArgs e)
		{

			// Find the files
			TextBox startDate = (TextBox)Form.FindControl("StartDate");
			TextBox endDate = (TextBox)Form.FindControl("EndDate");
			TextBox subscriberId = (TextBox)Form.FindControl("SubscriberId");
			DropDownList directory = (DropDownList)Form.FindControl("Directory");
			Literal errorMessage = (Literal)Form.FindControl("ErrorMessage");
			Literal successMessage = (Literal)Form.FindControl("SuccessMessage");

			errorMessage.Parent.Visible = false;
			successMessage.Parent.Visible = false;

			string rootFolder = ConfigurationManager.AppSettings["Export.LegalRequests.Root"];

			ExportDocumentRequest request = new ExportDocumentRequest()
			{
				CorrelationGuid = Guid.NewGuid(),
				EndDate = DateTime.Parse(endDate.Text),
				StartDate = DateTime.Parse(startDate.Text),
				Path = Path.Combine(
					rootFolder,
					directory.SelectedValue),
				Filters = new List<Filter>()
				{
					new Filter()
					{
						Name = "Subscriber ID",
						Value = subscriberId.Text
					}
				}
			};

			HttpClient client = ConfigureHttpClient(string.Empty);
			client.Timeout = TimeSpan.FromMinutes(10);
			HttpRequestMessage message = ConfigureHttpRequestMessage(request);
			Task<HttpResponseMessage> response = client.SendAsync(message);
			

			bool completedInTime = response.Wait(TimeSpan.FromMinutes(10));
	

			if (!completedInTime)
			{
				errorMessage.Text = "Export process has timed out.  Please verify results";
				errorMessage.Parent.Visible = true;
				return;
			}

			string result = response.Result.Content.ReadAsStringAsync()
				.Result;

			ExportDocumentResponse sResponse =
				JsonConvert.DeserializeObject<ExportDocumentResponse>(result);

			if (sResponse == null)
			{
				errorMessage.Text = "Response was null.  Please verify results";
				errorMessage.Parent.Visible = true;
			}
			else
			{
				startDate.Text = string.Empty;
				endDate.Text = $"{DateTime.Now.Year}-12-31";
				subscriberId.Text = string.Empty;

				successMessage.Text =
					$"Export complete, {sResponse.SuccessRecordCount} files exported, " +
					$"{sResponse.ErrorRecordCount} files not exported.";
				successMessage.Parent.Visible = true;

				DataList resultsListView = (DataList)Form.FindControl("ResultList");
				resultsListView.DataSource = null;
				resultsListView.DataBind();

				Button exportButton = (Button)Form.FindControl("Export");
				exportButton.Enabled = false;
			}
		}
	

		protected void Page_Load(
			object sender,
			EventArgs e)
		{
			string rootFolder = ConfigurationManager.AppSettings["Export.LegalRequests.Root"];

			if (!IsPostBack)
			{
				TextBox endDate = (TextBox)Form.FindControl("EndDate");
				DropDownList directory =
					(DropDownList)Form.FindControl("Directory");

				endDate.Text = $"{DateTime.Now.Year}-12-31";

				string[] directoryDataSource = System.IO.Directory.GetDirectories(rootFolder);

				directory.DataSource = directoryDataSource.Select(d => new DirectoryInfo(d).Name);
				directory.DataBind();
			}
		}

		protected void Search_Click(
			object sender,
			EventArgs e)
		{
			// Find the files
			TextBox startDate = (TextBox)Form.FindControl("StartDate");
			TextBox endDate = (TextBox)Form.FindControl("EndDate");
			TextBox subscriberId = (TextBox)Form.FindControl("SubscriberId");
			DropDownList directory = (DropDownList)Form.FindControl("Directory");
			DataList resultsListView = (DataList)Form.FindControl("ResultList");

			ExportDocumentRequest request = new ExportDocumentRequest()
			{
				CorrelationGuid = Guid.NewGuid(),
				EndDate = DateTime.Parse(endDate.Text),
				StartDate = DateTime.Parse(startDate.Text),
				Path = directory.SelectedValue,
				Filters = new List<Filter>()
				{
					new Filter()
					{
						Name = "Subscriber ID",
						Value = subscriberId.Text
					}
				}
			};

			HttpClient client = ConfigureHttpClient("search");
			HttpRequestMessage message = ConfigureHttpRequestMessage(request);
			HttpResponseMessage response = client.SendAsync(message).Result;
			string result = response.Content.ReadAsStringAsync().Result;

			ExportDocumentResponse sResponse =
				JsonConvert.DeserializeObject<ExportDocumentResponse>(result);

			if (sResponse == null)
			{
				throw new Exception(
					"Service error",
					new Exception("Response was null."));
			}

			TotalRecords = sResponse.SuccessRecordCount;
			resultsListView.DataSource = sResponse.Documents;
			resultsListView.DataBind();

			Button exportButton = (Button)Form.FindControl("Export");
			exportButton.Enabled = true;
		}

		public int TotalRecords { get; set; }

		private void ConfigureBasicAuth(HttpClient client)
		{
			string user = ConfigurationManager.AppSettings["OnBase.API_User"];
			string password = ConfigurationManager.AppSettings["OnBase.API_Password"];

			client.DefaultRequestHeaders.Authorization =
				new AuthenticationHeaderValue(
					"Basic",
					$"{user}:{password}".Base64Encode());
		}

		private HttpClient ConfigureHttpClient(string route)
		{
			HttpClient client = new HttpClient() {
				BaseAddress = new Uri(
					$"{ConfigurationManager.AppSettings["Document:Service:URL"]}/{route}")
			};

			client.DefaultRequestHeaders.Clear();

			client.DefaultRequestHeaders.Accept.Add(
				new MediaTypeWithQualityHeaderValue(ApplicationJson));

			ConfigureBasicAuth(client);

			return client;
		}

		private static HttpRequestMessage ConfigureHttpRequestMessage(IExportDocumentRequest message)
			=> new HttpRequestMessage(
				HttpMethod.Post,
				string.Empty)
			{
				Content = new StringContent(
					SerializeObject(message),
					Encoding.UTF8,
					ApplicationJson)
			};

		private static string SerializeObject(IExportDocumentRequest message) =>
			JsonConvert.SerializeObject(
				message,
				new JsonSerializerSettings
				{
					Converters = new List<JsonConverter> { new StringEnumConverter() }
				});
	}
}