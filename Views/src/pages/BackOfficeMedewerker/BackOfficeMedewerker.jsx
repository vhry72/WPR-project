import React from "react";

import { Link } from "react-router-dom";

const BackOfficeMedewerker = () => {
    console.log("Pagina wordt geladen.");

    return (
        <>
            <div className="index-container">
                <div className="options">
                    <Link to="/BackOfficeVerhuurAanvragen" className="btn">
                        Huur verzoeken
                    </Link>
                    <Link to="/VerhuurGegevens" className="btn">
                        Afgewezen verzoeken
                    </Link>
                    <Link to="/SchadeMeldingen" className="btn">
                        SchadeMeldingen
                    </Link>
                    <Link to="/Schadeclaims" className="btn">
                        Schade claims
                    </Link>
                    <Link to="SchadeClaimMaken" className="btn">
                        Maak schadeClaim
                    </Link>
                    <Link to="/VoertuigTonen" className="btn">
                        Wagenpark zien
                    </Link>
                </div>
            </div>
        </>
    );
};

export default BackOfficeMedewerker;