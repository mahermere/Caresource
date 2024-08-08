<%@ Page Language="C#" CodeBehind="LegalExport.aspx.cs" Inherits="CareSource.WC.Services.Document.Web.LegalExport" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<%--<script runat="server">

</script>--%>

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
	<meta charset="utf-8"/>
	<title>Document Service, Export for members</title>
	<link href="favicon.ico" rel="shortcut icon" type="image/x-icon"/>
	<meta name="viewport" content="width=device-width, initial-scale=1.0">
	<link href="Content/bootstrap.css" rel="stylesheet"/>
	<link href="Content/Site.css" rel="stylesheet"/>
	<script src="Scripts/modernizr-2.8.3.js"></script>
</head>
<style>
	.overlay {
		background: rgba(33, 66, 99, 0.8) center no-repeat;
		display: none;
		height: 100%;
		left: 0;
		padding-top: 10%;
		position: fixed;
		top: 0;
		width: 100%;
		z-index: 999;
	}

	body { /*url("Content/processing.gif")*/ }

	/* Turn off scrollbar when body element has the loading class */

	body.loading { overflow: hidden; }

	/* Make spinner image visible when body element has the loading class */

	body.loading .overlay {
		display: block;
		text-align: center;
	}

	.message {
		border: 2px solid;
		display: inline-block;
		margin: 5px 0px;
		padding: 3px;
		width: 100%;
	}

	.error {
		background-color: lightpink;
		border-color: darkred;
	}

	.warn {
		background-color: lightgoldenrodyellow;
		border-color: darkorange;
	}

	.success {
		background-color: azure;
		border-color: darkblue;
	}
</style>
<body>
<div class="navbar navbar-inverse navbar-fixed-top">
	<div class="container">
		<div class="navbar-header">
			<button type="button" class="navbar-toggle" data-toggle="collapse" data-target=".navbar-collapse" title="more options">
				<span class="icon-bar"></span>
				<span class="icon-bar"></span>
				<span class="icon-bar"></span>
			</button>
			<a class="navbar-brand" href="/">Legal Export Request</a>
		</div>
		<div class="navbar-collapse collapse">
			<ul class="nav navbar-nav">
				<li>
					<a href="/">Legal</a>
				</li>
			</ul>
		</div>
	</div>
