import { Link, useLocation } from "react-router-dom";
import React, { useState, useEffect } from "react";
import { FaBell } from 'react-icons/fa';
import axios from 'axios';
import "../styles/Notificatie.css"; // Ensure you have your CSS imported

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
    const location = useLocation();
    const HuurderId = new URLSearchParams(location.search).get("HuurderID");

    // State to control whether the red dot should be shown
    const [showDot, setShowDot] = useState(false);
    const [error, setError] = useState(null);

    useEffect(() => {
        // Check if there are any unanswered requests or notifications
        axios.get(`https://localhost:5033/api/Huurverzoek/check-Beantwoorde/${HuurderId}`)
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
    }, [HuurderId]); // Run the effect when HuurderId changes

    return (
        <div className="index-container">
            <div className="options">
                <Link to={`/ZakelijkeAutoTonen?HuurderID=${HuurderId}`} className="btn">
                    Huur Auto
                </Link>
                <Link to={`/accountwijzigingHuurders?HuurderID=${HuurderId}`} className="btn">
                    Account Wijzigen
                </Link>
                <Link to={`/NotificatieZakelijk?HuurderID=${HuurderId}`} className="btn">
                    <IconWithDot showDot={showDot} /> {/* Pass the showDot state as a prop */}
                </Link>
            </div>

            {/* Optionally show error if there's an issue */}
            {error && <div className="error-message">Error: {error}</div>}
        </div>
    );
};
export default ZakelijkHuurderDashBoard;
