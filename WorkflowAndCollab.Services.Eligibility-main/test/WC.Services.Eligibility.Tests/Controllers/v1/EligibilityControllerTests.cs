using CareSource.WC.Entities.Members;
using CareSource.WC.Services.Eligibility.Controllers.v1;
using CareSource.WC.Services.Eligibility.Managers.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Net;

namespace CareSource.WC.Services.Eligibility.Tests.Controllers.v1
{
    [TestClass]
    public class EligibilityControllerTests
    {
        private readonly Mock<IMemberManager<Member>> _mockMemberManager;
        private readonly Mock<IEligibilityManager<Entities.Eligibility.Eligibility>> _mockEligibilityManager;

        private readonly EligibilityController _eligibilityController;

        public EligibilityControllerTests()
        {
            _mockMemberManager = new Mock<IMemberManager<Member>>();
            _mockEligibilityManager = new Mock<IEligibilityManager<Entities.Eligibility.Eligibility>>();

            _eligibilityController = new EligibilityController(_mockMemberManager.Object
                , _mockEligibilityManager.Object);
        }

        [TestMethod]
        public void Get_HappyPath()
        {
            //Arrange
            var memberId = "10410410400";
            var eligibilities = new List<Entities.Eligibility.Eligibility>
            {
                new Entities.Eligibility.Eligibility
                {
                    CategoryDescription = "Dental Product",
                    ContractId = "CSD1",
                    ContrivedKey = 128471150,
                    EffectiveDate = new DateTime(2018,1,1),
                    PlanName = "Fake Plan Name",
                    PolicyStatus = "Not Eligible",
                    TermDate = new DateTime(9999, 12, 31),
                    EligibilityIndicator = "N"
                },
                new Entities.Eligibility.Eligibility
                {
                    CategoryDescription = "Medical Product",
                    ContractId = "CSM1",
                    ContrivedKey = 128471150,
                    EffectiveDate = new DateTime(2018,1,1),
                    PlanName = "Fake Plan Name",
                    PolicyStatus = "Not Eligible",
                    TermDate = new DateTime(9999, 12, 31),
                    EligibilityIndicator = "N"
                }
            };

            _mockMemberManager
                .Setup(mm => mm.ValidateMemberId(It.IsAny<string>(), It.IsAny<ModelStateDictionary>()))
                .Returns(true);
            _mockEligibilityManager
                .Setup(em => em.GetEligibilities(It.IsAny<string>()))
                .Returns(eligibilities);

            //Act
            Exception exception = null;
            IActionResult result = null;
            try
            {
                result = _eligibilityController.Get(memberId);
            }
            catch (Exception ex)
            {
                exception = ex;
            }

            //Assert
            exception.ShouldBeNull();
            result.ShouldNotBeNull();
        }

        [TestMethod]
        public void Get_InvalidMemberId()
        {
            //Arrange
            var memberId = "104104104";

            _mockMemberManager
                .Setup(mm => mm.ValidateMemberId(It.IsAny<string>(), It.IsAny<ModelStateDictionary>()))
                .Returns(false);

            //Act
            Exception exception = null;
            IActionResult result = null;
            try
            {
                result = _eligibilityController.Get(memberId);
            }
            catch (Exception ex)
            {
                exception = ex;
            }

            //Assert
            exception.ShouldBeNull();
            result.ShouldNotBeNull();
            result.ShouldBeOfType<BadRequestObjectResult>();

            var badResult = result as BadRequestObjectResult;
            badResult.StatusCode.ShouldBe((int)HttpStatusCode.BadRequest);
        }

        [TestMethod]
        public void Get_NoEligibilitiesFound()
        {
            //Arrange
            var memberId = "104104104";

            _mockMemberManager
                .Setup(mm => mm.ValidateMemberId(It.IsAny<string>(), It.IsAny<ModelStateDictionary>()))
                .Returns(true);
            _mockEligibilityManager
                .Setup(em => em.GetEligibilities(It.IsAny<string>()))
                .Returns<List<Entities.Eligibility.Eligibility>>(null);

            //Act
            Exception exception = null;
            IActionResult result = null;
            try
            {
                result = _eligibilityController.Get(memberId);
            }
            catch (Exception ex)
            {
                exception = ex;
            }

            //Assert
            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<ArgumentException>();
            result.ShouldBeNull();
        }
    }
}