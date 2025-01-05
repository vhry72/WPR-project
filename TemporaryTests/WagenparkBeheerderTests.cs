//using System;
//using System.Collections.Generic;
//using System.Linq;
//using Xunit;
//using WPR_project.Models;
//using WPR_project.Services;
//using Moq;
//using WPR_project.Repositories;
//using WPR_project.Services.Email;

//namespace WPR_project.TemporaryTests
//{
//    public class WagenparkBeheerderTests
//    {
//        private readonly Mock<IWagenparkBeheerderRepository> _mockRepository;
//        private readonly Mock<IZakelijkeHuurderRepository> _mockZakelijkeRepository;
//        private readonly Mock<IAbonnementRepository> _mockAbonnementRepository;
//        private readonly Mock<IEmailService> _mockEmailService;
//        private readonly WagenparkBeheerderService _service;

//        public WagenparkBeheerderTests()
//        {
//            _mockRepository = new Mock<IWagenparkBeheerderRepository>();
//            _mockZakelijkeRepository = new Mock<IZakelijkeHuurderRepository>();
//            _mockAbonnementRepository = new Mock<IAbonnementRepository>();
//            _mockEmailService = new Mock<IEmailService>();

//            _service = new WagenparkBeheerderService(
//                _mockRepository.Object,
//                _mockZakelijkeRepository.Object,
//                _mockAbonnementRepository.Object,
//                null, // DbContext mock is hier niet nodig
//                _mockEmailService.Object
//            );
//        }

//        [Fact]
//        public void VoegMedewerkerToe_ValidMedewerker_ShouldAddMedewerker()
//        {
//            // Arrange
//            var zakelijkeId = Guid.NewGuid();
//            var huurder = new ZakelijkHuurder
//            {
//                zakelijkeId = zakelijkeId,
//                bedrijfsNaam = "Test Bedrijf",
//                Medewerkers = new List<BedrijfsMedewerkers>()
//            };
//            var medewerkerEmail = "piet.janssen@bedrijf.nl";
//            var medewerkerNaam = "Piet Janssen";
//            var wachtwoord = "Password123!";

//            _mockZakelijkeRepository.Setup(r => r.GetZakelijkHuurderById(zakelijkeId)).Returns(huurder);

//            // Act
//            _service.VoegMedewerkerToe(zakelijkeId, medewerkerNaam, medewerkerEmail, wachtwoord);

//            // Assert
//            Assert.Single(huurder.Medewerkers);
//            var toegevoegdMedewerker = huurder.Medewerkers.First();
//            Assert.Equal(medewerkerEmail, toegevoegdMedewerker.medewerkerEmail);
//            Assert.Equal(medewerkerNaam, toegevoegdMedewerker.medewerkerNaam);
//            Assert.Equal(wachtwoord, toegevoegdMedewerker.wachtwoord);

//            _mockZakelijkeRepository.Verify(r => r.UpdateZakelijkHuurder(huurder), Times.Once);
//            _mockZakelijkeRepository.Verify(r => r.Save(), Times.Once);
//        }

//        [Fact]
//        public void VoegMedewerkerToe_DuplicateMedewerker_ShouldThrowException()
//        {
//            // Arrange
//            var zakelijkeId = Guid.NewGuid();
//            var medewerkerEmail = "piet.janssen@bedrijf.nl";
//            var huurder = new ZakelijkHuurder
//            {
//                zakelijkeId = zakelijkeId,
//                Medewerkers = new List<BedrijfsMedewerkers>
//                {
//                    new BedrijfsMedewerkers { medewerkerEmail = medewerkerEmail }
//                }
//            };

//            _mockZakelijkeRepository.Setup(r => r.GetZakelijkHuurderById(zakelijkeId)).Returns(huurder);

//            // Act & Assert
//            var ex = Assert.Throws<InvalidOperationException>(() =>
//                _service.VoegMedewerkerToe(zakelijkeId, "Piet Janssen", medewerkerEmail, "Password123!"));

