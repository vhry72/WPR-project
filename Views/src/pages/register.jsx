import React, { useState } from "react";
import "./styles.css";

const Register = () => {
    const [activeTab, setActiveTab] = useState("particulier");

    const handleTabClick = (tab) => {
        setActiveTab(tab);
    };

    return (
        <div className="register-container">
            <h1>Registreren</h1>
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
            </div>
            {/* Particulier Formulier */}
            {activeTab === "particulier" && (
                <form id="ParticulierForm" className="form">
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
                    <button type="submit" className="register-button">
                        Registreren
                    </button>
                </form>
            )}
            {/* Zakelijk Formulier */}
            {activeTab === "zakelijk" && (
                <form id="ZakelijkForm" className="form" action="/abonnement" method="get">
                    <label htmlFor="email-zakelijk">E-mail</label>
                    <input type="email" id="email-zakelijk" required />
                    <label htmlFor="wachtwoord-zakelijk">Wachtwoord</label>
                    <input type="password" id="wachtwoord-zakelijk" required />
                    <label htmlFor="bedrijfsnaam">Bedrijfsnaam</label>
                    <input type="text" id="bedrijfsnaam" required />
                    <label htmlFor="adres-zakelijk">Adres</label>
                    <input type="text" id="adres-zakelijk" required />
                    <label htmlFor="kvk-nummer">KVK-nummer</label>
                    <input type="text" id="kvk-nummer" required />
                    <button type="submit" className="register-button">
                        Registreren
                    </button>
                </form>
            )}
        </div>
    );
};

export default Register;
