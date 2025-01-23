import React, { useState, useEffect } from "react";
import { useLocation } from "react-router-dom";
import axios from "axios";
import JwtService from "../../services/JwtService";
import "../../styles/Abonnement.css";

const API_URL = "https://localhost:5033/api/Abonnement";

function BedrijfsAbonnement() {
    const location = useLocation();
    const [betaalMethode, setBetaalMethode] = useState(null);
    const [factuurVerstuurd, setFactuurVerstuurd] = useState(false);
    const [beheerderId, setBeheerderId] = useState(null);
    const [zakelijkeId, setZakelijkeId] = useState(null); // Correct gebruik van useState
    const [isLoading, setIsLoading] = useState(false);
    const [error, setError] = useState(null);

    const abonnement = location.state?.selectedAbonnement || location.state?.wijzigAbonnement;

    // Fetch Id van API
    useEffect(() => {
        const fetchBeheerderId = async () => {
            try {
                const userId = await JwtService.getUserId();
                if (userId) {
                    setBeheerderId(userId);
                } else {
                    console.error("Beheerder ID kon niet worden opgehaald via de API.");
                }
            } catch (error) {
                console.error("Fout bij het ophalen van de beheerder ID:", error);
            }
        };

        fetchBeheerderId();
    }, []);

    useEffect(() => {
        const fetchZakelijkeId = async () => {
            setIsLoading(true);
            setError(null);

            try {
                const response = await axios.get(
                    `https://localhost:5033/api/WagenparkBeheerder/${beheerderId}/zakelijkeId`
                );
                setZakelijkeId(response.data.zakelijkeId); //  zakelijkeId moet overeenkomen voor APi response
            } catch (error) {
                console.error("Fout bij het ophalen van de zakelijke ID:", error);
                setError("Kan zakelijke ID niet ophalen. Controleer de API of netwerkverbinding.");
            } finally {
                setIsLoading(false);
            }
        };

        if (beheerderId) {
            fetchZakelijkeId();
        }
    }, [beheerderId]);

    const handleFactuur = async () => {
        if (!betaalMethode) {
            alert("Selecteer een betaalmethode.");
            return;
        }

        // Controleer of alle gegevens beschikbaar zijn
        if (!abonnement || !zakelijkeId || !beheerderId) {
            alert("Alle gegevens zijn niet beschikbaar. Controleer of alles correct is geladen.");
            return;
        }

        try {
            const payload = {
                naam: abonnement.naam,
                kosten: abonnement.prijs,
                zakelijkeId: zakelijkeId,
                type: betaalMethode,
                abonnementTermijnen: abonnement.termijn,
                korting: abonnement.korting,
            };

            // Verstuur de factuur naar de API
            console.log(payload);
            await axios.post(`${API_URL}/${beheerderId}/abonnement/maken`, payload);
            setFactuurVerstuurd(true);
        } catch (error) {
            console.error("Fout bij het versturen van de factuur:", error);
            alert("Er is een fout opgetreden bij het versturen van de factuur.");
        }
    };

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
            <div className="abonnement-type-selector">
                <h3>Kies een betaalmethode:</h3>
                <button
                    className={`abonnement-button ${betaalMethode === "PayAsYouGo" ? "selected" : ""
                        }`}
                    onClick={() => setBetaalMethode("PayAsYouGo")}
                >
                    PayAsYouGo
                </button>
                <button
                    className={`abonnement-button ${betaalMethode === "PrepaidSaldo" ? "selected" : ""
                        }`}
                    onClick={() => setBetaalMethode("PrepaidSaldo")}
                >
                    PrepaidSaldo
                </button>
            </div>
            {error && <p className="error-message">{error}</p>}
            <button onClick={handleFactuur} className="create-subscription-button" disabled={isLoading}>
                {isLoading ? "Laden..." : "Activeer Abonnement"}
            </button>
        </div>
    );
}

export default BedrijfsAbonnement;
