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

    // Haal parameters uit de URL
    const kenteken = new URLSearchParams(location.search).get("kenteken");
    const VoertuigID = new URLSearchParams(location.search).get("VoertuigID");
    const SoortHuurder = new URLSearchParams(location.search).get("SoortHuurder");
    const startDate = new URLSearchParams(location.search).get("startdatum");
    const endDate = new URLSearchParams(location.search).get("einddatum");

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

                if (SoortHuurder === "Particulier") {
                    huurderResponse = await ParticulierHuurdersRequestService.getById(huurderId);
                    console.log("Particulier Huurder Data:", huurderResponse.data);
                } else if (SoortHuurder === "Zakelijk") {
                    huurderResponse = await BedrijfsMedewerkerRequestService.getById(huurderId);
                    console.log("Zakelijk Huurder Data:", huurderResponse.data);
                }
                console.log(huurderResponse.data);
                if (huurderResponse?.data) {
                    setHuurderNaam(
                        huurderResponse.data.particulierNaam || // Voor particulier
                        huurderResponse.data.medewerkerNaam || // Voor zakelijk
                        "Huurder" // Fallback
                    );
                }
            } catch (error) {
                console.error("Fout bij het ophalen van huurdergegevens:", error);
            }
        };

        if (huurderId) {
            fetchHuurderNaam();
        }
    }, [huurderId, SoortHuurder]);



    const handleBevestiging = async () => {
        setLoading(true);
        setErrorMessage("");

        try {
            // Controleer huurder op basis van SoortHuurder
            console.log("Controleer actief huurverzoek...");
            const checkResponse = await HuurVerzoekRequestService.checkActive(huurderId);
            if (checkResponse.data.hasActiveRequest) {
                throw new Error("Je hebt al een actief huurverzoek en kunt niet nogmaals huren.");
            }

            console.log("Haal voertuigdetails op...");
            const voertuigResponse = await VoertuigRequestService.getById(VoertuigID);
            const voertuigDetails = voertuigResponse.data;

            console.log("Maak nieuw huurverzoek...");
            const huurverzoek = {
                huurderID: huurderId,
                voertuigId: VoertuigID,
                beginDate: startDate,
                endDate: endDate,
                approved: false,
                isBevestigd: false,
                reden: "", 
            };

            console.log("Huurverzoek object:", huurverzoek);
            await HuurVerzoekRequestService.register(huurverzoek);

            console.log("Update voertuigstatus...");
            const voertuigUpdate = {
                startDatum: startDate,
                eindDatum: endDate,
                voertuigBeschikbaar: false,
            };
            await VoertuigRequestService.update(VoertuigID, voertuigUpdate);

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
                <p><strong>Startdatum:</strong> {startDate}</p>
                <p><strong>Einddatum:</strong> {endDate}</p>
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
