// Selecteer tabs, forms en de container
const tabs = document.querySelectorAll('.tab');
const particulierForm = document.getElementById('form-particulier');
const zakelijkForm = document.getElementById('form-zakelijk');
const medewerkerForm = document.getElementById('form-mededewerker');
const tabsContainer = document.querySelector('.tabs');

// Voeg click event listeners toe aan tabs
tabs.forEach(tab => {
    tab.addEventListener('click', () => {
        // Verwijder 'active' klasse van alle tabs
        tabs.forEach(t => t.classList.remove('active'));

        // Voeg 'active' klasse toe aan de geselecteerde tab
        tab.classList.add('active');

        // Toon de juiste form en stel de achtergrondkleur in
        if (tab.dataset.tab === "zakelijk") {
            tabsContainer.style.backgroundColor = '#4da1fc'; // Blauw voor Zakelijk
            particulierForm.style.display = 'none';
            zakelijkForm.style.display = 'block';
            medewerkerForm.style.display = 'none';
        } else if (tab.dataset.tab === "medewerker") {
            tabsContainer.style.backgroundColor = '#5cb85c'; // Groen voor Medewerker
            particulierForm.style.display = 'none';
            zakelijkForm.style.display = 'none';
            medewerkerForm.style.display = 'block';
        } else {
            tabsContainer.style.backgroundColor = '#d8a7ff'; // Paars voor Particulier
            particulierForm.style.display = 'block';
            zakelijkForm.style.display = 'none';
            medewerkerForm.style.display = 'none';
        }
    });
});

// Voeg submit event listeners toe aan de forms
const loginButtons = document.querySelectorAll('.login-button');
loginButtons.forEach(button => {
    button.addEventListener('click', function (event) {
        event.preventDefault(); // Voorkom standaard formulierverzending

        // Controleer welke tab actief is en stuur door naar het juiste dashboard
        if (document.querySelector('.tab[data-tab="particulier"]').classList.contains('active')) {
            window.location.href = '/partdashboard'; // Particulier dashboard
        } else if (document.querySelector('.tab[data-tab="zakelijk"]').classList.contains('active')) {
            window.location.href = '/zaakdashboard'; // Zakelijk dashboard
        } else if (document.querySelector('.tab[data-tab="medewerker"]').classList.contains('active')) {
            window.location.href = '/mededashboard'; // Medewerker dashboard
        }
    });
});
