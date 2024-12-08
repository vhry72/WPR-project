import React, { useState } from "react";
import { Link } from "react-router-dom";
import "../styles/styles.css";
import "../styles/Wagenparkbeheerder.css";
import "../styles/Notificatie.css";

const WagenbeheerDashboard = () => {
    console.log("wagendashboard component wordt gerenderd");

    const [medewerkers, setMedewerkers] = useState([]);
    const [nieuweMedewerker, setNieuweMedewerker] = useState({
        naam: "",
        email: "",
        wachtwoord: "",
    });
    const [notificatie, setNotificatie] = useState("");

    const handleInputChange = (e) => {
        const { name, value } = e.target;
        setNieuweMedewerker((prev) => ({ ...prev, [name]: value }));
    };

    const voegMedewerkerToe = () => {
        if (!nieuweMedewerker.email.endsWith("@bedrijf.nl")) {
            alert("Email moet een bedrijfs-e-mailadres zijn.");
            return;
        }
        setMedewerkers((prev) => [...prev, nieuweMedewerker]);
        setNotificatie(`Toegevoegd: ${nieuweMedewerker.naam}`);
        setTimeout(() => setNotificatie(""), 5000); // Verwijdert notificatie na 3 seconden
        setNieuweMedewerker({ naam: "", email: "", wachtwoord: "" });
    };

    const verwijderMedewerker = (email) => {
        const medewerker = medewerkers.find((m) => m.email === email);
        setMedewerkers((prev) => prev.filter((m) => m.email !== email));
        setNotificatie(`Verwijderd: ${medewerker.naam}`);
        setTimeout(() => setNotificatie(""), 5000); // Verwijdert notificatie na 3 seconden
    };

    return (
        <>
            <header>
                <h1 className="H1Tekst-Index">Wagenparkbeheerder Dashboard</h1>
            </header>
            <div className="index-container">
                <div className="options">
                    <h2>Medewerkerbeheer</h2>
                    <div>
                        <h3>Voeg een nieuwe medewerker toe</h3>
                        <input
                            type="text"
                            name="naam"
                            value={nieuweMedewerker.naam}
                            onChange={handleInputChange}
                            placeholder="Naam"
                        />
                        <input
                            type="email"
                            name="email"
                            value={nieuweMedewerker.email}
                            onChange={handleInputChange}
                            placeholder="Bedrijfs-E-mailadres"
                        />
                        <input
                            type="password"
                            name="wachtwoord"
                            value={nieuweMedewerker.wachtwoord}
                            onChange={handleInputChange}
                            placeholder="Wachtwoord"
                        />
                        <button onClick={voegMedewerkerToe}>Toevoegen</button>
                    </div>
                    <h3>Huidige Medewerkers</h3>
                    <ul>
                        {medewerkers.map((medewerker, index) => (
                            <li key={index}>
                                {medewerker.naam} - {medewerker.email}
                                <button onClick={() => verwijderMedewerker(medewerker.email)}>
                                    Verwijderen
                                </button>
                            </li>
                        ))}
                    </ul>
                </div>
                {notificatie && (
                    <div className="notificatie-box">
                        {notificatie}
                    </div>
                )}
            </div>
        </>
    );
};

export default WagenbeheerDashboard;
