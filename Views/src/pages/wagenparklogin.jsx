// eslint-disable-next-line no-unused-vars
import React from "react";
import "../styles/styles.css";
import { useNavigate } from "react-router-dom";

function wagenparkLogin() {
    const navigate = useNavigate();

    const handleLogin = (event) => {
        event.preventDefault(); // Voorkomt dat de pagina opnieuw wordt geladen
        // Hier kun je login-logica toevoegen (zoals validatie, API-aanroepen, etc.)
        navigate("/wagendashboard");
    };

    return (
        <div className="login-container">
            <h1>Wagenpark Login</h1>
            <form id="wagenpark" className="form">
                <label htmlFor="username">Gebruikersnaam</label>
                <input
                    type="text"
                    id="username"
                    name="username"
                    required
                />
                <label htmlFor="password">Wachtwoord</label>
                <input
                    type="password"
                    id="password"
                    name="password"
                    required
                />
                <button onClick={handleLogin} type="button" className="login-button">
                    Inloggen
                </button>
            </form>
        </div>
    );
}

export default wagenparkLogin;

