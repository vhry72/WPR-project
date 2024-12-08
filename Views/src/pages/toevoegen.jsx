import React, { useState } from "react";
import "../styles/styles.css";

const Toevoegen = () => {
    return (
        <div className="Toevoegen-container">
            <h1>Registreer medewerker</h1>
            <form id="medewerkerform" className="form">
                <label htmlFor="email">E-mail</label>
                <input type="email" id="email" required />
                <label htmlFor="voornaam">Voornaam</label>
                <input type="text" id="voornaam" required />
                <label htmlFor="achternaam">Achternaam</label>
                <input type="text" id="achternaam" required />
                <label htmlFor="telefoon">Telefoonnummer</label>
                <input type="tel" id="telefoon" required />
                <label htmlFor="adres">Adres</label>
                <input type="text" id="adres" required />
                <label htmlFor="wachtwoord">Wachtwoord</label>
                <input type="password" id="wachtwoord" required />
                <Link to="/toevoegen" className="btn">
                    voeg medewerkers toe
                </Link>
            </form>
        </div>
    );
};

export default Toevoegen;
