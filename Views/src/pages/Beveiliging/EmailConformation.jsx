import { useSearchParams } from "react-router-dom";
import React from "react";
import "../../styles/emailConformation.css";


const EmailConfirmation = () => {
    const [searchParams] = useSearchParams();
    const success = searchParams.get("success") === "true";
    const reason = searchParams.get("reason");

    return (
        <div className="email-confirmation-wrapper">
            <div className="email-confirmation">
                {success ? (
                    <h1 className="success-text">Je e-mailadres is succesvol bevestigd!</h1>
                ) : (
                    <h1 className="error-text">E-mailbevestiging mislukt.</h1>
                )}
                {reason && <p className="reason-text">Reden: {reason}</p>}
            </div>
        </div>
    );
};

export default EmailConfirmation;
