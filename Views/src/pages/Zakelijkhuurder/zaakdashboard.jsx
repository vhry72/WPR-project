import { Link } from "react-router-dom";
import React, { useState } from "react";
import "../../styles/styles.css";

const zaakdashboard = () => {
    console.log("zaakdashboard component wordt gerenderd");

    return (
        <>
            <div className="index-container">
                <div className="options">
                    <Link to="/zakelijkeautotonen" className="btn">
                        Huur Auto
                    </Link>

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
