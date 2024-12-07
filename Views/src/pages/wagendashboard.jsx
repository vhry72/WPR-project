import { Link } from "react-router-dom";
import React, { useState } from "react";
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
                    <Link to="/wagenparklogin" className="btn">
                        Beheer medewerkers
                    </Link>
                    <Link to="/zakelijkverwijder" className="btn">
                         beheer abonnement
                    </Link>
                </div>
            </div>
        </>
    );
};

export default wagendashboard;
