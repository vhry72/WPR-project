using System;
using System.Collections.Generic;
using Xunit;
using Moq;
using WPR_project.Models;
using WPR_project.Repositories;
using WPR_project.Services;

namespace WPR_project.TemporaryTests;

    public class BeschikbaarheidsTests
    {
    [Fact] // Testcase 6 Scenario 1: Succesvol bekijken van voertuigbeschikbaarheid en details
    public void GetAvailableVoertuigen_Successful_ReturnsVehiclesWithDetails()
    {
        // Arrange
        var mockRepo = new Mock<IVoertuigRepository>();
        var voertuigService = new VoertuigService(mockRepo.Object);

        var startDatum = new DateTime(2024, 1, 1);
        var eindDatum = new DateTime(2024, 1, 10);

        var voertuigen = new List<Voertuig>
    {
        new Voertuig { voertuigId = Guid.NewGuid(), merk = "Toyota", model = "Corolla", prijsPerDag = 50, voertuigBeschikbaar = true, startDatum = startDatum, eindDatum = eindDatum },
        new Voertuig { voertuigId = Guid.NewGuid(), merk = "Honda", model = "Civic", prijsPerDag = 60, voertuigBeschikbaar = true, startDatum = startDatum, eindDatum = eindDatum }
    };

        mockRepo.Setup(repo => repo.GetFilteredVoertuigen(null, startDatum, eindDatum, null))
                .Returns(voertuigen);

        // Act
        var resultaat = voertuigService.GetFilteredVoertuigen(null, startDatum, eindDatum, null);

        // Assert
        Assert.NotNull(resultaat);
        Assert.Equal(2, resultaat.Count());
        Assert.Contains(resultaat, v => v.merk == "Toyota");
        Assert.Contains(resultaat, v => v.merk == "Honda");
        Assert.All(resultaat, v => Assert.True(v.voertuigBeschikbaar));
        mockRepo.Verify(repo => repo.GetFilteredVoertuigen(null, startDatum, eindDatum, null), Times.Once);
    }


    [Fact] // Testcase 6 Scenario 2: Geen voertuigen voldoen aan de filtercriteria
        public void GetAvailableVoertuigen_NoMatches_ReturnsEmptyList()
        {
            // Arrange
            var mockRepo = new Mock<IVoertuigRepository>();
            var voertuigService = new VoertuigService(mockRepo.Object);

            mockRepo.Setup(repo => repo.GetFilteredVoertuigen(It.IsAny<string>(), It.IsAny<DateTime?>(), It.IsAny<DateTime?>(), null))
                    .Returns(new List<Voertuig>());

            // Act
            var resultaat = voertuigService.GetFilteredVoertuigen(null, new DateTime(2024, 2, 1), new DateTime(2024, 2, 10), null);

            // Assert
            Assert.NotNull(resultaat);
            Assert.Empty(resultaat);
            mockRepo.Verify(repo => repo.GetFilteredVoertuigen(null, new DateTime(2024, 2, 1), new DateTime(2024, 2, 10), null), Times.Once);
        }

        [Fact] // Testcase 6 Scenario 1: Voertuigdetails worden correct opgehaald
        public void GetVoertuigDetails_Success_ReturnsDetails()
        {
            // Arrange
            var mockRepo = new Mock<IVoertuigRepository>();
            var voertuigService = new VoertuigService(mockRepo.Object);

            var voertuigId = Guid.NewGuid();
            var voertuig = new Voertuig
            {
                voertuigId = voertuigId,
                merk = "Toyota",
                model = "Corolla",
                prijsPerDag = 50,
                voertuigBeschikbaar = true,
                startDatum = new DateTime(2024, 1, 1),
                eindDatum = new DateTime(2024, 1, 10)
            };

            mockRepo.Setup(repo => repo.GetVoertuigById(voertuigId))
                    .Returns(voertuig);

            // Act
            var resultaat = voertuigService.GetVoertuigDetails(voertuigId);

            // Assert
            Assert.NotNull(resultaat);
            Assert.Equal("Toyota", resultaat.merk);
            Assert.Equal("Corolla", resultaat.model);
            Assert.Equal(50, resultaat.prijsPerDag);
            Assert.True(resultaat.voertuigBeschikbaar);
            mockRepo.Verify(repo => repo.GetVoertuigById(voertuigId), Times.Once);
        }
    }
