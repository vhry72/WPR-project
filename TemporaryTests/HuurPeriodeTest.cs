using System;
using System.Collections.Generic;
using Xunit;
using Moq;
using WPR_project.Services;
using WPR_project.Repositories;
using WPR_project.Models;

namespace WPR_project.TemporaryTests;

public class HuurPeriodeTests
{
    [Fact]
    public void GetFilteredVoertuigen_ValidDates_ReturnsAvailableVehicles()
    {
        // Arrange
        var mockRepo = new Mock<IVoertuigRepository>();
        var voertuigService = new VoertuigService(mockRepo.Object);

        var startDatum = new DateTime(2024, 1, 1);
        var eindDatum = new DateTime(2024, 1, 10);
        var voertuigType = "Auto";

        var beschikbareVoertuigen = new List<Voertuig>
        {
            new Voertuig { voertuigId = Guid.NewGuid(), merk = "Toyota", model = "Corolla", voertuigBeschikbaar = true },
            new Voertuig { voertuigId = Guid.NewGuid(), merk = "Honda", model = "Civic", voertuigBeschikbaar = true }
        };

        mockRepo.Setup(repo => repo.GetFilteredVoertuigen(voertuigType, startDatum, eindDatum, null))
                .Returns(beschikbareVoertuigen);

        // Act
        var resultaat = voertuigService.GetFilteredVoertuigen(voertuigType, startDatum, eindDatum, null);

        // Assert
        Assert.NotNull(resultaat);
        Assert.Equal(2, resultaat.Count());
        Assert.Contains(resultaat, v => v.merk == "Toyota");
        Assert.Contains(resultaat, v => v.merk == "Honda");
        mockRepo.Verify(repo => repo.GetFilteredVoertuigen(voertuigType, startDatum, eindDatum, null), Times.Once);
    }

    [Fact]
    public void GetFilteredVoertuigen_NoVehiclesAvailable_ReturnsEmptyList()
    {
        // Arrange
        var mockRepo = new Mock<IVoertuigRepository>();
        var voertuigService = new VoertuigService(mockRepo.Object);

        var startDatum = new DateTime(2024, 1, 1);
        var eindDatum = new DateTime(2024, 1, 10);
        var voertuigType = "Auto";

        var legeLijst = new List<Voertuig>();

        mockRepo.Setup(repo => repo.GetFilteredVoertuigen(voertuigType, startDatum, eindDatum, null))
                .Returns(legeLijst);

        // Act
        var resultaat = voertuigService.GetFilteredVoertuigen(voertuigType, startDatum, eindDatum, null);

        // Assert
        Assert.NotNull(resultaat);
        Assert.Empty(resultaat);
        mockRepo.Verify(repo => repo.GetFilteredVoertuigen(voertuigType, startDatum, eindDatum, null), Times.Once);
    }

    [Fact]
    public void GetFilteredVoertuigen_InvalidDates_ThrowsArgumentException()
    {
        // Arrange
        var mockRepo = new Mock<IVoertuigRepository>();
        var voertuigService = new VoertuigService(mockRepo.Object);

        var startDatum = new DateTime(2024, 1, 10);
        var eindDatum = new DateTime(2024, 1, 1);
        var voertuigType = "Auto";

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => voertuigService.GetFilteredVoertuigen(voertuigType, startDatum, eindDatum, null));
        Assert.Equal("Startdatum mag niet later of gelijk zijn aan de einddatum.", exception.Message);
        mockRepo.Verify(repo => repo.GetFilteredVoertuigen(It.IsAny<string>(), It.IsAny<DateTime?>(), It.IsAny<DateTime?>(), It.IsAny<string>()), Times.Never);
    }

    [Fact]
    public void GetFilteredVoertuigen_MissingDates_ThrowsArgumentException()
    {
        // Arrange
        var mockRepo = new Mock<IVoertuigRepository>();
        var voertuigService = new VoertuigService(mockRepo.Object);

        DateTime? startDatum = null;
        DateTime? eindDatum = new DateTime(2024, 1, 10);
        var voertuigType = "Auto";

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => voertuigService.GetFilteredVoertuigen(voertuigType, startDatum, eindDatum, null));
        Assert.Equal("Startdatum en einddatum moeten beide ingevuld zijn.", exception.Message);
        mockRepo.Verify(repo => repo.GetFilteredVoertuigen(It.IsAny<string>(), It.IsAny<DateTime?>(), It.IsAny<DateTime?>(), It.IsAny<string>()), Times.Never);
    }

    [Fact]//testcase 7 senario 1
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

}
