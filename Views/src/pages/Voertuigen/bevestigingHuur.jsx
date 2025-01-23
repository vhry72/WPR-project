import React, { useState, useEffect } from "react";
import { useLocation, useNavigate } from 'react-router-dom';
import ParticulierHuurdersRequestService from '../../services/requests/ParticulierHuurderRequestService';
import BedrijfsMedewerkerRequestService from '../../services/requests/bedrijfsMedewerkerRequestService';
import VoertuigRequestService from '../../services/requests/VoertuigRequestService';
import HuurVerzoekRequestService from '../../services/requests/HuurVerzoekRequestService';
import JwtService from "../../services/JwtService";

const BevestigingHuur = () => {
    const navigate = useNavigate();
    const location = useLocation();
    const [loading, setLoading] = useState(false);
    const [errorMessage, setErrorMessage] = useState("");
    const [huurderNaam, setHuurderNaam] = useState("");
    const [huurderId, setHuurderId] = useState(null);
    const { startDateTime, endDateTime, userRole, kenteken, VoertuigID } = location.state;

    useEffect(() => {
        const fetchUserId = async () => {
            try {
                const userId = await JwtService.getUserId(); // Haal de gebruikers-ID op via de API
                if (userId) {
                    setHuurderId(userId);
                } else {
                    console.error("Huurder ID kon niet worden opgehaald via de API.");
                }
            } catch (error) {
                console.error("Fout bij het ophalen van de huurder ID:", error);
            }
        };

        fetchUserId();
    }, []);

    useEffect(() => {
        const fetchHuurderNaam = async () => {
            if (!huurderId) {
                console.error("Geen huurder ID beschikbaar.");
                return;
            }

            try {
                let huurderResponse;

                if (userRole === "ParticuliereHuurder") {
                    huurderResponse = await ParticulierHuurdersRequestService.getById(huurderId);
                    console.log("Particulier Huurder Data:", huurderResponse.data);
                } else if (userRole === "BedrijfsMedewerker") {
                    huurderResponse = await BedrijfsMedewerkerRequestService.getById(huurderId);
                    console.log("Zakelijk Huurder Data:", huurderResponse.data);
                }
                console.log(huurderResponse.data);
                if (huurderResponse?.data) {
                    setHuurderNaam(
                        huurderResponse.data.particulierNaam || // Voor particulier
                        huurderResponse.data.medewerkerNaam || // Voor zakelijk
                        "Huurder" // Fallback bij geen naam
                    );
                }
            } catch (error) {
                console.error("Fout bij het ophalen van huurdergegevens:", error);
            }
        };

        // haal de id van de role op
        if (huurderId) {
            fetchHuurderNaam();
        }
    }, [huurderId, userRole]);



    const handleBevestiging = async () => {
        setLoading(true);
        setErrorMessage("");

        try {
            console.log("Maak nieuw huurverzoek...");
            const huurverzoek = {
                huurderID: huurderId,
                voertuigId: VoertuigID,
                beginDate: startDateTime,
                endDate: endDateTime,
                approved: false,
                isBevestigd: false,
                reden: "", 
            };

            
            console.log("Huurverzoek object:", huurverzoek);
            await HuurVerzoekRequestService.register(huurverzoek);

            alert(`Huurverzoek succesvol bevestigd voor ${huurderNaam}!`);
            navigate("/");
        } catch (error) {
            console.error("Fout tijdens bevestiging:", error);
            setErrorMessage(error.response?.data?.Message || error.message || "Er is een fout opgetreden.");
        } finally {
            setLoading(false);
        }
    };


    return (
        <div className="container">
            <h1>Welkom op de bevestigingspagina, {huurderNaam}</h1>
            <div className="details">
                <p><strong>Kenteken:</strong> {kenteken}</p>
                <p><strong>Startdatum:</strong> {startDateTime}</p>
                <p><strong>Einddatum:</strong> {endDateTime}</p>
            </div>

            {errorMessage && <p className="error-message" style={{ color: "red" }}>{errorMessage}</p>}

            <button
                onClick={handleBevestiging}
                disabled={loading}
                className="submit-button"
                style={{ backgroundColor: loading ? "grey" : "green", color: "white", padding: "10px" }}
            >
                {loading ? "Bezig met verwerken..." : "Bevestig Huur"}
            </button>
        </div>
    );
};

export default BevestigingHuur;