using CareSource.WC.Entities.Common;
using CareSource.WC.Entities.Members;
using CareSource.WC.Services.Eligibility.Adapters.Interfaces;
using CareSource.WC.Services.Eligibility.Managers;
using CareSource.WC.Services.Eligibility.Managers.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Shouldly;
using System;
using System.Collections.Generic;

namespace CareSource.WC.Services.Eligibility.Tests.Managers
{
    [TestClass]
    public class EligibilityManagerTests
    {
        private readonly Mock<IMemberManager<Member>> _mockMemberManager;
        private readonly Mock<IEligibilityAdapter<Entities.Eligibility.Eligibility>> _mockEligibilityAdapter;

        private readonly EligibilityManager _eligibilityManager;

        public EligibilityManagerTests()
        {
            _mockMemberManager = new Mock<IMemberManager<Member>>();
            _mockEligibilityAdapter = new Mock<IEligibilityAdapter<Entities.Eligibility.Eligibility>>();

            _eligibilityManager = new EligibilityManager(_mockMemberManager.Object, _mockEligibilityAdapter.Object);
        }

        [TestMethod]
        public void ValidateMemberId_HappyPath()
        {
            //Arrange
            var memberId = "10410410400";
            var member = new Member()
            {
                ContrivedKey = 1516515616,
                DateOfBirth = new DateTime(2019, 1, 1),
                Email = "fake@fake.com",
                FirstName = "FakeFirstName",
                Hicn = "156161651",
                HomeAddress = new Address
                {
                    Line1 = "15166 Fake St.",
                    Line2 = "Appt. 5151",
                    Line3 = "",
                    City = "Fake City",
                    State = "FK",
                    Zip = "45545"
                },
                LastName = "FakeLastName",
                MedicaidId = "515151515",
                MiddleInitial = "F",
                Phone = "213-456-7890",
                SubscriberId = "104104104",
                Suffix = "00"
            };
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
                .Setup(mm => mm.GetMember(It.IsAny<string>()))
                .Returns(member);
            _mockEligibilityAdapter
                .Setup(ea => ea.GetEligibility(It.IsAny<long?>()))
                .Returns(eligibilities);

            //Act
            Exception exception = null;
            IEnumerable<Entities.Eligibility.Eligibility> result = null;
            try
            {
                result = _eligibilityManager.GetEligibilities(memberId);
            }
            catch (Exception ex)
            {
                exception = ex;
            }

            //Assert
            exception.ShouldBeNull();
            result.ShouldNotBeNull();
            result.ShouldBeSameAs(eligibilities);
        }

        [TestMethod]
        public void ValidateMemberId_FoundMemberIsNull ()
        {
            //Arrange
            var memberId = "10410410400";

            _mockMemberManager
                .Setup(mm => mm.GetMember(It.IsAny<string>()))
                .Returns<Member>(null);

            //Act
            Exception exception = null;
            IEnumerable<Entities.Eligibility.Eligibility> result = null;
            try
            {
                result = _eligibilityManager.GetEligibilities(memberId);
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

        [TestMethod]
        public void ValidateMemberId_FoundMemberContrivedKeyIsNull()
        {
            //Arrange
            var memberId = "10410410400";

            _mockMemberManager
                .Setup(mm => mm.GetMember(It.IsAny<string>()))
                .Returns(new Member());

            //Act
            Exception exception = null;
            IEnumerable<Entities.Eligibility.Eligibility> result = null;
            try
            {
                result = _eligibilityManager.GetEligibilities(memberId);
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