import React, { useState } from "react";
import { useNavigate } from "react-router-dom";
import "../styles/Abonnement.css";

function Abonnement() {
    const navigate = useNavigate();
    const [abonnementOptions] = useState([
        { id: 0, naam: "Standaard Maandelijks", prijs: 10, korting: 0 },
        { id: 1, naam: "Standaard Per kwartaal", prijs: 25, korting: 5 }, // 5% korting
        { id: 2, naam: "Standaard Per jaar", prijs: 80, korting: 10 }, // 10% korting
    ]);

    const handleSelect = (option) => {
        console.log("Selected abonnement:", option); // Debug-log
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
