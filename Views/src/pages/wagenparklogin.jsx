// eslint-disable-next-line no-unused-vars
import React from "react";
import "./styles.css";

const wagenparkLogin = () => {
    return (
        <div className="login-container">
            <h1>Wagenpark Login</h1>
            <form id="wagenpark" className="form" action="/wagendashboard" method="get">
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
                <button type="submit" className="login-button">
                    Inloggen
                </button>
            </form>
        </div>
    );
};

export default wagenparkLogin;
