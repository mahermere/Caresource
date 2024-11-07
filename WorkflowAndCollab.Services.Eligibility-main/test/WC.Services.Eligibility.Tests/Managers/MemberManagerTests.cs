using CareSource.WC.Entities.Common;
using CareSource.WC.Entities.Members;
using CareSource.WC.Services.Eligibility.Adapters.Interfaces;
using CareSource.WC.Services.Eligibility.Managers;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Shouldly;
using System;

namespace CareSource.WC.Services.Eligibility.Tests.Managers
{
    [TestClass]
    public class MemberManagerTests
    {
        private readonly Mock<IMemberAdapter<Member>> _mockMemberAdapter;

        private readonly MemberManager _memberManager;

        public MemberManagerTests()
        {
            _mockMemberAdapter = new Mock<IMemberAdapter<Member>>();

            _memberManager = new MemberManager(_mockMemberAdapter.Object);
        }

        [TestMethod]
        public void ValidateMemberId_HappyPath()
        {
            //Arrange
            var memberId = "10410410400";
            var modelState = new ModelStateDictionary();

            //Act
            Exception exception = null;
            bool result = true;
            try
            {
                result = _memberManager.ValidateMemberId(memberId, modelState);
            }
            catch (Exception ex)
            {
                exception = ex;
            }

            //Assert
            exception.ShouldBeNull();
            result.ShouldBe(true);
        }

        [TestMethod]
        public void ValidateMemberId_NullModelState()
        {
            //Arrange
            var memberId = "10410410400";
            ModelStateDictionary modelState = null;

            //Act
            Exception exception = null;
            bool result = true;
            try
            {
                result = _memberManager.ValidateMemberId(memberId, modelState);
            }
            catch (Exception ex)
            {
                exception = ex;
            }

            //Assert
            exception.ShouldBeNull();
            result.ShouldBe(true);
        }

        [TestMethod]
        public void ValidateMemberId_NullMemberId()
        {
            //Arrange
            var modelState = new ModelStateDictionary();

            //Act
            Exception exception = null;
            bool result = true;
            try
            {
                result = _memberManager.ValidateMemberId(null, modelState);
            }
            catch (Exception ex)
            {
                exception = ex;
            }

            //Assert
            exception.ShouldBeNull();
            result.ShouldBe(false);
        }

        [TestMethod]
        public void ValidateMemberId_LessThan11DigitMemberId()
        {
            //Arrange
            var memberId = "104104104";
            var modelState = new ModelStateDictionary();

            //Act
            Exception exception = null;
            bool result = true;
            try
            {
                result = _memberManager.ValidateMemberId(memberId, modelState);
            }
            catch (Exception ex)
            {
                exception = ex;
            }

            //Assert
            exception.ShouldBeNull();
            result.ShouldBe(false);
        }

        [TestMethod]
        public void ValidateMemberId_NonNumericMemberId()
        {
            //Arrange
            var memberId = "104104104a0";
            var modelState = new ModelStateDictionary();

            //Act
            Exception exception = null;
            bool result = true;
            try
            {
                result = _memberManager.ValidateMemberId(memberId, modelState);
            }
            catch (Exception ex)
            {
                exception = ex;
            }

            //Assert
            exception.ShouldBeNull();
            result.ShouldBe(false);
        }

        [TestMethod]
        public void ValidateMemberId_GreaterThan11MemberId()
        {
            //Arrange
            var memberId = "104104104000";
            var modelState = new ModelStateDictionary();

            //Act
            Exception exception = null;
            bool result = true;
            try
            {
                result = _memberManager.ValidateMemberId(memberId, modelState);
            }
            catch (Exception ex)
            {
                exception = ex;
            }

            //Assert
            exception.ShouldBeNull();
            result.ShouldBe(false);
        }

        [TestMethod]
        public void GetMember_HappyPath()
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

            _mockMemberAdapter
                .Setup(ma => ma.GetMemberBySubscriberId(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(member);

            //Act
            Exception exception = null;
            Member result = null;
            try
            {
                result = _memberManager.GetMember(memberId);
            }
            catch (Exception ex)
            {
                exception = ex;
            }

            //Assert
            exception.ShouldBeNull();
            result.ShouldNotBeNull();
        }
    }
}