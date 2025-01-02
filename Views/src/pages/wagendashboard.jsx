import { Link, useNavigate } from "react-router-dom";
import React from "react";
import axios from "axios";
import "../styles/styles.css";

const wagendashboard = () => {
    const navigate = useNavigate();

    const handleBeheerAbonnement = async () => {
        try {
            const response = await axios.get("/api/abonnement/check"); // Controleer abonnement
            if (response.data.hasAbonnement) {
                navigate("/medewerkerAbonnementDashboard", {
                    state: { abonnementId: response.data.abonnementId },
                });
            } else {
                navigate("/abonnement");
            }
        } catch (error) {
            console.error("Fout bij het controleren van het abonnement:", error);
            alert("Er is een fout opgetreden bij het controleren van het abonnement.");
        }
    };

    return (
        <div className="index-container">
            <div className="options">
                <Link to="/wagenbeheer" className="btn">
                    Beheer medewerkers
                </Link>
                <button onClick={handleBeheerAbonnement} className="btn">
                    Beheer abonnement
                </button>
            </div>
        </div>
    );
};

export default wagendashboard;
