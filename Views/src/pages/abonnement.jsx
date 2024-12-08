import React, { useState, useEffect } from "react";
import { Link } from "react-router-dom"; 
import "../styles/styles.css";

function Abonnement() {
    const [selectedOption, setSelectedOption] = useState(null);
    const [periodeTotWijziging, setPeriodeTotWijziging] = useState("");

    
    const abonnementOptions = [
        { id: 0, naam: "Maandelijks", prijs: "€10 / maand" },
        { id: 1, naam: "Per kwartaal", prijs: "€25 / kwartaal" },
        { id: 2, naam: "Per jaar", prijs: "€80 / jaar" },
    ];

    
    useEffect(() => {
        if (selectedOption !== null) {
            // Bereken de dagen tot de volgende periode
            const huidigeDatum = new Date();
            const volgendeMaand = new Date(huidigeDatum.getFullYear(), huidigeDatum.getMonth() + 1, 1);
            const verschilInDagen = Math.ceil((volgendeMaand - huidigeDatum) / (1000 * 60 * 60 * 19));
            setPeriodeTotWijziging(`${verschilInDagen} dagen tot de wijziging.`);
        }
    }, [selectedOption]);

    
    const selectOption = (optionIndex) => {
        setSelectedOption(optionIndex);
    };

    return (
        <div className="subscription-container">
            <h1>Kies je Abonnement</h1>
            <div className="subscription-options">
                {abonnementOptions.map((option) => (
                    <div
                        key={option.id}
                        className={`option ${selectedOption === option.id ? "selected" : ""}`}
                        onClick={() => selectOption(option.id)}
                    >
                        {option.naam}
                    </div>
                ))}
            </div>

            
            {selectedOption !== null && (
                <div className="remaining-period">
                    <p>Periode tot wijziging: {periodeTotWijziging}</p>
                </div>
            )}

            {selectedOption !== null && (
                <Link
                    to="/payment"
                    className="subscription-button"
                >
                    {abonnementOptions[selectedOption].prijs} - Bevestigen
                </Link>
            )}
        </div>
    );
}

export default Abonnement;
