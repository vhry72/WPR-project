import React, { useState, useEffect } from "react";
import axios from "axios";
import "../styles/Abonnement.css";

function PrepaidSaldo() {
    const [saldo, setSaldo] = useState(0);
    const [beheerderId, setBeheerderId] = useState(null);
    const [bedrag, setBedrag] = useState(0);

    useEffect(() => {
        const fetchUserId = async () => {
            try {
                const userId = await JwtService.getUserId(); // Haal de gebruikers-ID op via de API
                if (userId) {
                    setBeheerderId(userId);
                } else {
                    console.error("Huurder ID kon niet worden opgehaald via de API.");
                }
            } catch (error) {
                console.error("Fout bij het ophalen van de huurder ID:", error);
            }
        };

        fetchUserId();
    }, []);

    // haal de saldo op van de beheerder
    useEffect(() => {
        const fetchSaldo = async () => {
            try {
                const response = await axios.get(`/api/abonnement/${beheerderId}/huidig-abonnement`);
                setSaldo(response.data.prepaidSaldo);
            } catch (error) {
                console.error("Fout bij het ophalen van saldo:", error);
                alert("Er is een fout opgetreden bij het ophalen van het saldo.");
            }
        };

        if (beheerderId) {
            fetchSaldo();
        }
    }, [beheerderId]);

    // functie om saldo op te waarderen
    const laadSaldoOp = async () => {
        try {
            await axios.post(`/api/abonnement/${beheerderId}/saldo/opwaarderen`, { bedrag });
            alert("Saldo succesvol opgewaardeerd.");
            setSaldo(saldo + bedrag);
        } catch (error) {
            console.error("Fout bij het opwaarderen van saldo:", error);
            alert("Er is een fout opgetreden bij het opwaarderen van het saldo.");
        }
    };

    return (
        <div className="saldo-container">
            <h1>Huidig Prepaid Saldo</h1>
            <p>Uw huidige saldo bedraagt: €{saldo.toFixed(2)}</p>
            <input
                type="number"
                value={bedrag}
                onChange={(e) => setBedrag(parseFloat(e.target.value))}
                placeholder="Bedrag toevoegen"
            />
            <button onClick={laadSaldoOp}>Saldo Opwaarderen</button>
            
        </div>
    );
}

export default PrepaidSaldo;
