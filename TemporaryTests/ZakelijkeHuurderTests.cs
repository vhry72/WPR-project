using System;
using System.Collections.Generic;
using Xunit;
using WPR_project.Models;
using WPR_project.Services;
using Moq;
using WPR_project.Repositories;
using WPR_project.Services.Email;
using System.Linq;

namespace WPR_project.Tests
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
                bedrijsEmail = "testbedrijf@bedrijf.nl",
                KVKNummer = 12345678,
                wachtwoord = "Password123!",
                telNummer = "+31612345678",
                EmailBevestigingToken = Guid.NewGuid().ToString()
            };

            // Act
            _service.RegisterZakelijkeHuurder(huurder);

            // Assert
            _mockRepository.Verify(r => r.AddZakelijkHuurder(It.IsAny<ZakelijkHuurder>()), Times.Once);
            _mockRepository.Verify(r => r.Save(), Times.Once);
            _mockEmailService.Verify(e => e.SendEmail(
                huurder.bedrijsEmail,
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
                bedrijsEmail = "ongeldigemail", // Ongeldig e-mailadres
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

        /// <summary>
        /// Test voor succesvol toevoegen van een medewerker aan een zakelijke huurder.
        /// </summary>
        [Fact]
        public void VoegMedewerkerToe_ValidData_ShouldAddMedewerkerAndSendEmail()
        {
            // Arrange
            var huurderId = Guid.NewGuid();
            var huurder = new ZakelijkHuurder
            {
                zakelijkeId = huurderId,
                bedrijfsNaam = "Test Bedrijf",
                Medewerkers = new List<BedrijfsMedewerkers>()
            };

            var medewerkerEmail = "medewerker@bedrijf.nl";
            var medewerkerNaam = "John Doe";

            _mockRepository.Setup(r => r.GetZakelijkHuurderById(huurderId)).Returns(huurder);

            // Act
            _service.VoegMedewerkerToe(huurderId, medewerkerNaam, medewerkerEmail);

            // Assert
            Assert.Single(huurder.Medewerkers);
            Assert.Equal(medewerkerEmail, huurder.Medewerkers.First().medewerkerEmail);
            _mockRepository.Verify(r => r.UpdateZakelijkHuurder(huurder), Times.Once);
            _mockRepository.Verify(r => r.Save(), Times.Once);
            _mockEmailService.Verify(e => e.SendEmail(
                medewerkerEmail,
                "Medewerker Toegevoegd",
                It.Is<string>(body => body.Contains("U bent toegevoegd aan het bedrijfsaccount van Test Bedrijf"))
            ), Times.Once);
        }

        /// <summary>
        /// Test voor het verwijderen van een medewerker uit een zakelijke huurder.
        /// </summary>
        [Fact]
        public void VerwijderMedewerker_ValidEmail_ShouldRemoveMedewerkerAndSendEmail()
        {
            // Arrange
            var huurderId = Guid.NewGuid();
            var medewerkerEmail = "medewerker@bedrijf.nl";

            var medewerker = new BedrijfsMedewerkers
            {
                bedrijfsMedewerkerId = Guid.NewGuid(),
                medewerkerNaam = "John Doe",
                medewerkerEmail = medewerkerEmail
            };

            var huurder = new ZakelijkHuurder
            {
                zakelijkeId = huurderId,
                bedrijfsNaam = "Test Bedrijf",
                Medewerkers = new List<BedrijfsMedewerkers> { medewerker }
            };

            _mockRepository.Setup(r => r.GetZakelijkHuurderById(huurderId)).Returns(huurder);

            // Act
            _service.VerwijderMedewerker(huurderId, medewerkerEmail);

            // Assert
            Assert.Empty(huurder.Medewerkers);
            _mockRepository.Verify(r => r.UpdateZakelijkHuurder(huurder), Times.Once);
            _mockRepository.Verify(r => r.Save(), Times.Once);
            _mockEmailService.Verify(e => e.SendEmail(
                medewerkerEmail,
                "Medewerker Verwijderd",
                It.Is<string>(body => body.Contains("U bent verwijderd uit het bedrijfsaccount van Test Bedrijf"))
            ), Times.Once);
        }

        /// <summary>
        /// Test voor het verwijderen van een medewerker die niet bestaat.
        /// </summary>
        [Fact]
        public void VerwijderMedewerker_MedewerkerNotFound_ShouldThrowException()
        {
            // Arrange
            var huurderId = Guid.NewGuid();
            var medewerkerEmail = "nietbestaand@bedrijf.nl";

            var huurder = new ZakelijkHuurder
            {
                zakelijkeId = huurderId,
                bedrijfsNaam = "Test Bedrijf",
                Medewerkers = new List<BedrijfsMedewerkers>()
            };

            _mockRepository.Setup(r => r.GetZakelijkHuurderById(huurderId)).Returns(huurder);

            // Act & Assert
            var exception = Assert.Throws<InvalidOperationException>(() =>
                _service.VerwijderMedewerker(huurderId, medewerkerEmail)
            );

            Assert.Contains("Medewerker niet gevonden.", exception.Message);
            _mockRepository.Verify(r => r.UpdateZakelijkHuurder(It.IsAny<ZakelijkHuurder>()), Times.Never);
            _mockRepository.Verify(r => r.Save(), Times.Never);
            _mockEmailService.Verify(e => e.SendEmail(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        }
    }
}
