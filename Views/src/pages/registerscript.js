// Selecteer de tabs, formulieren en registratieknoppen
const tabs = document.querySelectorAll('.tab');
const particulierForm = document.getElementById('form-particulier');
const zakelijkForm = document.getElementById('form-zakelijk');
const tabsContainer = document.querySelector('.tabs');

// Voeg een click event listener toe aan elke tab
tabs.forEach(tab => {
    tab.addEventListener('click', () => {
        // Verwijder 'active' klasse van alle tabs
        tabs.forEach(t => t.classList.remove('active'));

        // Voeg 'active' klasse toe aan de aangeklikte tab
        tab.classList.add('active');

        // Toon het juiste formulier en pas de achtergrondkleur aan
        if (tab.dataset.tab === "zakelijk") {
            tabsContainer.style.backgroundColor = '#596ffa'; // Rood voor Zakelijk
            particulierForm.style.display = 'none';
            zakelijkForm.style.display = 'block';
        } else {
            tabsContainer.style.backgroundColor = '#b258ef'; // Blauw voor Particulier
            particulierForm.style.display = 'block';
            zakelijkForm.style.display = 'none';
        }
    });
});

// Functie om het registreren te behandelen
function handleFormSubmit(event, type) {
    event.preventDefault(); // Voorkom standaard formulier gedrag

    // Stuur de gebruiker naar het juiste dashboard afhankelijk van het type
    if (type === "particulier") {
        window.location.href = 'partdashboard.html';
    } else if (type === "zakelijk") {
        window.location.href = 'abonnement.html';
    }
}

// Voeg submit event listener toe aan beide formulieren
particulierForm.addEventListener('submit', (event) => handleFormSubmit(event, "particulier"));
zakelijkForm.addEventListener('submit', (event) => handleFormSubmit(event, "zakelijk"));
