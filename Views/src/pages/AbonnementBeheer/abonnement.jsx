import React, { useState, useEffect } from "react";
import { useNavigate } from "react-router-dom";
import axios from "axios";
import JwtService from "../../services/JwtService";
import "../../styles/Abonnement.css";

function Abonnement() {
    const navigate = useNavigate();
    const [abonnementOptions] = useState([
        { id: 0, naam: "Standaard Maandelijks", prijs: 10, korting: 0, termijn: "Maandelijks" },
        { id: 1, naam: "Standaard Per kwartaal", prijs: 25, korting: 5, termijn: "Kwartaal" },
        { id: 2, naam: "Standaard Per jaar", prijs: 80, korting: 10, termijn: "Jaarlijks" },
    ]);
    const [beheerderId, setBeheerderId] = useState(null);
    const [abonnementId, setAbonnementId] = useState(null);
    const [error, setError] = useState(null);

    // Fetch beheerder Id van API
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

    // Fetch abonnement Id van API
    const fetchAbonnementId = async () => {
        try {
            const response = await axios.get(
                `https://localhost:5033/api/WagenparkBeheerder/${beheerderId}/AbonnementId`, { withCredentials: true });
            setAbonnementId(response.data.abonnementId);
            return response.data.abonnementId;
        } catch (error) {
            if (error.response && error.response.status === 404) {
                console.warn("Abonnement ID niet gevonden.");
                return null; 
            } else {
                console.error("Fout bij het ophalen van de Abonnement ID:", error);
                setError("Kan Abonnement ID niet ophalen. Controleer de API of netwerkverbinding.");
                return undefined; 
            }
        }
    };


    const handleSelect = async (option) => {
        if (!beheerderId) {
            alert("Beheerder ID is niet beschikbaar.");
            return;
        }

        const abonnementId = await fetchAbonnementId();

        if (abonnementId === null) {
            navigate(`/bedrijfsabonnement`, { state: { selectedAbonnement: option } });
        } else if (abonnementId) {
            navigate(`/WijzigBedrijfsAbonnement`, { state: { selectedAbonnement: option } });
        } else {
            alert("Er is een fout opgetreden. Probeer het opnieuw.");
        }
    };

    return (
        <div className="subscription-container">
            <h1>Kies je Abonnement</h1>
            {error && <p className="error-message">{error}</p>}
            <div className="subscription-options">
                {abonnementOptions.map((option) => (
                    <div
                        key={option.id}
                        className="option"
                        onClick={() => handleSelect(option)}
                    >
                        {option.naam} - €{option.prijs}
                        {option.korting > 0 && (
                            <span> (Korting: {option.korting}%)</span>
                        )}
                    </div>
                ))}
            </div>
        </div>
    );
}

export default Abonnement;