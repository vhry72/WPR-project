import React from "react";

import { Link } from "react-router-dom";

const FrontOfficeMedewerker = () => {
    console.log("Pagina wordt geladen.");

    return (
        <>
            <div className="index-container">
                <div className="options">
                    <Link to="/VoertuigDetails" className="btn">
                       VoertuigGegevens
                    </Link> 
                </div>
            </div>
        </>
    );
};

export default FrontOfficeMedewerker;
