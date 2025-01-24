using System;
using System.Collections.Generic;
using Xunit;
using Moq;
using WPR_project.Models;
using WPR_project.Repositories;
using WPR_project.Services;

namespace WPR_project.TemporaryTests;

    public class ZakelijkVoertuigKiezenTests
    {
        [Fact] // Testcase 5 Scenario 1: Succesvolle selectie van een voertuig
        public void GetFilteredVoertuigen_ValidCriteria_ReturnsBusinessVehicles()
        {
            // Arrange
            var mockRepo = new Mock<IVoertuigRepository>();
            var voertuigService = new VoertuigService(mockRepo.Object);

            var startDatum = new DateTime(2024, 3, 1);
            var eindDatum = new DateTime(2024, 3, 10);
            var voertuigType = "Auto";

            var zakelijkeVoertuigen = new List<Voertuig>
            {
                new Voertuig { voertuigId = Guid.NewGuid(), merk = "BMW", model = "X5", voertuigType = "Auto", prijsPerDag = 100, voertuigBeschikbaar = true },
                new Voertuig { voertuigId = Guid.NewGuid(), merk = "Mercedes", model = "E-Klasse", voertuigType = "Auto", prijsPerDag = 120, voertuigBeschikbaar = true }
            };

            mockRepo.Setup(repo => repo.GetFilteredVoertuigen(voertuigType, startDatum, eindDatum, null))
                    .Returns(zakelijkeVoertuigen);

            // Act
            var resultaat = voertuigService.GetFilteredVoertuigen(voertuigType, startDatum, eindDatum, null);

            // Assert
            Assert.NotNull(resultaat);
            Assert.Equal(2, resultaat.Count());
            Assert.All(resultaat, v => Assert.Equal("Auto", v.voertuigType));
            Assert.Contains(resultaat, v => v.merk == "BMW");
            Assert.Contains(resultaat, v => v.merk == "Mercedes");
            mockRepo.Verify(repo => repo.GetFilteredVoertuigen(voertuigType, startDatum, eindDatum, null), Times.Once);
        }

        [Fact] // Testcase 5 Scenario 2: Geen voertuigen voldoen aan de filtercriteria
        public void GetFilteredVoertuigen_NoMatches_ReturnsEmptyList()
        {
            // Arrange
            var mockRepo = new Mock<IVoertuigRepository>();
            var voertuigService = new VoertuigService(mockRepo.Object);

            var startDatum = new DateTime(2024, 3, 1);
            var eindDatum = new DateTime(2024, 3, 10);
            var voertuigType = "Auto";

            mockRepo.Setup(repo => repo.GetFilteredVoertuigen(voertuigType, startDatum, eindDatum, null))
                    .Returns(new List<Voertuig>());

            // Act
            var resultaat = voertuigService.GetFilteredVoertuigen(voertuigType, startDatum, eindDatum, null);

            // Assert
            Assert.NotNull(resultaat);
            Assert.Empty(resultaat);
            mockRepo.Verify(repo => repo.GetFilteredVoertuigen(voertuigType, startDatum, eindDatum, null), Times.Once);
        }

        [Fact] // Testcase 5 Scenario 1: Voertuigdetails worden correct opgehaald
        public void GetVoertuigDetails_Success_ReturnsBusinessVehicleDetails()
        {
            // Arrange
            var mockRepo = new Mock<IVoertuigRepository>();
            var voertuigService = new VoertuigService(mockRepo.Object);

            var voertuigId = Guid.NewGuid();
            var voertuig = new Voertuig
            {
                voertuigId = voertuigId,
                merk = "Audi",
                model = "A6",
                voertuigType = "Auto",
                prijsPerDag = 110,
                voertuigBeschikbaar = true
            };

            mockRepo.Setup(repo => repo.GetVoertuigById(voertuigId))
                    .Returns(voertuig);

            // Act
            var resultaat = voertuigService.GetVoertuigDetails(voertuigId);

            // Assert
            Assert.NotNull(resultaat);
            Assert.Equal("Audi", resultaat.merk);
            Assert.Equal("A6", resultaat.model);
            Assert.Equal("Auto", resultaat.voertuigType);
            Assert.Equal(110, resultaat.prijsPerDag);
            Assert.True(resultaat.voertuigBeschikbaar);
            mockRepo.Verify(repo => repo.GetVoertuigById(voertuigId), Times.Once);
        }
    }

