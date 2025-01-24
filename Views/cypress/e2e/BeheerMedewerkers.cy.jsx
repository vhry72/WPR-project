describe('Wagenparkbeheerder Dashboard', () => {
        beforeEach(() => {
            cy.intercept('GET', '/api/WagenparkBeheerder/{id}/medewerkers', { fixture: 'medewerkers.fixture.json' });
            cy.visit('https://localhost:5174/wagenbeheer'); // Pas de URL aan aan jouw omgeving
        });

        it('laadt en toont de medewerker selectieformulier correct', () => {
            cy.get('.medewerker-form').should('be.visible');
            cy.get('#medewerkerSelect').should('exist');
            cy.get('button').contains('Toevoegen').should('be.disabled');
        });

    it('voegt een medewerker toe aan het abonnement en toont een notificatie', () => {
        cy.get('#medewerkerSelect').select('janedoe@bedrijf.nl');
        cy.get('button').contains('Toevoegen').should('not.be.disabled').click();
        cy.wait('@addMedewerker');
        cy.get('.notificatie-box').should('contain', 'Medewerker janedoe@bedrijf.nl toegevoegd aan abonnement.');
    });

    it('verwijdert een medewerker uit het abonnement en toont een notificatie', () => {
        cy.get('ul').contains('Verwijderen').first().click();
        cy.wait('@removeMedewerker');
        cy.get('.notificatie-box').should('contain', 'Medewerker met email verwijderd uit abonnement.');
    });

    it('validatie dat alleen medewerkers met een bedrijfsdomein kunnen worden toegevoegd', () => {
        cy.get('#medewerkerSelect').select('wrongemail@gmail.com');
        cy.get('button').contains('Toevoegen').click();
        cy.get('.notificatie-box').should('contain', 'Ongeldige email'); // Deze test hangt af van de daadwerkelijke validatie die op de server plaatsvindt
    });
});
