import React, { useState } from "react";
import { useLocation } from "react-router-dom";
import "../styles/Abonnement.css";

function MedewerkersDashboard() {
    const location = useLocation();
    const abonnement = location.state?.abonnement;
    const [medewerkers, setMedewerkers] = useState([]);
    const [nieuweMedewerker, setNieuweMedewerker] = useState({ naam: "", email: "" });
    const [toegevoegdAanAbonnement, setToegevoegdAanAbonnement] = useState([]);

    const voegMedewerkerToe = () => {
        if (!nieuweMedewerker.naam || !nieuweMedewerker.email) {
            alert("Voer een naam en e-mailadres in.");
            return;
        }
        setMedewerkers([...medewerkers, nieuweMedewerker]);
        setNieuweMedewerker({ naam: "", email: "" });
    };

    const verwijderMedewerker = (index) => {
        const updatedMedewerkers = medewerkers.filter((_, i) => i !== index);
        setMedewerkers(updatedMedewerkers);
    };

    const voegToeAanAbonnement = (index) => {
        const medewerker = medewerkers[index];
        if (toegevoegdAanAbonnement.some((m) => m.email === medewerker.email)) {
            alert(`${medewerker.naam} is al toegevoegd aan het abonnement.`);
            return;
        }
        setToegevoegdAanAbonnement([...toegevoegdAanAbonnement, medewerker]);
        alert(`${medewerker.naam} is toegevoegd aan het abonnement.`);
    };

    return (
        <div className="medewerkers-dashboard">
            <h1>Medewerkers Toevoegen</h1>
            <br></br>
            <h3>Gekozen Abonnement: {abonnement?.naam} - {abonnement?.prijs}</h3>
            <div>
                <input
                    type="text"
                    placeholder="Naam"
                    value={nieuweMedewerker.naam}
                    onChange={(e) => setNieuweMedewerker({ ...nieuweMedewerker, naam: e.target.value })}
                />
                <input
                    type="email"
                    placeholder="E-mail"
                    value={nieuweMedewerker.email}
                    onChange={(e) => setNieuweMedewerker({ ...nieuweMedewerker, email: e.target.value })}
                />
                <button onClick={voegMedewerkerToe}>Toevoegen</button>
            </div>
            <h3>Huidige Medewerkers</h3>
            <ul>
                {medewerkers.map((medewerker, index) => (
                    <li key={index}>
                        {medewerker.naam} ({medewerker.email})
                        <button className="verwijder-knop" onClick={() => verwijderMedewerker(index)}>
                            Verwijderen
                        </button>
                        <button className="toevoegen-knop" onClick={() => voegToeAanAbonnement(index)}>
                            Toevoegen aan Abonnement
                        </button>
                    </li>
                ))}
            </ul>
            <h3>Medewerkers in Abonnement</h3>
            <ul>
                {toegevoegdAanAbonnement.map((medewerker, index) => (
                    <li key={index}>{medewerker.naam} ({medewerker.email})</li>
                ))}
            </ul>
        </div>
    );
}

export default MedewerkersDashboard;
