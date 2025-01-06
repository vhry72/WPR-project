import { Link, useLocation } from "react-router-dom";
import React from "react";
import "../styles/styles.css";

const ZakelijkHuurderDashBoard = () => {
    

    return (
        <>
            <div className="index-container">
                <div className="options">
                    <Link to={`/particulierVoertuigTonen`} className="btn">
                        Huur Auto
                    </Link>
                    <Link to={`/accountwijzigingHuurders`} className="btn">
                        Account Wijzigen 
                    </Link>
                </div>
            </div>
        </>
    );
};

export default ZakelijkHuurderDashBoard;
