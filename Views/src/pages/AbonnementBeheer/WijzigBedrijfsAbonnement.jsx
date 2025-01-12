import React, { useState, useEffect } from "react";
import { useLocation } from "react-router-dom";
import axios from "axios";
import JwtService from "../../services/JwtService";
import "../../styles/Abonnement.css";

const API_URL = "https://localhost:5033/api/Abonnement";

function WijzigBedrijfsAbonnement() {
    const [beheerderId, setBeheerderId] = useState("");
    const [abonnementId, setAbonnementId] = useState("");
    const location = useLocation();
    const [isLoading, setIsLoading] = useState(false);
    const [wijzigingVerstuurd, setWijzigingVerstuurd] = useState(false);
    const [aantalDagen, setAantalDagen] = useState(0);
    const [directZichtbaar, setDirectZichtbaar] = useState(false)
    const [betaalMethode, setBetaalMethode] = useState("");
    const [error, setError] = useState("");
    const [zakelijkeId, setZakelijkeId] = useState(""); 
    const [updateDatum, setUpdateDatum] = useState("");

    const abonnement = location.state?.selectedAbonnement || location.state?.wijzigAbonnement;

    // Haal het beheerder ID op bij het laden van de component
    useEffect(() => {
        const fetchBeheerderId = async () => {
            try {
                const userId = await JwtService.getUserId();
                if (userId) {
                    setBeheerderId(userId);
                } else {
                    console.error("Beheerder ID kon niet worden opgehaald via de API.");
                }
            }
            catch (error) {
                console.error("Fout bij het ophalen van de beheerder ID:", error);
            }
        };

        fetchBeheerderId();
    }, []);

    useEffect(() => {
        const fetchZakelijkeId = async () => {
            setIsLoading(true);
            setError(null);

            try {
                const response = await axios.get(
                    `https://localhost:5033/api/WagenparkBeheerder/${beheerderId}/zakelijkeId`
                );
                setZakelijkeId(response.data.zakelijkeId); // Veronderstel dat zakelijkeId juist is in de API-respons
            } catch (error) {
                console.error("Fout bij het ophalen van de zakelijke ID:", error);
                setError("Kan zakelijke ID niet ophalen. Controleer de API of netwerkverbinding.");
            } finally {
                setIsLoading(false);
            }
        };

        if (beheerderId) {
            fetchZakelijkeId();
        }
    }, [beheerderId]);

    useEffect(() => {
        const fetchAbonnementId = async () => {
            setIsLoading(true);
            setError(null);

            try {
                const response = await axios.get(
                    `https://localhost:5033/api/WagenparkBeheerder/${beheerderId}/AbonnementId`
                );
                setAbonnementId(response.data.abonnementId);
            } catch (error) {
                console.error("Fout bij het ophalen van de Abonnement id:", error);
                setError("Kan Abonnement id niet ophalen. Controleer de API of netwerkverbinding.");
            } finally {
                setIsLoading(false);
            }
        };

        if (beheerderId) {
            fetchAbonnementId();
        }
    }, [beheerderId]);

    const wijzigAbonnement = async () => {

        if (!betaalMethode) {
            alert("Selecteer een betaalmethode.");
            return;
        }

        if (!abonnement || !zakelijkeId || !beheerderId) {
            alert("Alle gegevens zijn niet beschikbaar. Controleer of alles correct is geladen.");
            return;
        }

        let berekendeUpdateDatum = new Date();
        berekendeUpdateDatum.setDate(berekendeUpdateDatum.getDate() + aantalDagen);

        try {
            const payload = {
                abonnementId: abonnementId,
                naam: abonnement.naam,
                kosten: abonnement.prijs,
                zakelijkeId: zakelijkeId,
                type: betaalMethode,
                abonnementTermijnen: abonnement.termijn,
                korting: abonnement.korting,
                aantalDagen: aantalDagen,
                directZichtbaar: directZichtbaar,
                updateDatum: berekendeUpdateDatum.toISOString()
            };

            console.log(payload);
            await axios.post(`${API_URL}/${beheerderId}/abonnement/wijzig`, payload);
            setWijzigingVerstuurd(true);
        } catch (error) {
            console.error("Fout bij het versturen van de factuur:", error);
            alert("Er is een fout opgetreden bij het versturen van de factuur.");
        }
    };

    if (wijzigingVerstuurd) {
        return (
            <div className="bedrijf-abonnement-container">
                <h1>Wijziging is Doorgegeven Verstuurd!</h1>
                <p>De factuur en bevestiging is succesvol naar je e-mailadres gestuurd.</p>
            </div>
        );
    }

    return (
        <div className="bedrijf-abonnement-container">
            <h1>Wijzig Bedrijfsabonnement</h1>
            {abonnement && (
                <h3>
                    Geselecteerd abonnement: {abonnement?.naam} - €{abonnement?.prijs}{" "}
                    {abonnement?.korting > 0 && (
                        <span>(Korting: {abonnement?.korting}%)</span>
                    )}
                </h3>
            )}
            <div className="abonnement-type-selector">
                <h3>Kies een betaalmethode:</h3>
                <button
                    className={`abonnement-button ${betaalMethode === "PayAsYouGo" ? "selected" : ""}`}
                    onClick={() => setBetaalMethode("PayAsYouGo")}
                >
                    PayAsYouGo
                </button>
                <button
                    className={`abonnement-button ${betaalMethode === "PrepaidSaldo" ? "selected" : ""}`}
                    onClick={() => setBetaalMethode("PrepaidSaldo")}
                >
                    PrepaidSaldo
                </button>
            </div>
            <div className="abonnement-form">
                <label>
                    Aantal dagen (1-10):
                    <input
                        type="number"
                        min="1"
                        max="10"
                        value={aantalDagen}
                        onChange={(e) => setAantalDagen(e.target.value)}
                    />
                </label>
                <div className="direct-zichtbaar-selector">
                    <h3>Direct Zichtbaar:</h3>
                    <button
                        className={`abonnement-button ${directZichtbaar ? "selected" : ""}`}
                        onClick={() => setDirectZichtbaar(true)}
                    >
                        Ja
                    </button>
                    <button
                        className={`abonnement-button ${!directZichtbaar ? "selected" : ""}`}
                        onClick={() => setDirectZichtbaar(false)}
                    >
                        Nee
                    </button>
                </div>
            </div>
            <button
                onClick={wijzigAbonnement}
                className="create-subscription-button"
                disabled={isLoading}
            >
                {isLoading ? "Laden..." : "Wijzig Abonnement"}
            </button>
            {wijzigingVerstuurd && <p>Wijziging succesvol verzonden!</p>}
        </div>
    );
}

export default WijzigBedrijfsAbonnement;





    