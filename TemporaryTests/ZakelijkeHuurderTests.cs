using System;
using System.Collections.Generic;
using Xunit;
using WPR_project.Models;
using WPR_project.Services;
using Moq;
using WPR_project.Repositories;
using WPR_project.Services.Email;
using System.Linq;

namespace WPR_project.TemporaryTests
{
    public class ZakelijkeHuurderTests
    {
        private readonly Mock<IZakelijkeHuurderRepository> _mockRepository;
        private readonly Mock<IEmailService> _mockEmailService;
        private readonly ZakelijkeHuurderService _service;

        public ZakelijkeHuurderTests()
        {
            _mockRepository = new Mock<IZakelijkeHuurderRepository>();
            _mockEmailService = new Mock<IEmailService>();
            _service = new ZakelijkeHuurderService(_mockRepository.Object, null, _mockEmailService.Object);
        }

        /// <summary>
        /// Test voor succesvol registreren van een zakelijke huurder.
        /// </summary>
        [Fact]
        public void RegisterZakelijkeHuurder_ValidHuurder_ShouldRegisterAndSendEmail()
        {
            // Arrange
            var huurder = new ZakelijkHuurder
            {
                zakelijkeId = Guid.NewGuid(),
                bedrijfsNaam = "Test Bedrijf",
                adres = "Weimarstraat 24b",
                bedrijfsEmail = "testbedrijf@bedrijf.nl",
                KVKNummer = 12345678,
                wachtwoord = "Password123!",
                telNummer = "+31612345678",
                EmailBevestigingToken = Guid.NewGuid(),
                AspNetUserId = "De Guid komt hier terecht"
            };

            // Act
            _service.RegisterZakelijkeHuurder(huurder);

            // Assert
            _mockRepository.Verify(r => r.AddZakelijkHuurder(It.IsAny<ZakelijkHuurder>()), Times.Once);
            _mockRepository.Verify(r => r.Save(), Times.Once);
            _mockEmailService.Verify(e => e.SendEmail(
                huurder.bedrijfsEmail,
                "Bevestig je registratie",
                It.Is<string>(body => body.Contains("Klik op de volgende link om je e-mailadres te bevestigen"))
            ), Times.Once);
        }

        /// <summary>
        /// Test voor registreren van een zakelijke huurder met ongeldig e-mailadres.
        /// </summary>
        [Fact]
        public void RegisterZakelijkeHuurder_InvalidEmail_ShouldThrowException()
        {
            // Arrange
            var huurder = new ZakelijkHuurder
            {
                bedrijfsNaam = "Test Bedrijf",
                adres = "Weimarstraat 24b",
                bedrijfsEmail = "ongeldigemail", // Ongeldig e-mailadres
                KVKNummer = 12345678,
                wachtwoord = "Password123!",
                telNummer = "+31612345678"
            };

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => _service.RegisterZakelijkeHuurder(huurder));

            Assert.Contains("Validatie mislukt", exception.Message);
            _mockRepository.Verify(r => r.AddZakelijkHuurder(It.IsAny<ZakelijkHuurder>()), Times.Never);
            _mockEmailService.Verify(e => e.SendEmail(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        }
    }
}