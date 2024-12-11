using System;
using System.Collections.Generic;
using Xunit;
using WPR_project.Models;
using WPR_project.Services;
using Moq;
using WPR_project.Repositories;
using WPR_project.Services.Email;

namespace WPR_project.TemporaryTests
{
    public class ParticulierHuurderTests
    {
        private readonly Mock<IHuurderRegistratieRepository> _mockRepository;
        private readonly Mock<IEmailService> _mockEmailService;
        private readonly ParticulierHuurderService _service;

        public ParticulierHuurderTests()
        {
            _mockRepository = new Mock<IHuurderRegistratieRepository>();
            _mockEmailService = new Mock<IEmailService>();
            _service = new ParticulierHuurderService(_mockRepository.Object, _mockEmailService.Object);
        }

        [Fact]
        public void Register_ValidHuurder_ShouldRegisterAndSendEmail()
        {
            // Arrange
            var huurder = new ParticulierHuurder
            {
                particulierEmail = "test@gmail.com",
                particulierNaam = "John Doe",
                wachtwoord = "Password123!",
                adress = "Weimarstraat 24b",
                postcode = "1234AB",
                woonplaats = "Amsterdam",
                telefoonnummer = "+31612345678"
            };

            // Act
            _service.Register(huurder);

            // Assert
            _mockRepository.Verify(r => r.Add(It.IsAny<ParticulierHuurder>()), Times.Once);
            _mockRepository.Verify(r => r.Save(), Times.Once);
            _mockEmailService.Verify(e => e.SendEmail(
                "test@gmail.com",
                "Bevestig je registratie",
                It.Is<string>(body => body.Contains("Beste John Doe"))
            ), Times.Once);
        }

        [Fact]
        public void Register_InvalidEmail_ShouldThrowException()
        {
            // Arrange
            var huurder = new ParticulierHuurder
            {
                particulierEmail = "invalid-bedrijsEmail", // Ongeldig e-mailadres
                particulierNaam = "John Doe",
                wachtwoord = "Password123!",
                adress = "Weimarstraat 24b",
                postcode = "1234AB",
                woonplaats = "Amsterdam",
                telefoonnummer = "+31612345678"
            };

            // Act
            var ex = Assert.Throws<ArgumentException>(() => _service.Register(huurder));

            // Assert
            Assert.Contains("Ongeldig e-mailadres.", ex.Message);
        }

        [Fact]
        public void Register_InvalidPassword_ShouldThrowException()
        {
            // Arrange
            var huurder = new ParticulierHuurder
            {
                particulierEmail = "test@gmail.com",
                particulierNaam = "John Doe",
                wachtwoord = "123", //Ongeldig wachtwoord
                adress = "Weimarstraat 24b",
                postcode = "1234AB",
                woonplaats = "Amsterdam",
                telefoonnummer = "+31612345678"
            };

            // Act
            var ex = Assert.Throws<ArgumentException>(() => _service.Register(huurder));

            // Assert
            Assert.Contains("Wachtwoord moet minimaal 8 tekens bevatten.", ex.Message);
        }

        [Fact]
        public void Register_MissingRequiredFields_ShouldThrowException()
        {
            // Arrange
            var huurder = new ParticulierHuurder
            {
                particulierEmail = "", //leeg e-mailadres
                particulierNaam = "John Doe",
                wachtwoord = "Password123!",
                adress = "Weimarstraat 24b",
                postcode = "1234AB",
                woonplaats = "Amsterdam",
                telefoonnummer = "+31612345678"
            };

            // Act
            var ex = Assert.Throws<ArgumentException>(() => _service.Register(huurder));

            // Assert
            Assert.Contains("E-mailadres is verplicht.", ex.Message);
        }

        [Fact]
        public void Register_ValidationsFail_ShouldReturnCorrectErrorMessage()
        {
            // Arrange
            var huurder = new ParticulierHuurder
            {
                particulierEmail = "test@gmail.com",
                particulierNaam = "", // lege naam
                wachtwoord = "Password123!",
                adress = "Weimarstraat 24b",
                postcode = "1234AB",
                woonplaats = "Amsterdam",
                telefoonnummer = "+31612345678"
            };

            // Act
            var ex = Assert.Throws<ArgumentException>(() => _service.Register(huurder));

            // Assert
            Assert.Contains("Naam is verplicht.", ex.Message);
        }
    }
}