//            Assert.Equal("Deze medewerker bestaat al.", ex.Message);
//            _mockZakelijkeRepository.Verify(r => r.UpdateZakelijkHuurder(It.IsAny<ZakelijkHuurder>()), Times.Never);
//            _mockZakelijkeRepository.Verify(r => r.Save(), Times.Never);
//        }

//        [Fact]
//        public void VerwijderMedewerker_ValidMedewerker_ShouldRemoveMedewerker()
//        {
//            // Arrange
//            var zakelijkeId = Guid.NewGuid();
//            var medewerkerId = Guid.NewGuid();
//            var medewerker = new BedrijfsMedewerkers
//            {
//                bedrijfsMedewerkerId = medewerkerId,
//                medewerkerNaam = "Piet Janssen",
//                medewerkerEmail = "piet.janssen@bedrijf.nl"
//            };
//            var huurder = new ZakelijkHuurder
//            {
//                zakelijkeId = zakelijkeId,
//                bedrijfsNaam = "Test Bedrijf",
//                Medewerkers = new List<BedrijfsMedewerkers> { medewerker }
//            };

//            _mockZakelijkeRepository.Setup(r => r.GetZakelijkHuurderById(zakelijkeId)).Returns(huurder);

//            // Act
//            _service.VerwijderMedewerker(zakelijkeId, medewerkerId);

//            // Assert
//            Assert.Empty(huurder.Medewerkers);
//            _mockZakelijkeRepository.Verify(r => r.UpdateZakelijkHuurder(huurder), Times.Once);
//            _mockZakelijkeRepository.Verify(r => r.Save(), Times.Once);
//            _mockEmailService.Verify(e => e.SendEmail(
//                medewerker.medewerkerEmail,
//                "Medewerker verwijderd",
//                It.Is<string>(s => s.Contains("U bent verwijderd"))
//            ), Times.Once);
//        }

//        [Fact]
//        public void VerwijderMedewerker_MedewerkerNotFound_ShouldThrowException()
//        {
//            // Arrange
//            var zakelijkeId = Guid.NewGuid();
//            var nietBestaandMedewerkerId = Guid.NewGuid();
//            var huurder = new ZakelijkHuurder
//            {
//                zakelijkeId = zakelijkeId,
//                Medewerkers = new List<BedrijfsMedewerkers>()
//            };

//            _mockZakelijkeRepository.Setup(r => r.GetZakelijkHuurderById(zakelijkeId)).Returns(huurder);

//            // Act & Assert
//            var ex = Assert.Throws<KeyNotFoundException>(() =>
//                _service.VerwijderMedewerker(zakelijkeId, nietBestaandMedewerkerId));

//            Assert.Equal("Medewerker niet gevonden.", ex.Message);
//            _mockZakelijkeRepository.Verify(r => r.UpdateZakelijkHuurder(It.IsAny<ZakelijkHuurder>()), Times.Never);
//            _mockZakelijkeRepository.Verify(r => r.Save(), Times.Never);
//        }

//        [Fact]
//        public void VerwijderMedewerker_ZakelijkeHuurderNotFound_ShouldThrowException()
//        {
//            // Arrange
//            var nietBestaandZakelijkeId = Guid.NewGuid();
//            var medewerkerId = Guid.NewGuid();

//            _mockZakelijkeRepository.Setup(r => r.GetZakelijkHuurderById(nietBestaandZakelijkeId)).Returns((ZakelijkHuurder)null);

//            // Act & Assert
//            var ex = Assert.Throws<KeyNotFoundException>(() =>
//                _service.VerwijderMedewerker(nietBestaandZakelijkeId, medewerkerId));

//            Assert.Equal("Zakelijke huurder niet gevonden.", ex.Message);
//            _mockZakelijkeRepository.Verify(r => r.UpdateZakelijkHuurder(It.IsAny<ZakelijkHuurder>()), Times.Never);
//            _mockZakelijkeRepository.Verify(r => r.Save(), Times.Never);
//        }
//    }
//}
