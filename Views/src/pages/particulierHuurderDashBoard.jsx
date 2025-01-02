import { Link, useLocation } from "react-router-dom";
import React from "react";
import "../styles/styles.css";

const ZakelijkHuurderDashBoard = () => {
    const location = useLocation();
    const HuurderId = new URLSearchParams(location.search).get("HuurderID");

    return (
        <>
            <div className="index-container">
                <div className="options">
                    <Link to={`/particulierVoertuigTonen?HuurderID=${HuurderId}`} className="btn">
                        Huur Auto
                    </Link>
                    <Link to={`/accountwijzigingHuurders?HuurderID=${HuurderId}`} className="btn">
                        Account Wijzigen 
                    </Link>
                </div>
            </div>
        </>
    );
};

export default ZakelijkHuurderDashBoard;
