describe('CarAndAll Home Page', () => {
    beforeEach(() => {
        // Bezoek de homepagina
        cy.visit('http://localhost:5175/'); // Zorg ervoor dat de juiste URL wordt gebruikt
    });

    it('verifies the hero section loads correctly', () => {
        // Controleer of de hero-sectie de juiste tekst bevat
        cy.get('.hero-text-container h1').should('contain', 'Welkom bij CarAndAll!');
        cy.get('.hero-text-container p').should('contain', 'Ontdek de beste diensten die we aanbieden!');
    });

    it('verifies the "Ontdek Meer" button is clickable', () => {
        // Controleer of de knop aanwezig is en werkt
        cy.get('.ontdek-btn a.cta')
            .should('exist')
            .and('have.attr', 'href', '#services') // Controleer of de knop naar de juiste sectie verwijst
            .click();

        // Controleer of het scrollen naar de #services sectie werkt
        cy.get('.services').should('be.visible');
    });

    it('verifies the services section displays all services', () => {
        // Controleer of alle services correct worden weergegeven
        cy.get('.service-list .service').should('have.length', 4); // Controleer of er 4 services zijn
        cy.get('.service-list .service').each((service, index) => {
            cy.wrap(service).find('h3').should('exist'); // Controleer of elke service een titel heeft
            cy.wrap(service).find('img').should('exist'); // Controleer of elke service een afbeelding heeft
            cy.wrap(service).find('p').should('exist'); // Controleer of elke service een beschrijving heeft
        });
    });

    it('verifies the footer is visible', () => {
        // Controleer of de footer aanwezig is
        cy.get('footer').should('contain', '\u00A9 2024 CarAndAll. Alle rechten voorbehouden.');

    });
});
