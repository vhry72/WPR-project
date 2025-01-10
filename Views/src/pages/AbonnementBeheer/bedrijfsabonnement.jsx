import React, { useState } from "react";
import { useLocation } from "react-router-dom";
import axios from "axios";
import "../../styles/Abonnement.css";

const API_URL = 'https://localhost:5033/api/Abonnement'; // Pas dit aan naar jouw API-base-URL

function BedrijfsAbonnement() {
    const location = useLocation();
    const [betaalMethode, setBetaalMethode] = useState(null);
    const [factuurVerstuurd, setFactuurVerstuurd] = useState(false);
    const abonnement = location.state?.selectedAbonnement;

    // Functie om korting te berekenen
    const berekenKorting = (prijs, korting) => {
        return prijs - (prijs * korting) / 100;
    };

    // Verwerk factuur en stuur deze naar de backend
    const handleFactuur = async () => {
        if (!betaalMethode) {
            alert("Selecteer een betaalmethode.");
            return;
        }

        try {
            // API-aanroep naar backend om factuur te versturen
            await axios.post(`${API_URL}/${abonnement.beheerderId}/factuur/stuur`, {
                betaalMethode,
            });
            setFactuurVerstuurd(true);
        } catch (error) {
            console.error("Fout bij het versturen van de factuur:", error);
            alert("Er is een fout opgetreden bij het versturen van de factuur.");
        }
    };

    // Weergave wanneer factuur is verstuurd
    if (factuurVerstuurd) {
        return (
            <div className="bedrijf-abonnement-container">
                <h1>Factuur Verstuurd!</h1>
                <p>De factuur is succesvol naar je e-mailadres gestuurd.</p>
            </div>
        );
    }

    return (
        <div className="bedrijf-abonnement-container">
            <h1>Maak een Bedrijfsabonnement</h1>
            <h3>
                Geselecteerd abonnement: {abonnement?.naam} - €{abonnement?.prijs}{" "}
                {abonnement?.korting > 0 && (
                    <span>(Korting: {abonnement?.korting}%)</span>
                )}
            </h3>
            <h3>
                Effectieve prijs na korting: €
                {berekenKorting(abonnement?.prijs, abonnement?.korting).toFixed(2)}
            </h3>
            <div className="abonnement-type-selector">
                <h3>Kies een betaalmethode:</h3>
                <button
                    className={`abonnement-button ${betaalMethode === "pay-as-you-go" ? "selected" : ""
                        }`}
                    onClick={() => setBetaalMethode("pay-as-you-go")}
                >
                    Pay-as-you-go
                </button>
                <button
                    className={`abonnement-button ${betaalMethode === "prepaid" ? "selected" : ""
                        }`}
                    onClick={() => setBetaalMethode("prepaid")}
                >
                    Prepaid
                </button>
            </div>
            <button onClick={handleFactuur} className="create-subscription-button">
                Stuur Factuur
            </button>
        </div>
    );
}

export default BedrijfsAbonnement;
