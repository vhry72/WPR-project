import React from "react";

import { Link } from "react-router-dom";

const BackOfficeMedewerker = () => {
    console.log("Pagina wordt geladen.");

    return (
        <>
            <div className="index-container">
                <div className="options">
                    <Link to="/BackOfficeVerhuurAanvragen" className="btn">
                        aanvragen
                    </Link>
                    <Link to="/VerhuurGegevens" className="btn">
                        VerhuurGegevens
                    </Link>
                    <Link to="/SchadeMeldingen" className="btn">
                        SchadeMeldingen
                    </Link>
                </div>
            </div>
        </>
    );
};

export default BackOfficeMedewerker;


