import { Link } from "react-router-dom";
import React, { useState } from "react";
import "../styles/styles.css";

const zaakdashboard = () => {
    console.log("zaakdashboard component wordt gerenderd");

    return (
        <>
            <header>
                <h1 className="H1Tekst-Index"></h1>
            </header>
            <div className="index-container">
                <div className="options">
                    <Link to="/wagenparklogin" className="btn">
                        Beheerder login
                    </Link>
                    <Link to="/zakelijkverwijder" className="btn">
                        Verwijder bedrijf
                    </Link>
                </div>
            </div>
        </>
    );
};

export default zaakdashboard;
