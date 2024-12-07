import React from "react";
import { useNavigate } from "react-router-dom"; // Gebruik voor navigatie in React Router
import "../styles/styles.css"; 

function Betaling() {
    const navigate = useNavigate();

    const handleSubmit = (event) => {
        event.preventDefault(); // Voorkom standaard formuliergedrag

        // Haal gegevens op uit het formulier
        const formData = new FormData(event.target);
        const data = Object.fromEntries(formData.entries());

        console.log("Betalingsgegevens:", data);

        // Hier kun je een API-aanroep toevoegen om de betaling te verwerken

        // Navigeren naar de "Home" pagina na succesvolle verwerking
        navigate("/Home");
    };

    return (
        <div className="payment-container">
            <h1>Vul je Betalingsgegevens in</h1>
            <p>
                Om je betaling af te ronden, vul de benodigde gegevens in en klik op
                'Betalen'.
            </p>
            <form onSubmit={handleSubmit} className="payment-form">
                <div>
                    <label htmlFor="iban">IBAN</label>
                    <input
                        type="text"
                        id="iban"
                        name="iban"
                        required
                        placeholder="Vul je IBAN in"
                    />
                </div>
                <div>
                    <label htmlFor="bank-name">Bank Naam</label>
                    <input
                        type="text"
                        id="bank-name"
                        name="bank-name"
                        required
                        placeholder="Vul de naam van je bank in"
                    />
                </div>
                <div>
                    <label htmlFor="account-holder">Rekeninghouder</label>
                    <input
                        type="text"
                        id="account-holder"
                        name="account-holder"
                        required
                        placeholder="Vul de naam van de rekeninghouder in"
                    />
                </div>
                <button type="submit" className="payment-button">
                    Betalen
                </button>
            </form>
        </div>
    );
}

export default Betaling;
