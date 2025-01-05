describe('Particulier Voertuig Tonen Cypress Test', () => {
    beforeEach(() => {
        cy.visit('http://localhost:5177/particulierVoertuigTonen');
    });

    it('Filtert voertuigen op type en controleert resultaten', () => {
        cy.get('select').select('auto');
        cy.get('button').contains('Voer voertuigType in').click();

        cy.get('tbody tr').should('have.length.greaterThan', 0);
    });

    it('Sorteert voertuigen op merk', () => {
        cy.get('button').contains('Sorteer op Merk').click();
        cy.get('tbody tr').first().find('td').eq(0).invoke('text').then((firstText) => {
            cy.get('tbody tr').eq(1).find('td').eq(0).invoke('text').should((secondText) => {
                expect(firstText.localeCompare(secondText)).to.be.at.most(0);
            });
        });
    });

    it('Toont voertuigdetails na selectie', () => {
        cy.get('button').contains('Voer voertuigType in').click();
        cy.get('tbody tr').first().find('td').first().click();

        cy.url().should('include', '/huurVoertuig');
        cy.url().should('include', 'VoertuigID');
    });

    it('Geeft een melding wanneer geen voertuigen gevonden zijn', () => {
        cy.get('select').select('camper');
        cy.get('button').contains('Voer voertuigType in').click();

        cy.get('tbody tr').should('have.length', 0);
        cy.get('body').should('contain', 'Geen voertuigen gevonden').should('be.visible');
    });
});
