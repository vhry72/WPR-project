using Moq;
using Xunit;
using Microsoft.AspNetCore.Mvc;
using WPR_project.Controllers;
using WPR_project.Models;
using WPR_project.Services;
using WPR_project.Services.Email;
using System;
using System.Collections.Generic;
using WPR_project.Repositories;

namespace WPR_project.TemporaryTests
{
    public class MedewerkerToevoegenTests
    {
        private readonly Mock<IWagenparkBeheerderRepository> _repositoryMock;
        private readonly Mock<IEmailService> _emailServiceMock;
        private readonly WagenparkBeheerderService _service;
        private readonly WagenparkBeheerderController _controller;

        public MedewerkerToevoegenTests()
        {
            _repositoryMock = new Mock<IWagenparkBeheerderRepository>();
            _emailServiceMock = new Mock<IEmailService>();

            // Maak een echte service met de gemockte afhankelijkheden
            _service = new WagenparkBeheerderService(
                _repositoryMock.Object,
                null,  // Andere gemockte services indien nodig
                null,
                null,
                _emailServiceMock.Object
            );

            // Injecteer de echte service in de controller
            _controller = new WagenparkBeheerderController(_service, _emailServiceMock.Object);
        }

        /// <summary>
        /// Test voor succesvol toevoegen van een medewerker
        /// </summary>
        [Fact]
        public void VoegMedewerkerToe_Success_ReturnsOkWithMessage()
        {
            // Arrange
            var zakelijkeId = Guid.NewGuid();
            var medewerker = new BedrijfsMedewerkers
            {
                medewerkerNaam = "Piet Janssen",
                medewerkerEmail = "piet.janssen@bedrijf.nl",
                wachtwoord = "P@ssword123!"
            };

            _repositoryMock
                .Setup(r => r.getBeheerderById(zakelijkeId))
                .Returns(new WagenparkBeheerder
                {
                    beheerderId = zakelijkeId,
                    MedewerkerLijst = new List<BedrijfsMedewerkers>()
                });

            // Act
            var result = _controller.VoegMedewerkerToe(zakelijkeId, medewerker);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsAssignableFrom<dynamic>(okResult.Value);

            Assert.Equal("Medewerker succesvol toegevoegd en bevestigingsmail verzonden.", response.Message);
            _emailServiceMock.Verify(es => es.SendEmail(
                medewerker.medewerkerEmail,
                "Welkom bij het systeem - Bevestigingsmail",
                It.IsAny<string>()), Times.Once);
        }

        /// <summary>
        /// Test voor medewerker toevoegen met ongeldige gegevens
        /// </summary>
        [Fact]
        public void VoegMedewerkerToe_InvalidInput_ReturnsBadRequest()
        {
            // Arrange
            var zakelijkeId = Guid.NewGuid();
            var medewerker = new BedrijfsMedewerkers
            {
                medewerkerNaam = "", // Ongeldig, lege naam
                medewerkerEmail = "fout.email",
                wachtwoord = "1234" // Ongeldig wachtwoord
            };

            _controller.ModelState.AddModelError("medewerkerNaam", "Naam is vereist");
            _controller.ModelState.AddModelError("wachtwoord", "Wachtwoord voldoet niet aan eisen");

            // Act
            var result = _controller.VoegMedewerkerToe(zakelijkeId, medewerker);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var response = Assert.IsAssignableFrom<dynamic>(badRequestResult.Value);

            Assert.Contains("Ongeldige invoer.", response.Message);
            _emailServiceMock.Verify(es => es.SendEmail(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        }

        /// <summary>
        /// Test voor succesvol verwijderen van een medewerker
        /// </summary>
        [Fact]
        public void VerwijderMedewerker_Success_ReturnsOk()
        {
            // Arrange
            var zakelijkeId = Guid.NewGuid();
            var medewerkerId = Guid.NewGuid();

            _repositoryMock
                .Setup(r => r.getBeheerderById(zakelijkeId))
                .Returns(new WagenparkBeheerder
                {
                    beheerderId = zakelijkeId,
                    MedewerkerLijst = new List<BedrijfsMedewerkers>
                    {
                        new BedrijfsMedewerkers
                        {
                            bedrijfsMedewerkerId = medewerkerId,
                            medewerkerNaam = "Test Medewerker",
                            medewerkerEmail = "test@bedrijf.nl"
                        }
                    }
                });

            // Act
            var result = _controller.VerwijderMedewerker(zakelijkeId, medewerkerId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result); // Controleren op OkObjectResult
            var response = Assert.IsType<dynamic>(okResult.Value); // Response is dynamisch

            Assert.Equal("Medewerker succesvol verwijderd.", response.Message);
            _repositoryMock.Verify(r => r.UpdateWagenparkBeheerder(It.IsAny<WagenparkBeheerder>()), Times.Once);
        }

        /// <summary>
        /// Test voor verwijderen van een medewerker die niet bestaat
        /// </summary>
        [Fact]
        public void VerwijderMedewerker_NotFound_ReturnsNotFound()
        {
            // Arrange
            var zakelijkeId = Guid.NewGuid();
            var medewerkerId = Guid.NewGuid();

            _repositoryMock
                .Setup(r => r.getBeheerderById(zakelijkeId))
                .Returns(new WagenparkBeheerder
                {
                    beheerderId = zakelijkeId,
                    MedewerkerLijst = new List<BedrijfsMedewerkers>()
                });

            // Act
            var result = _controller.VerwijderMedewerker(zakelijkeId, medewerkerId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result); // Controleren op NotFoundObjectResult
            var response = Assert.IsType<dynamic>(notFoundResult.Value); // Response is dynamisch

            Assert.Equal("Medewerker niet gevonden.", response.Message);
            _repositoryMock.Verify(r => r.UpdateWagenparkBeheerder(It.IsAny<WagenparkBeheerder>()), Times.Never);
        }
    }
}