</div>
<form id="form1" runat="server">
	<div class="container body-content">
		<h2>Legal Request Export.</h2>
		<asp:Panel ID="Error" CssClass="message error" Visible="False" runat="server">
			<asp:Literal ID="ErrorMessage" runat="server" ></asp:Literal>
		</asp:Panel>
		<asp:Panel ID="Warn" CssClass="message warn" Visible="False" runat="server">
			<asp:Literal ID="WarningMessage" runat="server" ></asp:Literal>
		</asp:Panel>
		<asp:Panel ID="Success" CssClass="message success" Visible="False" runat="server">
			<asp:Literal ID="SuccessMEssage" runat="server" ></asp:Literal>
		</asp:Panel>
		<div class="row">
			<div class="col-lg-12">

				<asp:ScriptManager runat="server"></asp:ScriptManager>
				<section id="loginForm">
					<h4>Complete the form to export the documents for the Legal Request.</h4>
					<hr/>
					<div class="form-group">
						<label class="col-lg-2 control-label" for="SubscriberId">Subscriber Id</label>
						<div class="col-lg-10">
							<asp:TextBox
								ID="SubscriberId"
								name="SubscriberId"
								CssClass="form-control"
								type=""
								data-val="true"
								data-val-email="The Subscriber Id field is not valid, shluld be 9 characters."
								data-val-required="The Subscriber Id field is required."
								runat="server">
							</asp:TextBox>
							<span
								class="field-validation-valid text-danger"
								data-valmsg-for="SubscriberId"
								data-valmsg-replace="true">
							</span>
						</div>
					</div>
					<br/>
					<div class="form-group">
						<label class="col-lg-2 control-label" for="StartDate">Start Date</label>
						<div class="col-lg-10">
							<asp:TextBox
								ID="StartDate"
								CssClass="form-control"
								name="StartDate"
								type="date"
								data-val="true"
								data-val-required="The Start Date field is required."
								runat="server">
							</asp:TextBox>
							<span
								class="field-validation-valid text-danger"
								data-valmsg-for="StartDate"
								data-valmsg-replace="true">
							</span>
						</div>
					</div>
					<br/>
					<div class="form-group">
						<label class="col-lg-2 control-label" for="EndDate">End Date</label>
						<div class="col-lg-10">
							<asp:TextBox
								ID="EndDate"
								name="EndDate"
								type="date"
								CssClass="form-control"
								data-val="true"
								data-val-required="The End Date field is required."
								runat="server">
							</asp:TextBox>
							<span
								class="field-validation-valid text-danger"
								data-valmsg-for="EndDate"
								data-valmsg-replace="true">
							</span>
						</div>
					</div>
					<br/>
					<div class="form-group">
						<label class="col-lg-2 control-label" for="Directory">Directory</label>
						<div class="col-lg-10">
							<asp:DropDownList
								ID="Directory"
								name="Directory"
								runat="server"
								data-val="true"
								data-val-required="The directory field is required."
								CssClass="form-control">
							</asp:DropDownList>
							<span class="field-validation-valid text-danger"
							      data-valmsg-for="Directory"
							      data-valmsg-replace="true">
							</span>
						</div>
					</div>
					&nbsp;
					<div class="form-group">
						<div class="col-lg-offset-2 col-lg-10">
							<asp:Button
								ID="Search"
								runat="server"
								Text="Search Documents"
								CssClass="btn btn-default"
								EnableViewState="True"
								OnClientClick="javascript:Processing()"
								OnClick="Search_Click"/>
							<asp:Button
								ID="Export"
								runat="server"
								Text="Export Documents"
								CssClass="btn btn-default"
								EnableViewState="True"
								Enabled="False"
								OnClientClick="javascript:Processing()"
								OnClick="Export_Click"/>
						</div>
					</div>
				</section>
				<hr/>
				<section id="SearchResults">
					<div class="row">
						<div class="col-lg-12">
							<asp:DataList ID="ResultList" runat="server" CellPadding="4" ForeColor="#333333" Width="100%">
								<AlternatingItemStyle BackColor="White" ForeColor="#284775"/>
								<FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White"/>
								<HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" Width="100%"/>
								<HeaderTemplate>
									<div class="form-group col-lg-12">
										<h4>
											<label class="col-lg-3 control-label">Total Documents</label>
											<div class="col-lg-09">
												<%# TotalRecords %>
											</div>
										</h4>
									</div>
									<div class="form-group">
										<span class="col-lg-1">
											Id
										</span>
										<span class="col-lg-3">
											Date
										</span>
										<span class="col-lg-3">
											Type
										</span>
										<span class="col-lg-5">
											Name
										</span>
									</div>
								</HeaderTemplate>
								<ItemStyle BackColor="#F7F6F3" ForeColor="#333333" Width="100%"/>
								<ItemTemplate>
									<div class="form-group">
										<span class="col-lg-1">
											<%# Eval("DocumentId") %>
										</span>
										<span class="col-lg-3">
											<%# Eval("DocumentDate") %>
										</span>
										<span class="col-lg-3">
											<%# Eval("DocumentType") %>
										</span>
										<span class="col-lg-5">
											<%# Eval("DocumentName") %>
										</span>
									</div>
								</ItemTemplate>
								<SelectedItemStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333"/>
							</asp:DataList>
						</div>
					</div>
				</section>

			</div>

		</div>


		<hr/>
		<footer>
			<p>&copy; 2023 - CareSource</p>
		</footer>
		<div class="overlay">
			<h2 style="text-align: center; vert-align: middle;">Processing</h2>
			<img src="Content/processing.gif"/>
		</div>
	</div>
</form>
<script src="Scripts/jquery-3.4.1.js"></script>

<script src="Scripts/bootstrap.js"></script>


<!-- Visual Studio Browser Link -->
<script type="text/javascript"
        src="https://localhost:44398/29a7113737364bef9bcf807841a88f81/browserLink"
        async="async"
        id="__browserLink_initializationData"
        data-requestId="7fd6faa597c949da9dc0082627a9c143"
        data-appName="Microsoft Edge">
</script>
<script type="text/javascript">
function Processing()
	{
	$("body").addClass("loading");
	return false;
	}

</script>
<!-- End Browser Link -->
</body>
</html>