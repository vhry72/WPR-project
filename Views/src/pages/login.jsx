// eslint-disable-next-line no-unused-vars
import React, { useState } from "react";
import "../styles/styles.css";


const Login = () => {
    const [activeTab, setActiveTab] = useState("particulier");

    const tabs = [
        { key: "particulier", label: "Particulier" },
        { key: "zakelijk", label: "Zakelijk" },
        { key: "medewerker", label: "Medewerker" },
    ];

    const renderForm = (tabKey) => (
        <form className="form">
            <label htmlFor={`username-${tabKey}`}>Gebruikersnaam</label>
            <input
                type="text"
                id={`username-${tabKey}`}
                name={`username-${tabKey}`}
                required
            />
            <label htmlFor={`password-${tabKey}`}>Wachtwoord</label>
            <input
                type="password"
                id={`password-${tabKey}`}
                name={`password-${tabKey}`}
                required
            />
            <button type="submit" className="login-button">
                Inloggen
            </button>
        </form>
    );

    return (
        <div className="login-container">
            <h1>Inloggen</h1>
            <div className="tabs">
                {tabs.map((tab) => (
                    <div
                        key={tab.key}
                        className={`tab ${activeTab === tab.key ? "active" : ""}`}
                        onClick={() => setActiveTab(tab.key)}
                    >
                        {tab.label}
                    </div>
                ))}
            </div>
            {renderForm(activeTab)}
        </div>
    );
};

export default Login;
