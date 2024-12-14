import React from "react";
import { useLocation, useNavigate } from "react-router-dom";
import "../styles/Abonnement.css";

function Betaling() {
    const location = useLocation();
    const navigate = useNavigate();
    const abonnement = location.state?.abonnement;
    const betaalMethode = location.state?.betaalMethode;

    const handleSubmit = (event) => {
        event.preventDefault();
        console.log("Betaling succesvol afgerond.");
        navigate("/medewerkerAbonnementDashboard", { state: { abonnement } });
    };

    return (
        <div className="payment-container">
            <h1>Vul je Betalingsgegevens in</h1>
            <br></br>
            <hr></hr>
            <h3>Abonnement: {abonnement?.naam} - {abonnement?.prijs}</h3>
            <h3>Betaalmethode: {betaalMethode}</h3>
            <form onSubmit={handleSubmit}>
                <div>
                    <label htmlFor="iban">IBAN</label>
                    <input type="text" id="iban" name="iban" required />
                </div>
                <div>
                    <label htmlFor="bank-name">Bank Naam</label>
                    <input type="text" id="bank-name" name="bank-name" required />
                </div>
                <button type="submit" className="payment-button">Betalen</button>
            </form>
        </div>
    );
}

export default Betaling;
