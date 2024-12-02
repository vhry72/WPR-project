import React, { useState } from "react";
import "./styles.css";

const Login = () => {
    const [activeTab, setActiveTab] = useState("particulier");

    const handleTabClick = (tab) => {
        setActiveTab(tab);
    };

    return (
        <div className="login-container">
            <h1>Inloggen</h1>
            <div className="tabs">
                <div
                    className={`tab ${activeTab === "particulier" ? "active" : ""}`}
                    onClick={() => handleTabClick("particulier")}
                >
                    Particulier
                </div>
                <div
                    className={`tab ${activeTab === "zakelijk" ? "active" : ""}`}
                    onClick={() => handleTabClick("zakelijk")}
                >
                    Zakelijk
                </div>
                <div
                    className={`tab ${activeTab === "medewerker" ? "active" : ""}`}
                    onClick={() => handleTabClick("medewerker")}
                >
                    Medewerker
                </div>
            </div>
            {/* Particulier Formulier */}
            {activeTab === "particulier" && (
                <form className="form">
                    <label htmlFor="username-particulier">Gebruikersnaam</label>
                    <input type="text" id="username-particulier" required />
                    <label htmlFor="password-particulier">Wachtwoord</label>
                    <input type="password" id="password-particulier" required />
                    <button type="submit" className="login-button">
                        Inloggen
                    </button>
                </form>
            )}
            {/* Zakelijk Formulier */}
            {activeTab === "zakelijk" && (
                <form className="form">
                    <label htmlFor="username-zakelijk">Gebruikersnaam</label>
                    <input type="text" id="username-zakelijk" required />
                    <label htmlFor="password-zakelijk">Wachtwoord</label>
                    <input type="password" id="password-zakelijk" required />
                    <button type="submit" className="login-button">
                        Inloggen
                    </button>
                </form>
            )}
            {/* Medewerker Formulier */}
            {activeTab === "medewerker" && (
                <form className="form">
                    <label htmlFor="username-medewerker">Gebruikersnaam</label>
                    <input type="text" id="username-medewerker" required />
                    <label htmlFor="password-medewerker">Wachtwoord</label>
                    <input type="password" id="password-medewerker" required />
                    <button type="submit" className="login-button">
                        Inloggen
                    </button>
                </form>
            )}
        </div>
    );
};

export default Login;
