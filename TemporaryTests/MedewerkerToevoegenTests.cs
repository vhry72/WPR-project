using Moq;
using Xunit;
using Microsoft.AspNetCore.Mvc;
using WPR_project.Controllers;
using WPR_project.Models;
using WPR_project.Services;
using WPR_project.Services.Email;
using System;
using System.Collections.Generic;




namespace WPR_project.TemporaryTests
{
    

   
        public class MedewerkerToevoegenTests
        {
            private readonly Mock<WagenparkBeheerderService> _serviceMock;
            private readonly Mock<IEmailService> _emailServiceMock;
            private readonly WagenparkBeheerderController _controller;

            public MedewerkerToevoegenTests()
            {
                // Mock services
                _serviceMock = new Mock<WagenparkBeheerderService>();
                _emailServiceMock = new Mock<IEmailService>();

                // Instantiate controller with mocks
                _controller = new WagenparkBeheerderController(_serviceMock.Object, _emailServiceMock.Object);
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

                _serviceMock
                    .Setup(s => s.VoegMedewerkerToe(zakelijkeId, medewerker.medewerkerNaam, medewerker.medewerkerEmail, medewerker.wachtwoord))
                    .Verifiable();

                // Act
                var result = _controller.VoegMedewerkerToe(zakelijkeId, medewerker);

                // Assert
                var okResult = Assert.IsType<OkObjectResult>(result);
                var response = Assert.IsAssignableFrom<dynamic>(okResult.Value);

                Assert.Equal("Medewerker succesvol toegevoegd en bevestigingsmail verzonden.", response.Message);
                _serviceMock.Verify(s => s.VoegMedewerkerToe(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
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
                _serviceMock.Verify(s => s.VoegMedewerkerToe(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Never);
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

                _serviceMock
                    .Setup(s => s.GetMedewerkerById(medewerkerId))
                    .Returns(new BedrijfsMedewerkers { medewerkerNaam = "Test Medewerker" });

                _serviceMock
                    .Setup(s => s.VerwijderMedewerker(zakelijkeId, medewerkerId))
                    .Verifiable();

                // Act
                var result = _controller.VerwijderMedewerker(zakelijkeId, medewerkerId);

                // Assert
                var okResult = Assert.IsType<OkObjectResult>(result);
                var response = Assert.IsAssignableFrom<dynamic>(okResult.Value);

                Assert.Equal("Medewerker succesvol verwijderd.", response.Message);
                _serviceMock.Verify(s => s.VerwijderMedewerker(zakelijkeId, medewerkerId), Times.Once);
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

                _serviceMock
                    .Setup(s => s.GetMedewerkerById(medewerkerId))
                    .Returns((BedrijfsMedewerkers)null);

                // Act
                var result = _controller.VerwijderMedewerker(zakelijkeId, medewerkerId);

                // Assert
                var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
                var response = Assert.IsAssignableFrom<dynamic>(notFoundResult.Value);

                Assert.Equal("Medewerker niet gevonden.", response.Message);
                _serviceMock.Verify(s => s.VerwijderMedewerker(It.IsAny<Guid>(), It.IsAny<Guid>()), Times.Never);
            }
        }
}
