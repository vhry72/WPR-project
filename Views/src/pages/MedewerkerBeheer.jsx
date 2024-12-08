import React, { useState } from "react";

function MedewerkerBeheer() {
    const [medewerkers, setMedewerkers] = useState([]);
    const [nieuweMedewerker, setNieuweMedewerker] = useState({
        naam: "",
        email: "",
        wachtwoord: "",
    });

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
        setNieuweMedewerker({ naam: "", email: "", wachtwoord: "" });
    };

    const verwijderMedewerker = (email) => {
        setMedewerkers((prev) => prev.filter((m) => m.email !== email));
    };

    return (
        <div>
            <h2>Medewerker Beheer</h2>
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
                    placeholder="E-mailadres"
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
    );
}

export default MedewerkerBeheer;
