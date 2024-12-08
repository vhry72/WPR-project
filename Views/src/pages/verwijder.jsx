import React, { useState } from "react";
import { Link } from "react-router-dom"; // Gebruik React Router voor navigatie
import "../styles/styles.css";

const Verwijderen = () => {
    return (
        <div className="Verwijderen-container">
            <h1>verwijder medewerker</h1>
            <form id="medewerkerform" className="form">
                <label htmlFor="email">E-mail</label>
                <input type="email" id="email" required />
                <label htmlFor="wachtwoord">Wachtwoord</label>
                <input type="password" id="wachtwoord" required />
                <Link to="/wagenbeheer" className="btn">
                    Verwijder medewerker
                </Link>
            </form>
        </div>
    );
};

export default Verwijderen;
