import { Link, useLocation } from "react-router-dom";
import React, { useState, useEffect } from "react";
import { FaBell } from 'react-icons/fa';
import axios from 'axios';
import "../styles/Notificatie.css"; // Ensure you have your CSS imported
import JwtService from "../services/JwtService";

// IconWithDot Component
export const IconWithDot = ({ showDot }) => {
    return (
        <div className="container">
            <div className="icon">
                <FaBell size={24} />
                {showDot && <div className="red-dot"></div>} {/* Only show red dot when showDot is true */}
            </div>
        </div>
    );
};

// ZakelijkHuurderDashBoard Component
export const ZakelijkHuurderDashBoard = () => {
    const [huurderId, setHuurderId] = useState(null);

    // State to control whether the red dot should be shown
    const [showDot, setShowDot] = useState(false);
    const [error, setError] = useState(null);

    // Haal het huurderID op via de API bij mount
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
        // Check if there are any unanswered requests or notifications
        axios.get(`https://localhost:5033/api/Huurverzoek/check-Beantwoorde/${huurderId}`)
            .then(response => {
                // Assuming the API response indicates if the red dot should be shown
                if (response.data && response.data.showDot) {
                    setShowDot(true);  // Show the red dot if condition is met
                } else {
                    setShowDot(false);  // Hide the red dot otherwise
                }
            })
            .catch(err => {
                setError(err.message);
                console.error("Error fetching data:", err);
            });
    }, [huurderId]); // Run the effect when HuurderId changes

    return (
        <div className="index-container">
            <div className="options">
                <Link to={`/ZakelijkeAutoTonen`} className="btn">
                    Huur Auto
                </Link>
                <Link to={`/accountwijzigingHuurders`} className="btn">
                    Account Wijzigen
                </Link>
                <Link to={`/NotificatieZakelijk`} className="btn">
                    <IconWithDot showDot={showDot} /> {/* Pass the showDot state as a prop */}
                </Link>
            </div>

            {/* Optionally show error if there's an issue */}
            {error && <div className="error-message">Error: {error}</div>}
        </div>
    );
};
export default ZakelijkHuurderDashBoard;
