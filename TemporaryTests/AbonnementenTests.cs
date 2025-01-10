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
        private readonly Mock<IWagenparkBeheerderRepository> _wagenparkBeheerderRepositoryMock;
        private readonly Mock<IEmailService> _emailServiceMock;
        private readonly AbonnementService _abonnementService;

        public AbonnementenTests()
        {
            _abonnementRepositoryMock = new Mock<IAbonnementRepository>();
            _wagenparkBeheerderRepositoryMock = new Mock<IWagenparkBeheerderRepository>();
            _emailServiceMock = new Mock<IEmailService>();

            _abonnementService = new AbonnementService(
                _abonnementRepositoryMock.Object,
                null,
                _wagenparkBeheerderRepositoryMock.Object,
                _emailServiceMock.Object
            );
        }

        [Fact]
        public void WijzigAbonnement_SuccesvolWijzigen_VoegtAbonnementToe()
        {
            // Arrange
            var beheerderId = Guid.NewGuid();
            var nieuwAbonnementId = Guid.NewGuid();
            var huidigAbonnement = new Abonnement
            {
                AbonnementId = Guid.NewGuid(),
                Naam = "Maandelijks",
                Kosten = 10,
                AbonnementType = AbonnementType.PayAsYouGo
            };
            var nieuwAbonnement = new Abonnement
            {
                AbonnementId = nieuwAbonnementId,
                Naam = "Jaarlijks",
                Kosten = 100,
                AbonnementType = AbonnementType.PrepaidSaldo // Stel het type in
            };

            var wagenparkBeheerder = new WagenparkBeheerder
            {
                beheerderId = beheerderId,
                beheerderNaam = "Jan Janssen",
                bedrijfsEmail = "jan.janssen@bedrijf.nl",
                HuidigAbonnement = huidigAbonnement
            };

            _wagenparkBeheerderRepositoryMock
                .Setup(repo => repo.GetBeheerderById(beheerderId))
                .Returns(wagenparkBeheerder);

            _abonnementRepositoryMock
                .Setup(repo => repo.GetAbonnementById(nieuwAbonnementId))
                .Returns(nieuwAbonnement);

            // Act
            _abonnementService.WijzigAbonnement(beheerderId, nieuwAbonnementId, AbonnementType.PrepaidSaldo);

            // Assert
            Assert.Equal(nieuwAbonnement, wagenparkBeheerder.HuidigAbonnement);
            Assert.Equal(AbonnementType.PrepaidSaldo, wagenparkBeheerder.AbonnementType);
            Assert.NotNull(wagenparkBeheerder.updateDatumAbonnement);

            _wagenparkBeheerderRepositoryMock.Verify(repo => repo.UpdateWagenparkBeheerder(wagenparkBeheerder), Times.Once);
            _wagenparkBeheerderRepositoryMock.Verify(repo => repo.Save(), Times.Once);

            _emailServiceMock.Verify(e => e.SendEmail(
                wagenparkBeheerder.bedrijfsEmail,
                "Bevestiging van uw abonnementswijziging",
                It.Is<string>(body => body.Contains("Jaarlijks"))
            ), Times.Once);
        }

        [Fact]
        public void WijzigAbonnement_FoutAbonnementNietBeschikbaar_GooitException()
        {
            // Arrange
            var beheerderId = Guid.NewGuid();
            var nietBeschikbaarAbonnementId = Guid.NewGuid();

            var wagenparkBeheerder = new WagenparkBeheerder
            {
                beheerderId = beheerderId,
                beheerderNaam = "Jan Janssen"
            };

            _wagenparkBeheerderRepositoryMock
                .Setup(repo => repo.GetBeheerderById(beheerderId))
                .Returns(wagenparkBeheerder);

            _abonnementRepositoryMock
                .Setup(repo => repo.GetAbonnementById(nietBeschikbaarAbonnementId))
                .Returns((Abonnement)null);

            // Act & Assert
            var ex = Assert.Throws<KeyNotFoundException>(() =>
                _abonnementService.WijzigAbonnement(beheerderId, nietBeschikbaarAbonnementId, AbonnementType.PrepaidSaldo)
            );

            Assert.Equal("Abonnement niet gevonden.", ex.Message);

            _wagenparkBeheerderRepositoryMock.Verify(repo => repo.UpdateWagenparkBeheerder(It.IsAny<WagenparkBeheerder>()), Times.Never);
            _emailServiceMock.Verify(e => e.SendEmail(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public void WijzigAbonnement_NietBestaandeBeheerder_GooitException()
        {
            // Arrange
            var beheerderId = Guid.NewGuid();
            var nieuwAbonnementId = Guid.NewGuid();

            _wagenparkBeheerderRepositoryMock
                .Setup(repo => repo.GetBeheerderById(beheerderId))
                .Returns((WagenparkBeheerder)null);

            // Act
            var ex = Assert.Throws<KeyNotFoundException>(() =>
                _abonnementService.WijzigAbonnement(beheerderId, nieuwAbonnementId, AbonnementType.PayAsYouGo)
            );

            // Assert
            Assert.Equal("Wagenparkbeheerder niet gevonden", ex.Message);

            _abonnementRepositoryMock.Verify(repo => repo.GetAbonnementById(It.IsAny<Guid>()), Times.Never);
            _wagenparkBeheerderRepositoryMock.Verify(repo => repo.UpdateWagenparkBeheerder(It.IsAny<WagenparkBeheerder>()), Times.Never);
        }
    }
}
