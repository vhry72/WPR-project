import React, { useState, useEffect } from "react";
import { useLocation } from "react-router-dom";
import "../styles/Abonnement.css";

function MedewerkersDashboard() {
    const location = useLocation();
    const abonnement = location.state?.abonnement;
    const [medewerkers, setMedewerkers] = useState([]);
    const [toegevoegdAanAbonnement, setToegevoegdAanAbonnement] = useState([]);
    const [notificatie, setNotificatie] = useState("");

    // Haal medewerkers op bij het laden van de pagina
    useEffect(() => {
        const fetchMedewerkers = async () => {
            try {
                const response = await fetch("/api/medewerkers"); // Simuleer een API-endpoint
                const data = await response.json();
                setMedewerkers(data);
            } catch (error) {
                setNotificatie("Kon medewerkers niet ophalen.");
                setTimeout(() => setNotificatie(""), 3000);
            }
        };

        fetchMedewerkers();
    }, []);

    const voegToeAanAbonnement = (index) => {
        const medewerker = medewerkers[index];
        if (toegevoegdAanAbonnement.some((m) => m.email === medewerker.email)) {
            alert(`${medewerker.naam} is al toegevoegd aan het abonnement.`);
            return;
        }
        setToegevoegdAanAbonnement([...toegevoegdAanAbonnement, medewerker]);
        alert(`${medewerker.naam} is toegevoegd aan het abonnement.`);
    };

    const verwijderUitAbonnement = (index) => {
        const updatedList = toegevoegdAanAbonnement.filter((_, i) => i !== index);
        setToegevoegdAanAbonnement(updatedList);
        alert("Medewerker verwijderd uit het abonnement.");
    };

    return (
        <div className="medewerkers-dashboard">
            <h1>Medewerkers Toevoegen</h1>
            <br />
            <h3>Gekozen Abonnement: {abonnement?.naam} - €{abonnement?.prijs}</h3>
            <h3>Beschikbare Medewerkers</h3>
            <ul>
                {medewerkers.map((medewerker, index) => (
                    <li key={index}>
                        {medewerker.naam} ({medewerker.email})
                        <button
                            className="toevoegen-knop"
                            onClick={() => voegToeAanAbonnement(index)}
                        >
                            Toevoegen aan Abonnement
                        </button>
                    </li>
                ))}
            </ul>

            <h3>Medewerkers in Abonnement</h3>
            <ul>
                {toegevoegdAanAbonnement.map((medewerker, index) => (
                    <li key={index}>
                        {medewerker.naam} ({medewerker.email})
                        <button
                            className="verwijder-knop"
                            onClick={() => verwijderUitAbonnement(index)}
                        >
                            Verwijderen
                        </button>
                    </li>
                ))}
            </ul>

            {notificatie && <div className="notificatie-box">{notificatie}</div>}
        </div>
    );
}

export default MedewerkersDashboard;
