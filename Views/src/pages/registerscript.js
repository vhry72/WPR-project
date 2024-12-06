function RegisterScript() {
    // Voeg een DOMContentLoaded event toe om te zorgen dat de DOM is geladen
    document.addEventListener('DOMContentLoaded', () => {
        // Selecteer de tabs en formulieren
        const tabs = document.querySelectorAll('.tab');
        const particulierForm = document.getElementById('ParticulierForm');
        const zakelijkForm = document.getElementById('ZakelijkForm');
        const tabsContainer = document.querySelector('.tabs');

        if (!particulierForm || !zakelijkForm || !tabsContainer) {
            console.error("Required elements are missing in the DOM.");
            return;
        }

        // Voeg een click event listener toe aan elke tab
        tabs.forEach(tab => {
            tab.addEventListener('click', () => {
                // Verwijder 'active' klasse van alle tabs
                tabs.forEach(t => t.classList.remove('active'));

                // Voeg 'active' klasse toe aan de aangeklikte tab
                tab.classList.add('active');

                // Toon het juiste formulier en pas de achtergrondkleur aan
                if (tab.dataset.tab === "zakelijk") {
                    tabsContainer.style.backgroundColor = "#f0f0f0";
                    particulierForm.style.display = "none";
                    zakelijkForm.style.display = "block";
                } else {
                    tabsContainer.style.backgroundColor = "#fff";
                    particulierForm.style.display = "block";
                    zakelijkForm.style.display = "none";
                }
            });
        });
    });
}

export default RegisterScript;
