import React, { useState } from "react";
import { Link } from "react-router-dom"; // Gebruik React Router voor navigatie
import "../styles/styles.css";

const wagendashboard = () => {
    console.log("wagendashboard component wordt gerenderd");

    return (
        <>
            <header>
                <h1 className="H1Tekst-Index"></h1>
            </header>
            <div className="index-container">
                <div className="options">
                    <Link to="/toevoegen" className="btn">
                        voeg medewerkers toe
                    </Link>
                    <Link to="/verwijderen" className="btn">
                        verwijder medewerkers
                    </Link>

                </div>
            </div>
        </>
    );
};

export default wagendashboard;
