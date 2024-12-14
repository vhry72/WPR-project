import { Link } from "react-router-dom";
import React, { useState } from "react";
import "../styles/styles.css";

const wagendashboard = () => {
    console.log("wagendashboard component wordt gerenderd");

    return (
        <>
            <div className="index-container">
                <div className="options">
                    <Link to="/wagenbeheer" className="btn">
                        Beheer medewerkers
                    </Link>
                    <Link to="/abbonementupdate" className="btn">
                         beheer abonnement
                    </Link>
                </div>
            </div>
        </>
    );
};

export default wagendashboard;
