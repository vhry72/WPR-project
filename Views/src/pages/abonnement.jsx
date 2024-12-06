import React, { useState } from "react";
import { Link } from "react-router-dom"; // Gebruik React Router voor navigatie
import "./styles.css"; // Zorg ervoor dat dit pad correct is

function Abonnement() {
    const [selectedOption, setSelectedOption] = useState(null);

    // Functie om een optie te selecteren
    const selectOption = (optionIndex) => {
        setSelectedOption(optionIndex);
    };

    return (
        <div className="subscription-container">
            <h1>Kies je Abonnement</h1>
            <div className="subscription-options">
                <div
                    className={`option ${selectedOption === 0 ? "selected" : ""}`}
                    id="monthly"
                    onClick={() => selectOption(0)}
                >
                    Maandelijks
                </div>
                <div
                    className={`option ${selectedOption === 1 ? "selected" : ""}`}
                    id="quarterly"
                    onClick={() => selectOption(1)}
                >
                    Per kwartaal
                </div>
                <div
                    className={`option ${selectedOption === 2 ? "selected" : ""}`}
                    id="yearly"
                    onClick={() => selectOption(2)}
                >
                    Per jaar
                </div>
            </div>
            {/* Bevestigingsknop die naar de betalingen leidt */}
            <Link
                to="/payment"
                className="subscription-button"
                id="confirm-btn"
            >
                €10 / maand - Bevestigen
            </Link>
        </div>
    );
}

export default Abonnement;
