using System;
using System.Collections.Generic;
using Xunit;
using Moq;
using WPR_project.Services;
using WPR_project.Repositories;
using WPR_project.Models;

namespace WPR_project.TemporaryTests;

public class VoertuigServiceTests
{
    [Fact] // Testcase 7 Scenario 1: Succesvolle selectie en aanvraag van een voertuig
    public void GetFilteredVoertuigen_SuccessfulSelection_ReturnsAvailableSortedVehicles()
    {
        // Arrange
        var mockRepo = new Mock<IVoertuigRepository>();
        var voertuigService = new VoertuigService(mockRepo.Object);

        var startDatum = new DateTime(2024, 1, 1);
        var eindDatum = new DateTime(2024, 1, 10);
        var voertuigType = "Auto";

        var voertuigen = new List<Voertuig>
            {
                new Voertuig { voertuigId = Guid.NewGuid(), merk = "Toyota", prijsPerDag = 50, voertuigBeschikbaar = true },
                new Voertuig { voertuigId = Guid.NewGuid(), merk = "Honda", prijsPerDag = 60, voertuigBeschikbaar = true }
            };

        mockRepo.Setup(repo => repo.GetFilteredVoertuigen(voertuigType, startDatum, eindDatum, "prijs"))
                .Returns(voertuigen);

        // Act
        var resultaat = voertuigService.GetFilteredVoertuigen(voertuigType, startDatum, eindDatum, "prijs");

        // Assert
        Assert.NotNull(resultaat);
        Assert.Equal(2, resultaat.Count());
        Assert.True(resultaat.First().merk == "Toyota");
        mockRepo.Verify(repo => repo.GetFilteredVoertuigen(voertuigType, startDatum, eindDatum, "prijs"), Times.Once);
    }

    [Fact] // Testcase 7 Scenario 2: Geen voertuigen voldoen aan de filtercriteria
    public void GetFilteredVoertuigen_NoVehiclesMatchFilter_ReturnsEmptyList()
    {
        // Arrange
        var mockRepo = new Mock<IVoertuigRepository>();
        var voertuigService = new VoertuigService(mockRepo.Object);

        var startDatum = new DateTime(2024, 1, 1);
        var eindDatum = new DateTime(2024, 1, 10);
        var voertuigType = "Camper";

        mockRepo.Setup(repo => repo.GetFilteredVoertuigen(voertuigType, startDatum, eindDatum, null))
                .Returns(new List<Voertuig>());

        // Act
        var resultaat = voertuigService.GetFilteredVoertuigen(voertuigType, startDatum, eindDatum, null);

        // Assert
        Assert.NotNull(resultaat);
        Assert.Empty(resultaat);
        mockRepo.Verify(repo => repo.GetFilteredVoertuigen(voertuigType, startDatum, eindDatum, null), Times.Once);
    }

    [Fact] // Testcase 7 Scenario 3: Sorteren werkt niet correct
    public void GetFilteredVoertuigen_InvalidSortOption_ThrowsArgumentException()
    {
        // Arrange
        var mockRepo = new Mock<IVoertuigRepository>();
        var voertuigService = new VoertuigService(mockRepo.Object);

        var startDatum = new DateTime(2024, 1, 1);
        var eindDatum = new DateTime(2024, 1, 10);
        var voertuigType = "Auto";

        mockRepo.Setup(repo => repo.GetFilteredVoertuigen(voertuigType, startDatum, eindDatum, "onbekendeSortering"))
                .Throws(new ArgumentException("Ongeldige sorteervolgorde."));

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() =>
            voertuigService.GetFilteredVoertuigen(voertuigType, startDatum, eindDatum, "onbekendeSortering"));

        Assert.Equal("Ongeldige sorteervolgorde.", exception.Message);
        mockRepo.Verify(repo => repo.GetFilteredVoertuigen(voertuigType, startDatum, eindDatum, "onbekendeSortering"), Times.Once);
    }
}
