import React, { useState, useEffect } from "react";
import { useNavigate } from "react-router-dom";
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

    // Haal het beheerder ID op bij het laden van de component
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

    // Selecteer een abonnement en navigeer naar de bedrijfsabonnementpagina
    const handleSelect = (option) => {
        navigate(`/bedrijfsabonnement`, { state: { selectedAbonnement: option } });
    };

    return (
        <div className="subscription-container">
            <h1>Kies je Abonnement</h1>
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
