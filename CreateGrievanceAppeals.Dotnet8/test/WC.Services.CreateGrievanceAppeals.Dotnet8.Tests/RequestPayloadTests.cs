using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using WC.Services.CreateGrievanceAppeals.Dotnet8.Models.Requests;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace WC.Services.CreateGrievanceAppeals.Dotnet8.Tests.Models.Requests
{
    [TestClass]
    public class RequestPayloadTests
    {
        [TestMethod]
        public void RequestPayload_Creation_Succeeds()
        {
            // Arrange
            var transactionData = new TransactionHeader();
            transactionData.CorrelationId = "1";
            transactionData.DocumentCount = "1";
            transactionData.ESBGUID = "1";
            transactionData.IncludeDocuments = "true";
            transactionData.Notify="true";
            transactionData.NotifyList = "true";
            transactionData.NotifyType = "SMS";
            transactionData.SourceAppName = "AppName";
            transactionData.SourceAppId = "1";
            transactionData.TransactionType = "Grievance";
            transactionData.UserId = "1";
            KeywordValue keywordValue1 = new KeywordValue(){
                InputName = "Info1",
                Value = "Additionalinfo"
            };
            KeywordValue[] additionalInput2 = new KeywordValue[]{
                keywordValue1
            };
            var payload = new GrievanceAppealSubmission();
            payload.ApplicationName = "SL";
            payload.AdminCat1 = "admin1";
            payload.AdminCat2 = "admin2";
            payload.AOR = "true";
            payload.AORExpires = "true";
            payload.AORRepresentativeCity = "city";
            payload.AORRepresentativeEmail = "email";
            payload.AORRepresentativeFirst="fname";
            payload.AORRepresentativeLast="lname";
            payload.AORRepresentativePhone="phone";
            payload.AORRepresentativeState="state";
            payload.AORRepresentativeStreet="street";
            payload.AORRepresentativeZIP="zip";
            payload.AppealType = "AppealType";
            payload.AutoClose = "true";
            payload.BeginDOS = "true";
            payload.CallerId = "1";
            payload.CallerStatus = "valid";
            payload.CallInMethod = "true";
            payload.CallType = "Inquiry";
            payload.Category = "Category";
            payload.CategoryCode = "1";
            payload.ClaimId = "1";
            payload.Complaint = "Complaint";
            payload.CurrentUserName = "user1";
            payload.CustomerType = "custType";
            payload.DateComplete = "12/11/2024";
            payload.DateOfService = "12/10/2024";
            payload.DateReceived = "12/10/2024";
            payload.Description = "Desc";
            payload.DisputeId = "1";
            payload.DisputeType = "DisputeType";
            payload.AdditionalInput = additionalInput2;
            payload.EndDOS = "true";
            payload.EnrolleeName = "EnrolleeName";
            payload.EnrollmentEndDate = "12/11/2024";
            payload.EnrollmentStartDate = "12/10/2024";
            payload.FacetsGrievanceId = "1";
            payload.GroupName="group1";
            payload.HasPriorAuth = "true";
            payload.HealthPlan = "HealthPlan";
            payload.HICN = "3";
            payload.InitiationDate = "12/10/2024";
            payload.IsNonPar = "true";
            payload.IsRetroAuth = "true";
            payload.IsSNP = "true";
            payload.IssueCategory = "IssueCategory";
            payload.LinkReason="LinkReason";
            payload.LinkType="LinkType";
            payload.MedicaidId = "1";
            payload.MemberCity = "memberCity";
            payload.MemberContactPhone = "545454";
            payload.MemberDOB = "12/10/2024";
            payload.MemberEMail = "email";
            payload.MemberId = "1";
            payload.MemberFirst = "memberFirstName";
            payload.MemberLast = "memberLastName";
            payload.MemberState = "state";
            payload.MemberStreet = "memberStreet";
            payload.MemberZIP = "memberZIP";
            payload.PlanId = "1";
            payload.PlanName = "planName";
            payload.PlanStatus = "Active";
            payload.Priority = "1";
            payload.ProviderCity = "city";
            payload.ProviderContactName = "pname";
            payload.ProviderEmail = "email";
            payload.ProviderFourCity = "fourcity";
            payload.ProviderFourContactName = "43545";
            payload.ProviderFourEmail = "email";
            payload.ProviderFourId = "4";
            payload.ProviderFourName = "fourname4";
            payload.ProviderFourNPI = "NPI4";
            payload.ProviderFourPhone = "phone4";
            payload.ProviderFourState = "state4";
            payload.ProviderFourStreet = "street4";
            payload.ProviderFourTaxId = "taxID4";
            payload.ProviderFourZIP = "zip4";
            payload.ProviderId = "1";
            payload.ProviderName = "providerName";
            payload.ProviderNPI = "NPI";
            payload.ProviderPhone = "phone";
            payload.ProviderState = "state";
            payload.ProviderStreet = "street";
            payload.ProviderZIP = "zip";
            payload.ProviderThreeCity = "city3";
            payload.ProviderThreeContactName = "name3";
            payload.ProviderThreeEmail = "email3";
            payload.ProviderThreeId = "3";
            payload.ProviderThreeName = "name3";
            payload.ProviderThreeNPI = "NPI3";
            payload.ProviderThreePhone = "phone3";
            payload.ProviderThreeState = "state3";
            payload.ProviderThreeStreet = "street3";
            payload.ProviderThreeTaxId = "taxID3";
            payload.ProviderThreeZIP = "zip3";
            payload.ProviderTwoCity = "city2";
            payload.ProviderTwoContactName = "name2";
            payload.ProviderTwoEmail = "email2";
            payload.ProviderTwoName = "name2";
            payload.ProviderTwoId = "2";
            payload.ProviderTwoNPI = "NPI2";
            payload.ProviderTwoPhone = "phone2";
            payload.ProviderTwoState = "state2";
            payload.ProviderTwoStreet = "street2";
            payload.ProviderTwoZIP =  "zip2";
            payload.ProviderTwoTaxId  = "taxID2";
            payload.Reason = "reason";
            payload.ReceivedDate = "12/10/2024";
            payload.RepresentativeTermDate = "12/10/2024 ";
            payload.Resolution = "resolution";
            payload.Signature = "signature";
            payload.SignatureDate = "12/10/2024";
            payload.Status = "status";
            payload.SubGroup = "subGroup";
            payload.Subject = "subject";
            payload.SubjectCode = "1";
            payload.SubType = "subType";
            payload.Summary = "summary";
            payload.TaxId = "taxID";
            payload.Type = "type";
            
            // Act
            var requestPayload = new RequestPayload<GrievanceAppealSubmission>
            {
                TransactionData = transactionData,
                Payload = payload
            };

            string json = JsonSerializer.Serialize(requestPayload);
            File.WriteAllText("c:/temp/requestPayload.json", json);
            // Assert
            Assert.IsNotNull(requestPayload);
            Assert.AreEqual(transactionData, requestPayload.TransactionData);
            Assert.AreEqual(payload, requestPayload.Payload);
        }

        [TestMethod]
        public void RequestPayload_Serialization_Succeeds()
        {
            // Arrange
            var transactionData = new TransactionHeader();
            var payload = new GrievanceAppealSubmission();
            var requestPayload = new RequestPayload<GrievanceAppealSubmission>
            {
                TransactionData = transactionData,
                Payload = payload
            };

            

            // Act
            var json =  JsonSerializer.Serialize(requestPayload);

            // Assert
            Assert.IsNotNull(json);
            Assert.IsTrue(json.Contains("TransactionData"));
            Assert.IsTrue(json.Contains("Payload"));
        }

        [TestMethod]
        public void RequestPayload_Deserialization_Succeeds()
        {
            // Arrange
            var json = "{\"TransactionData\": {}, \"Payload\": {}}";

            // Act
            var requestPayload = JsonSerializer.Deserialize<RequestPayload<GrievanceAppealSubmission>>(json);

            // Assert
            Assert.IsNotNull(requestPayload);
            Assert.IsNotNull(requestPayload.TransactionData);
            Assert.IsNotNull(requestPayload.Payload);
        }
    }
}