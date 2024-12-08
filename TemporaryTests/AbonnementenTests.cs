using System;
using System.Collections.Generic;
using Xunit;
using Moq;
using WPR_project.Models;
using WPR_project.Repositories;
using WPR_project.Services;
using WPR_project.Services.Email;

namespace WPR_project.TemporaryTests
{
    public class AbonnementenTests
    {
        private readonly Mock<IAbonnementRepository> _abonnementRepositoryMock;
        private readonly Mock<IZakelijkeHuurderRepository> _zakelijkeHuurderRepositoryMock;
        private readonly Mock<IEmailService> _emailServiceMock;
        private readonly AbonnementService _abonnementService;

        public AbonnementenTests()
        {
            _abonnementRepositoryMock = new Mock<IAbonnementRepository>();
            _zakelijkeHuurderRepositoryMock = new Mock<IZakelijkeHuurderRepository>();
            _emailServiceMock = new Mock<IEmailService>();

            _abonnementService = new AbonnementService(
                _abonnementRepositoryMock.Object,
                _zakelijkeHuurderRepositoryMock.Object,
                _emailServiceMock.Object
            );
        }

        [Fact]
        public void WijzigAbonnement_SuccesvolWijzigen_VoegtAbonnementToe()
        {
            // Arrange
            var zakelijkeId = Guid.NewGuid();
            var nieuwAbonnementId = Guid.NewGuid();
            var huidigAbonnement = new Abonnement { AbonnementId = Guid.NewGuid(), Naam = "Maandelijks", Kosten = 10 };
            var nieuwAbonnement = new Abonnement { AbonnementId = nieuwAbonnementId, Naam = "Per kwartaal", Kosten = 25 };
            var zakelijkeHuurder = new ZakelijkHuurder
            {
                zakelijkeId = zakelijkeId,
                AbonnementId = huidigAbonnement.AbonnementId,
                HuidigAbonnement = huidigAbonnement,
                email = "bedrijf@example.com",
                bedrijfsNaam = "Test Bedrijf"
            };

            _zakelijkeHuurderRepositoryMock
                .Setup(repo => repo.GetZakelijkHuurderById(zakelijkeId))
                .Returns(zakelijkeHuurder);

            _abonnementRepositoryMock
                .Setup(repo => repo.GetAbonnementById(nieuwAbonnementId))
                .Returns(nieuwAbonnement);

            // Act
            _abonnementService.WijzigAbonnement(zakelijkeId, nieuwAbonnementId);

            // Assert
            Assert.Equal(nieuwAbonnementId, zakelijkeHuurder.NieuwAbonnementId);
            Assert.NotNull(zakelijkeHuurder.IngangsdatumNieuwAbonnement);

            _emailServiceMock.Verify(
                email => email.SendEmail(
                    zakelijkeHuurder.email,
                    It.Is<string>(s => s.Contains("Bevestiging van uw abonnementswijziging")),
                    It.Is<string>(b => b.Contains("Per kwartaal"))
                ),
                Times.Once
            );

            _zakelijkeHuurderRepositoryMock.Verify(repo => repo.UpdateZakelijkHuurder(zakelijkeHuurder), Times.Once);
            _zakelijkeHuurderRepositoryMock.Verify(repo => repo.Save(), Times.Once);
        }

        [Fact]
        public void WijzigAbonnement_FoutAbonnementNietBeschikbaar_GooitException()
        {
            // Arrange
            var zakelijkeId = Guid.NewGuid();
            var nietBeschikbaarAbonnementId = Guid.NewGuid();
            var zakelijkeHuurder = new ZakelijkHuurder { zakelijkeId = zakelijkeId };

            _zakelijkeHuurderRepositoryMock
                .Setup(repo => repo.GetZakelijkHuurderById(zakelijkeId))
                .Returns(zakelijkeHuurder);

            _abonnementRepositoryMock
                .Setup(repo => repo.GetAbonnementById(nietBeschikbaarAbonnementId))
                .Returns((Abonnement)null); // Abonnement bestaat niet

            // Act & Assert
            var exception = Assert.Throws<KeyNotFoundException>(() =>
                _abonnementService.WijzigAbonnement(zakelijkeId, nietBeschikbaarAbonnementId)
            );

            Assert.Equal("Abonnement niet gevonden.", exception.Message);

            _zakelijkeHuurderRepositoryMock.Verify(repo => repo.UpdateZakelijkHuurder(It.IsAny<ZakelijkHuurder>()), Times.Never);
            _emailServiceMock.Verify(email => email.SendEmail(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public void WijzigAbonnement_GebruikerAnnuleertWijziging_GeenWijzigingOpgeslagen()
        {
            // Arrange
            var zakelijkeId = Guid.NewGuid();
            var huidigAbonnement = new Abonnement { AbonnementId = Guid.NewGuid(), Naam = "Maandelijks", Kosten = 10 };
            var zakelijkeHuurder = new ZakelijkHuurder
            {
                zakelijkeId = zakelijkeId,
                AbonnementId = huidigAbonnement.AbonnementId,
                HuidigAbonnement = huidigAbonnement
            };

            _zakelijkeHuurderRepositoryMock
                .Setup(repo => repo.GetZakelijkHuurderById(zakelijkeId))
                .Returns(zakelijkeHuurder);

            //Act leeg frontend cancel

            // Assert
            Assert.Equal(huidigAbonnement.AbonnementId, zakelijkeHuurder.AbonnementId);
            _zakelijkeHuurderRepositoryMock.Verify(repo => repo.UpdateZakelijkHuurder(It.IsAny<ZakelijkHuurder>()), Times.Never);
            _emailServiceMock.Verify(email => email.SendEmail(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        }
    }
}
