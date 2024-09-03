using Microsoft.Extensions.Configuration;
using Moq;
using ShoppingAppAPI.Models;
using ShoppingAppAPI.Services.Classes;
using ShoppingAppAPI.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingAppTest.Service_Tests
{
    public class TokenServicesTest
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void CreateTokenPassTest()
        {
            //Arrange
            Mock<IConfigurationSection> configurationJWTSection = new Mock<IConfigurationSection>();
            configurationJWTSection.Setup(x => x.Value).Returns("This is the dummy key which has to be a bit long for the 512. which should be even more longer for the passing");
            Mock<IConfigurationSection> congigTokenSection = new Mock<IConfigurationSection>();
            congigTokenSection.Setup(x => x.GetSection("JWT")).Returns(configurationJWTSection.Object);
            Mock<IConfiguration> mockConfig = new Mock<IConfiguration>();
            mockConfig.Setup(x => x.GetSection("TokenKey")).Returns(congigTokenSection.Object);
            ITokenServices service = new TokenServices(mockConfig.Object);

            //Action
            var token = service.GenerateToken(new User { UserID = 103, Role = Enums.UserRole.Customer });

            //Assert
            Assert.IsNotNull(token);
        }
    }
}
