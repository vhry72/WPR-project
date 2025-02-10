describe('Wagenparkbeheerder Dashboard', () => {

    // Authentication skip
    beforeEach(() => {
        localStorage.setItem("role", "Wagenparkbeheerder");
    });

    context("Happy flow", () => {

        // Mock een response voor elke API call op de pagina
        beforeEach(function () {
     
            cy.intercept(
                {
                    method: 'GET',
                    url: '/api/account/user-info'
                },
                { fixture: 'user-info.fixture.json' }
            );


            cy.intercept(
                {
                    method: 'GET',
                    url: '/api/WagenparkBeheerder/ba6e3439-7021-4c1c-b92e-4451a254f1b3/medewerker-object'
                },
                { fixture: 'medewerkers-object.fixture.json' }
            );

            cy.intercept(
                {
                    method: 'POST',
                    url: '/api/Abonnement/ba6e3439-7021-4c1c-b92e-4451a254f1b3/medewerker/toevoegen/425e43ed-36b4-466e-bb50-702069552a1f'
                },
                { fixture: 'medewerker-toevoegen.fixture.json' }
            );

            cy.intercept(
                {
                    method: 'DELETE',
                    url: '/api/Abonnement/ba6e3439-7021-4c1c-b92e-4451a254f1b3/medewerker/verwijderen/1d840e69-f8e6-4331-8ec2-6f193c6a4ee9'
                },
                {fixture : 'medewerker-verwijderen.fixture.json'}
                
            );

        });

        it('Happy flow', () => {
            cy.visit('https://localhost:5173/wagenbeheer').then(() => {
                cy.log('Page loaded');
            });

        });

    });

});
