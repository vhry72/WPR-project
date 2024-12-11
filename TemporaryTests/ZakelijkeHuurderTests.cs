//using System;
//using System.Collections.Generic;
//using Xunit;
//using WPR_project.Models;
//using WPR_project.Services;
//using Moq;
//using WPR_project.Repositories;
//using WPR_project.Services.Email;

//namespace WPR_project.TemporaryTests
//{
//    public class ZakelijkeHuurderTests
//    {
//        private readonly Mock<IZakelijkeHuurderRepository> _mockRepository;
//        private readonly Mock<IEmailService> _mockEmailService;
//        private readonly ZakelijkeHuurderService _service;

//        public ZakelijkeHuurderTests()
//        {
//            _mockRepository = new Mock<IZakelijkeHuurderRepository>();
//            _mockEmailService = new Mock<IEmailService>();
//            _service = new ZakelijkeHuurderService(_mockRepository.Object, _mockEmailService.Object);
//        }

//        [Fact]
//        public void RegisterZakelijkeHuurder_GeldigeHuurder_ShouldRegisterAndSendEmail()
//        {
//            // Arrange
//            var huurder = new ZakelijkHuurder
//            {
//                bedrijfsNaam = "Test Bedrijf",
//                adres = "Weimarstraat 24b",
//                bedrijsEmail = "testbedrijf@gmail.com",
//                KVKNummer = 12345678,
//                wachtwoord = "Password123!",
//                telNummer = "+31612345678"
//            };

//            // Act
//            _service.RegisterZakelijkeHuurder(huurder);

//            // Assert
//            _mockRepository.Verify(r => r.AddZakelijkHuurder(It.IsAny<ZakelijkHuurder>()), Times.Once);
//            _mockRepository.Verify(r => r.Save(), Times.Once);
//            _mockEmailService.Verify(e => e.SendEmail(
//                "testbedrijf@gmail.com",
//                "Bevestig je registratie",
//                It.Is<string>(body => body.Contains("Beste Test Bedrijf"))
//            ), Times.Once);
//        }

//        [Fact]
//        public void RegisterZakelijkeHuurder_OngeldigEmail_ShouldThrowException()
//        {
//            // Arrange
//            var huurder = new ZakelijkHuurder
//            {
//                bedrijfsNaam = "Test Bedrijf",
//                adres = "Weimarstraat 24b",
//                bedrijsEmail = "invalid-bedrijsEmail", // Ongeldig e-mailadres
//                KVKNummer = 12345678,
//                wachtwoord = "Password123!",
//                telNummer = "+31612345678"
//            };

//            // Act
//            var ex = Assert.Throws<ArgumentException>(() => _service.RegisterZakelijkeHuurder(huurder));

//            // Assert
//            Assert.Contains("Ongeldig e-mailadres.", ex.Message);
//        }

//        [Fact]
//        public void RegisterZakelijkeHuurder_OngeldigKVK_ShouldThrowException()
//        {
//            // Arrange
//            var huurder = new ZakelijkHuurder
//            {
//                bedrijfsNaam = "Test Bedrijf",
//                adres = "Weimarstraat 24b",
//                bedrijsEmail = "testbedrijf@gmail.com",
//                KVKNummer = 123, // Ongeldig KVK-nummer
//                wachtwoord = "Password123!",
//                telNummer = "+31612345678"
//            };

//            // Act
//            var ex = Assert.Throws<ArgumentException>(() => _service.RegisterZakelijkeHuurder(huurder));

//            // Assert
//            Assert.Contains("KVK-nummer moet een 8-cijferig getal zijn.", ex.Message);
//        }

//        [Fact]
//        public void RegisterZakelijkeHuurder_OngeldigPassword_ShouldThrowException()
//        {
//            // Arrange
//            var huurder = new ZakelijkHuurder
//            {
//                bedrijfsNaam = "Test Bedrijf",
//                adres = "Weimarstraat 24b",
//                bedrijsEmail = "testbedrijf@gmail.com",
//                KVKNummer = 12345678,
//                wachtwoord = "123", // Ongeldig wachtwoord
//                telNummer = "+31612345678"
//            };

