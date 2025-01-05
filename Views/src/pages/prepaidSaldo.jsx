import React, { useState, useEffect } from "react";
import { useLocation } from "react-router-dom";
import axios from "axios";
import "../styles/Abonnement.css";

function PrepaidSaldo() {
    const location = useLocation();
    const [saldo, setSaldo] = useState(0);
    const [beheerderId, setBeheerderId] = useState(location.state?.beheerderId || null);
    const [bedrag, setBedrag] = useState(0);

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
