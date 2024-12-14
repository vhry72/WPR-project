import React, { useState } from "react";
import { useLocation, useNavigate } from "react-router-dom";
import "../styles/Abonnement.css";

function BedrijfsAbonnement() {
    const location = useLocation();
    const navigate = useNavigate();
    const [betaalMethode, setBetaalMethode] = useState(null);
    const abonnement = location.state?.selectedAbonnement;

    const handleBetaling = () => {
        if (!betaalMethode) {
            alert("Selecteer een betaalmethode.");
            return;
        }
        console.log("Selected betaalmethode:", betaalMethode); // Debug-log
        navigate("/payment", { state: { abonnement, betaalMethode } });
    };

    return (
        <div className="bedrijf-abonnement-container">
            <h1>Maak een Bedrijfsabonnement</h1>
            <h3>Geselecteerd abonnement: {abonnement?.naam} - {abonnement?.prijs}</h3>
            <div className="abonnement-type-selector">
                <h3>Kies een betaalmethode:</h3>
                <button
                    className={`abonnement-button ${betaalMethode === "pay-as-you-go" ? "selected" : ""}`}
                    onClick={() => setBetaalMethode("pay-as-you-go")}
                >
                    Pay-as-you-go
                </button>
                <button
                    className={`abonnement-button ${betaalMethode === "prepaid" ? "selected" : ""}`}
                    onClick={() => setBetaalMethode("prepaid")}
                >
                    Prepaid
                </button>
            </div>
            <button onClick={handleBetaling} className="create-subscription-button">
                Doorgaan naar Betaling
            </button>
        </div>
    );
}

export default BedrijfsAbonnement;