//            // Act
//            var ex = Assert.Throws<ArgumentException>(() => _service.RegisterZakelijkeHuurder(huurder));

//            // Assert
//            Assert.Contains("Wachtwoord moet minimaal 8 tekens bevatten.", ex.Message);
//        }

//        [Fact]
//        public void RegisterZakelijkeHuurder_Legevelden_IsThrowException()
//        {
//            // Arrange
//            var huurder = new ZakelijkHuurder
//            {
//                bedrijfsNaam = "", // Lege bedrijfsnaam
//                adres = "Weimarstraat 24b",
//                bedrijsEmail = "testbedrijf@gmail.com",
//                KVKNummer = 12345678,
//                wachtwoord = "Password123!",
//                telNummer = "+31612345678"
//            };

//            // Act
//            var ex = Assert.Throws<ArgumentException>(() => _service.RegisterZakelijkeHuurder(huurder));

//            // Assert
//            Assert.Contains("Bedrijfsnaam is verplicht.", ex.Message);
//        }

//        [Fact]
//        public void VoegMedewerkerToe_Is_ShouldAddMedewerker()
//        {
//            // Arrange
//            var huurder = new ZakelijkHuurder
//            {
//                zakelijkeId = Guid.NewGuid(),
//                bedrijfsNaam = "Test Bedrijf",
//                Medewerkers = new List<BedrijfsMedewerkers>()
//            };
//            var medewerkerEmail = "medewerker@testbedrijf.com";

//            _mockRepository.Setup(r => r.GetZakelijkHuurderById(huurder.zakelijkeId)).Returns(huurder);

//            // Act
//            _service.VoegMedewerkerToe(huurder.zakelijkeId, medewerkerEmail);

//            // Assert
//            Assert.Contains(medewerkerEmail, huurder.MedewerkersEmails);
//            _mockRepository.Verify(r => r.UpdateZakelijkHuurder(huurder), Times.Once);
//            _mockRepository.Verify(r => r.Save(), Times.Once);
//            _mockEmailService.Verify(e => e.SendEmail(
//                medewerkerEmail,
//                "Medewerker Toegevoegd",
//                It.Is<string>(body => body.Contains("U bent toegevoegd aan het bedrijfsaccount van Test Bedrijf."))
//            ), Times.Once);
//        }

//        [Fact]
//        public void VerwijderMedewerker_Is_ShouldRemoveMedewerker()
//        {
//            // Arrange
//            var huurder = new ZakelijkHuurder
//            {
//                zakelijkeId = Guid.NewGuid(),
//                bedrijfsNaam = "Test Bedrijf",
//                MedewerkersEmails = new List<string> { "medewerker@testbedrijf.com" }
//            };
//            var medewerkerEmail = "medewerker@testbedrijf.com";

//            _mockRepository.Setup(r => r.GetZakelijkHuurderById(huurder.zakelijkeId)).Returns(huurder);

//            // Act
//            _service.VerwijderMedewerker(huurder.zakelijkeId, medewerkerEmail);

//            // Assert
//            Assert.DoesNotContain(medewerkerEmail, huurder.MedewerkersEmails);
//            _mockRepository.Verify(r => r.UpdateZakelijkHuurder(huurder), Times.Once);
//            _mockRepository.Verify(r => r.Save(), Times.Once);
//            _mockEmailService.Verify(e => e.SendEmail(
//                medewerkerEmail,
//                "Medewerker Verwijderd",
//                It.Is<string>(body => body.Contains("U bent verwijderd uit het bedrijfsaccount van Test Bedrijf."))
//            ), Times.Once);
//        }
//    }
//}
