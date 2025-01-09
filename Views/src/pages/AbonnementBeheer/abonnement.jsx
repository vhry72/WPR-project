import React, { useState, useEffect } from "react";
import { useNavigate } from "react-router-dom";
import AbonnementService from "../../services/requests/AbonnementService";
import "../../styles/Abonnement.css";

function Abonnement() {
    const navigate = useNavigate();
    const [abonnementOptions] = useState([
        { id: 0, naam: "Standaard Maandelijks", prijs: 10, korting: 0 },
        { id: 1, naam: "Standaard Per kwartaal", prijs: 25, korting: 5 },
        { id: 2, naam: "Standaard Per jaar", prijs: 80, korting: 10 },
    ]);
    const [huidigAbonnement, setHuidigAbonnement] = useState(null);
    const [huurderId, setHuurderId] = useState(null);

    useEffect(() => {
        const fetchUserId = async () => {
            try {
                const userId = await JwtService.getUserId(); // Haal de gebruikers-ID op via de API
                if (userId) {
                    setHuurderId(userId);
                } else {
                    console.error("Huurder ID kon niet worden opgehaald via de API.");
                }
            } catch (error) {
                console.error("Fout bij het ophalen van de huurder ID:", error);
            }
        };

        fetchUserId();
    }, []);

    useEffect(() => {
        const fetchAbonnement = async () => {
            try {
                const response = await AbonnementService.getById(huurderId); // Vervang met de juiste ID
                setHuidigAbonnement(response.data);
            } catch (error) {
                console.error("Geen huidig abonnement gevonden.");
            }
        };
        fetchAbonnement();
    }, []);

    const handleSelect = (option) => {
        if (huidigAbonnement) {
            navigate(`/bedrijfsabonnement`, { state: { wijzigAbonnement: option } });
        } else {
            navigate(`/bedrijfsabonnement`, { state: { selectedAbonnement: option } });
        }
    };

    return (
        <div className="subscription-container">
            <h1>{huidigAbonnement ? "Wijzig je Abonnement" : "Kies je Abonnement"}</h1>
            {huidigAbonnement && (
                <p>Huidig abonnement: {huidigAbonnement.naam} - €{huidigAbonnement.prijs}</p>
            )}
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
