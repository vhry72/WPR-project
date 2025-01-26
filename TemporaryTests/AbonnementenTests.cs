//using System;
//using System.Collections.Generic;
//using System.Linq;
//using Xunit;
//using Moq;
//using WPR_project.Models;
//using WPR_project.Services;
//using WPR_project.Repositories;
//using WPR_project.Services.Email;
//using Microsoft.AspNetCore.Mvc;
//using WPR_project.DTO_s;
//using WPR_project.Controllers;

//namespace WPR_project.TemporaryTests
//{
//    public class AbonnementenTests
//    {
//        private readonly Mock<IAbonnementRepository> _abonnementRepositoryMock;
//        private readonly Mock<IZakelijkeHuurderRepository> _zakelijkeHuurderRepositoryMock;
//        private readonly Mock<IWagenparkBeheerderRepository> _wagenparkBeheerderRepositoryMock;
//        private readonly Mock<IEmailService> _emailServiceMock;
//        private readonly AbonnementService _abonnementService;

//        public AbonnementenTests()
//        {
//            _abonnementRepositoryMock = new Mock<IAbonnementRepository>();
//            _zakelijkeHuurderRepositoryMock = new Mock<IZakelijkeHuurderRepository>();
//            _wagenparkBeheerderRepositoryMock = new Mock<IWagenparkBeheerderRepository>();
//            _emailServiceMock = new Mock<IEmailService>();

//            // Instantiate the service with the mocked repositories and email service
//            _abonnementService = new AbonnementService(
//                _abonnementRepositoryMock.Object,
//                _zakelijkeHuurderRepositoryMock.Object,
//                _wagenparkBeheerderRepositoryMock.Object,
//                _emailServiceMock.Object
//            );
//        }


//        [Fact]
//        public void VoegMedewerkerToe_ValidMedewerker_AddsAbonnementAndSendsEmail()
//        {
//            // Arrange
//            var beheerderId = Guid.NewGuid();
//            var medewerkerId = Guid.NewGuid();
//            var abonnementId = Guid.NewGuid();
//            var medewerkers = new List<BedrijfsMedewerkers> { new BedrijfsMedewerkers { bedrijfsMedewerkerId = medewerkerId, medewerkerNaam = "Jan", medewerkerEmail = "jan@example.com" } };
//            var medewerker = medewerkers.First();

//            _wagenparkBeheerderRepositoryMock.Setup(r => r.GetMedewerkersByWagenparkbeheerder(beheerderId)).Returns(medewerkers);
//            _wagenparkBeheerderRepositoryMock.Setup(r => r.GetAbonnementId(beheerderId)).Returns(abonnementId);

//            // Act
//            _abonnementService.VoegMedewerkerToe(beheerderId, medewerkerId);

//            // Assert
//            Assert.Equal(abonnementId, medewerker.AbonnementId); // Check if the subscription was assigned
//            _wagenparkBeheerderRepositoryMock.Verify(r => r.Save(), Times.Once); // Verify that changes were saved

//            _emailServiceMock.Verify(e => e.SendEmail(
//                medewerker.medewerkerEmail,
//                "Welkom bij het bedrijfsabonnement",
//                It.Is<string>(s => s.Contains("U bent toegevoegd als medewerker bij het bedrijfsabonnement"))),
//                Times.Once);
//        }

//        [Fact]
//        public void VoegMedewerkerToe_MedewerkerNotInWagenpark_ThrowsInvalidOperationException()
//        {
//            // Arrange
//            var beheerderId = Guid.NewGuid();
//            var medewerkerId = Guid.NewGuid();
//            var medewerkers = new List<BedrijfsMedewerkers>();

//            _wagenparkBeheerderRepositoryMock.Setup(r => r.GetMedewerkersByWagenparkbeheerder(beheerderId)).Returns(medewerkers);

//            // Act & Assert
//            var exception = Assert.Throws<InvalidOperationException>(() => _abonnementService.VoegMedewerkerToe(beheerderId, medewerkerId));
//            Assert.Equal("Deze medewerker zit niet in uw wagenpark.", exception.Message);
//        }

//        [Fact]
//        public void VoegMedewerkerToe_MedewerkerHasAbonnement_ThrowsInvalidOperationException()
//        {
//            // Arrange
//            var beheerderId = Guid.NewGuid();
//            var medewerkerId = Guid.NewGuid();
//            var medewerkers = new List<BedrijfsMedewerkers> { new BedrijfsMedewerkers { bedrijfsMedewerkerId = medewerkerId, AbonnementId = Guid.NewGuid() } };

//            _wagenparkBeheerderRepositoryMock.Setup(r => r.GetMedewerkersByWagenparkbeheerder(beheerderId)).Returns(medewerkers);

//            // Act & Assert
//            var exception = Assert.Throws<InvalidOperationException>(() => _abonnementService.VoegMedewerkerToe(beheerderId, medewerkerId));
//            Assert.Equal("Deze medewerker heeft al een abonnement.", exception.Message);
//        }
//    }
//}