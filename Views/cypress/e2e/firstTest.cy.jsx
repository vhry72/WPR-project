describe('CarAndAll Home Page', () => {
    beforeEach(() => {
        // Bezoek de homepagina
        cy.visit('https://localhost:5174'); // Zorg ervoor dat de juiste URL wordt gebruikt
    });

    it('verifies the cookie consent banner loads correctly and can be accepted', () => {
        // Controleer of de cookie banner correct laadt
        cy.get('.cookieConsentModal').should('be.visible');
        cy.get('.cookie-message').should('contain', 'Wij gebruiken een noodzakelijk cookie voor authenticatie die essentieel is voor de werking van deze website.');
        // Klik op de accepteer knop
        cy.get('.cookieAcceptButton').click();
        // Controleer of de banner verdwijnt na het accepteren
        cy.get('.cookieConsentModal').should('not.exist');
    });

    it('verifies the hero section loads correctly', () => {
        // Controleer of de hero-sectie de juiste tekst bevat
        cy.get('.hero-section .hero-text h1').should('contain', 'Welkom bij CarAndAll!');
        cy.get('.hero-section .hero-text p').should('contain', 'Ontdek de beste diensten die we aanbieden!');
    });

    it('verifies the services section displays all services', () => {
        // Scroll naar services sectie
        cy.get('#services').scrollIntoView();
        // Controleer of alle services correct worden weergegeven
        cy.get('.services-grid .service-card').should('have.length', 4);
        cy.get('.services-grid .service-card').each((service, index) => {
            cy.wrap(service).find('h3').should('exist'); // Controleer of elke service een titel heeft
            cy.wrap(service).find('img').should('exist'); // Controleer of elke service een afbeelding heeft
            cy.wrap(service).find('p').should('exist'); // Controleer of elke service een beschrijving heeft
        });
    });

    it('verifies the footer is visible', () => {
        // Controleer of de footer aanwezig is
        cy.get('footer').should('be.visible');
        cy.get('footer').should('contain', '\u00A9 2024 CarAndAll. Alle rechten voorbehouden.');
    });
});
