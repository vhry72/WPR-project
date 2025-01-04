import { Link, useLocation } from "react-router-dom";
import React, { useState, useEffect } from "react";
import "../styles/styles.css";
//import { FaBell } from 'react-icons/fa';
import AbonnementService from "../services/requests/AbonnementService";  // Zorg ervoor dat deze geïmporteerd is

export const ZakelijkHuurderDashBoard = () => {
    const location = useLocation();
    const HuurderId = new URLSearchParams(location.search).get("HuurderID");

    // Gebruik state om notificaties bij te houden
    const [hasNotifications, setHasNotifications] = useState(false);  // <-- Dit is de declaratie van de state

    useEffect(() => {
        const fetchAbonnementen = async () => {
            try {
                // Haal de abonnementen op via de API
                const response = await AbonnementService.getBijnaVerlopen();
                console.log(hasNotifications);

                // Controleer of er abonnementen zijn die bijna verlopen
                if (response.data && Array.isArray(response.data) && response.data.length > 0) {
                    // Stel in dat er notificaties zijn als er abonnementen bijna verlopen
                    setHasNotifications(true);
                } else {
                    setHasNotifications(false);  // Geen notificaties als er geen abonnementen zijn
                }
            } catch (error) {
                console.error("Fout bij het ophalen van abonnementen:", error);
                setHasNotifications(false);  // Zorg ervoor dat notificaties niet worden weergegeven bij een fout
            }
        };
        
        fetchAbonnementen();  // Roep de functie aan bij de eerste render
    }, []); // De useEffect wordt nu alleen uitgevoerd bij de eerste render

    return (
        <>
            <div className="index-container">
                <div className="options">
                    <Link to={`/ZakelijkeAutoTonen?HuurderID=${HuurderId}`} className="btn">
                        Huur Auto
                    </Link>
                    <Link to={`/accountwijzigingHuurders?HuurderID=${HuurderId}`} className="btn">
                        Account Wijzigen
                    </Link>
                    <Link to={`/NotificatieZakelijk?HuurderID=${HuurderId}`} className="btn">
                        <FaBell size={24} />
                        {hasNotifications && <span className="notification-badge"></span>}
                    </Link>
                </div>
            </div>
        </>
    );
};

export default ZakelijkHuurderDashBoard;

