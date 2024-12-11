//using System;
//using System.Collections.Generic;
//using System.Linq;
//using Xunit;
//using WPR_project.Models;
//using WPR_project.Services;
//using Moq;
//using WPR_project.Repositories;

//namespace WPR_project.Tests
//{
//    public class WagenparkBeheerderTests
//    {
//        private readonly Mock<IWagenparkBeheerderRepository> _mockRepository;
//        private readonly WagenparkBeheerderService _service;

//        public WagenparkBeheerderTests()
//        {
//            _mockRepository = new Mock<IWagenparkBeheerderRepository>();
//            _service = new WagenparkBeheerderService(_mockRepository.Object);
//        }

//        [Fact]
//        public void VoegMedewerkerToe_ValidMedewerker_ShouldAddMedewerker()
//        {
//            // Arrange
//            var beheerderId = Guid.NewGuid();
//            var beheerder = new WagenparkBeheerder
//            {
//                beheerderId = beheerderId,
//                beheerderNaam = "Jan Janssen",
//                bedrijfsEmail = "jan.janssen@bedrijf.nl",
//                telefoonNummer = "+31612345678",
//                MedewerkerLijst = new List<BedrijfsMedewerkers>()
//            };
//            var medewerker = new BedrijfsMedewerkers
//            {
//                BedrijfsMedewerkId = 1,
//                medewerkerNaam = "Piet Janssen",
//                medewerkerEmail = "piet.janssen@bedrijf.nl",
//                Wachtwoord = "Password123!"
//            };

//            _mockRepository.Setup(r => r.getBeheerderById(beheerderId)).Returns(beheerder);

//            // Act
//            _service.VoegMedewerkerToe(beheerderId, medewerker);

//            // Assert
//            Assert.Contains(medewerker, beheerder.MedewerkerLijst);
//            _mockRepository.Verify(r => r.UpdateWagenparkBeheerder(beheerder), Times.Once);
//            _mockRepository.Verify(r => r.Save(), Times.Once);
//        }

//        [Fact]
//        public void VoegMedewerkerToe_DuplicateMedewerker_ShouldThrowException()
//        {
//            // Arrange
//            var beheerderId = Guid.NewGuid();
//            var beheerder = new WagenparkBeheerder
//            {
//                beheerderId = beheerderId,
//                MedewerkerLijst = new List<BedrijfsMedewerkers>
//                {
//                    new BedrijfsMedewerkers { BedrijfsMedewerkId = 1, medewerkerEmail = "piet.janssen@bedrijf.nl" }
//                }
//            };
//            var medewerker = new BedrijfsMedewerkers
//            {
//                BedrijfsMedewerkId = 1,
//                medewerkerEmail = "piet.janssen@bedrijf.nl"
//            };

//            _mockRepository.Setup(r => r.getBeheerderById(beheerderId)).Returns(beheerder);

//            // Act & Assert
//            var ex = Assert.Throws<InvalidOperationException>(() => _service.VoegMedewerkerToe(beheerderId, medewerker));
//            Assert.Equal("Medewerker is al gekoppeld aan deze beheerder.", ex.Message);
//        }

//        [Fact]
//        public void VerwijderMedewerker_ValidMedewerker_ShouldRemoveMedewerker()
//        {
//            // Arrange
//            var beheerderId = Guid.NewGuid();
//            var medewerkerId = 1;
//            var beheerder = new WagenparkBeheerder
//            {
//                beheerderId = beheerderId,
//                MedewerkerLijst = new List<BedrijfsMedewerkers>
//                {
//                    new BedrijfsMedewerkers { BedrijfsMedewerkId = medewerkerId, medewerkerEmail = "piet.janssen@bedrijf.nl" }
//                }
//            };

//            _mockRepository.Setup(r => r.getBeheerderById(beheerderId)).Returns(beheerder);

//            // Act
//            _service.VerwijderMedewerker(beheerderId, medewerkerId);

//            // Assert
//            Assert.Empty(beheerder.MedewerkerLijst);
//            _mockRepository.Verify(r => r.UpdateWagenparkBeheerder(beheerder), Times.Once);
//            _mockRepository.Verify(r => r.Save(), Times.Once);
//        }

//        [Fact]
//        public void VerwijderMedewerker_NonExistentMedewerker_ShouldThrowException()
//        {
//            // Arrange
//            var beheerderId = Guid.NewGuid();
//            var medewerkerId = 99;
//            var beheerder = new WagenparkBeheerder
//            {
//                beheerderId = beheerderId,
//                MedewerkerLijst = new List<BedrijfsMedewerkers>
//                {
//                    new BedrijfsMedewerkers { BedrijfsMedewerkId = 1, medewerkerEmail = "piet.janssen@bedrijf.nl" }
//                }
//            };

//            _mockRepository.Setup(r => r.getBeheerderById(beheerderId)).Returns(beheerder);

//            // Act & Assert
//            var ex = Assert.Throws<KeyNotFoundException>(() => _service.VerwijderMedewerker(beheerderId, medewerkerId));
//            Assert.Equal("Medewerker niet gevonden.", ex.Message);
//        }

//        [Fact]
//        public void VerwijderMedewerker_NonExistentBeheerder_ShouldThrowException()
//        {
//            // Arrange
//            var beheerderId = Guid.NewGuid();
//            var medewerkerId = 1;

//            _mockRepository.Setup(r => r.getBeheerderById(beheerderId)).Returns((WagenparkBeheerder)null);

//            // Act & Assert
//            var ex = Assert.Throws<KeyNotFoundException>(() => _service.VerwijderMedewerker(beheerderId, medewerkerId));
//            Assert.Equal("Beheerder niet gevonden.", ex.Message);
//        }
//    }
//}
