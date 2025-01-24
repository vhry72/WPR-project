describe('Accessibility Tests', () => {
    beforeEach(() => {
        // Bezoek de pagina die je wilt testen
        cy.visit('https://localhost:5174/');
        // Voer axe core-injectie uit
        cy.injectAxe();
    });

    it('Should have no detectable accessibility violations on load', () => {
        // Voer de accessibility check uit
        cy.checkA11y();
    });
});
