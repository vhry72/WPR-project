using System;
using Xunit;
using Moq;
using WPR_project.Models;
using WPR_project.Repositories;
using WPR_project.Services;
using WPR_project.Services.Email;

namespace WPR_project.TemporaryTests
{
    public class AbonnementServiceTests
    {
        private readonly Mock<IAbonnementRepository> _abonnementRepositoryMock;
        private readonly Mock<IWagenparkBeheerderRepository> _wagenparkBeheerderRepositoryMock;
        private readonly Mock<IEmailService> _emailServiceMock;
        private readonly AbonnementService _abonnementService;

        public AbonnementServiceTests()
        {
            //gebruik mocks om de repository en email service te mocken
            _abonnementRepositoryMock = new Mock<IAbonnementRepository>();
            _wagenparkBeheerderRepositoryMock = new Mock<IWagenparkBeheerderRepository>();
            _emailServiceMock = new Mock<IEmailService>();

            // maak een nieuwe AbonnementService aan met de mocks
            _abonnementService = new AbonnementService(
                _abonnementRepositoryMock.Object,
                null,
                _wagenparkBeheerderRepositoryMock.Object,
                _emailServiceMock.Object
            );
        }

        [Fact]
        public void StuurFactuurEmail_VerifieertEmailVerstuurd()
        {
            // Arrange
            var beheerderId = Guid.NewGuid();
            var abonnementId = Guid.NewGuid();
            var beheerder = new WagenparkBeheerder { beheerderNaam = "Jan", bedrijfsEmail = "jan@bedrijf.nl" };
            var abonnement = new Abonnement { Naam = "Maandelijks", Kosten = 100m };

            // haal de repository mock op en geef de beheerder en abonnement terug
            _wagenparkBeheerderRepositoryMock.Setup(r => r.GetBeheerderById(beheerderId)).Returns(beheerder);
            _abonnementRepositoryMock.Setup(r => r.GetAbonnementById(abonnementId)).Returns(abonnement);

            // Act
            _abonnementService.StuurFactuurEmail(beheerderId, abonnementId);
            // zorgt dat de email verstuurd wordt met de juiste gegevens


            // Assert
            _emailServiceMock.Verify(e => e.SendEmail(
                beheerder.bedrijfsEmail,
                It.IsAny<string>(),
                It.Is<string>(s => s.Contains("Maandelijks") && s.Contains("100"))
            ), Times.Once);
            // 2 keer string gebruiken want dat zit in de email body
        }

        [Fact]
        public void WijzigAbonnement_SuccesvolWijzigen_VerifieertTweeEmailsVerstuurd()
        {
            // Arrange
            var beheerderId = Guid.NewGuid();
            var nieuwAbonnementId = Guid.NewGuid();
            var nieuwAbonnement = new Abonnement { AbonnementId = nieuwAbonnementId, Naam = "Jaarlijks", Kosten = 100m, AbonnementType = AbonnementType.PrepaidSaldo };
            var beheerder = new WagenparkBeheerder { beheerderId = beheerderId, beheerderNaam = "Jan", bedrijfsEmail = "jan@bedrijf.nl" };

            _wagenparkBeheerderRepositoryMock.Setup(r => r.GetBeheerderById(beheerderId)).Returns(beheerder);
            _abonnementRepositoryMock.Setup(r => r.GetAbonnementById(nieuwAbonnementId)).Returns(nieuwAbonnement);

            // Act
            _abonnementService.WijzigAbonnement(beheerderId, nieuwAbonnementId, AbonnementType.PrepaidSaldo);

            // Assert: controleer dat er precies twee e-mails zijn verstuurd
            _emailServiceMock.Verify(e => e.SendEmail(
                It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()
            ), Times.Exactly(2));
        }

    }
}