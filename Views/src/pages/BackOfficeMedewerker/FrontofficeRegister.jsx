import React, { useState, useEffect } from "react";
import axios from "axios";
import JwtService from "../../services/JwtService";
import { v4 as uuidv4 } from "uuid";

function FrontofficeRegister() {
    const [frontOfficeMedewerkers, setFrontOfficeMedewerkers] = useState([]);
    const [nieuweMedewerker, setNieuweMedewerker] = useState({
        medewerkerNaam: "",
        medewerkerEmail: "",
        wachtwoord: "",
    });
    const [geselecteerdeMedewerker, setGeselecteerdeMedewerker] = useState(null);

    // Haal de huidige gebruiker-ID op bij het laden van de component
    useEffect(() => {
        const fetchUserId = async () => {
            try {
                const userId = await JwtService.getUserId();
                if (userId) {
                    console.log("Gebruikers-ID opgehaald:", userId);
                } else {
                    console.error("Gebruikers-ID kon niet worden opgehaald.");
                }
            } catch (error) {
                console.error("Fout bij het ophalen van de gebruikers-ID:", error);
            }
        };

        fetchUserId();
    }, []);

    // Haal frontoffice medewerkers op via de API
    useEffect(() => {
        const fetchFrontOfficeMedewerkers = async () => {
            try {
                const response = await axios.get("/api/Account/frontoffice-medewerkers");
                const medewerkersData = Array.isArray(response.data) ? response.data : [];
                setFrontOfficeMedewerkers(medewerkersData);
            } catch (error) {
                console.error("Fout bij ophalen van medewerkers:", error);
            }
        };

        fetchFrontOfficeMedewerkers();
    }, []);

    // Update inputwaarden voor de nieuwe medewerker
    const handleInputChange = (e) => {
        const { name, value } = e.target;
        setNieuweMedewerker((prev) => ({ ...prev, [name]: value }));
    };

    // Voeg een nieuwe medewerker toe via de juiste API
    const voegMedewerkerToe = async () => {
        if (!nieuweMedewerker.medewerkerEmail.endsWith("@frontoffice.nl")) {
            alert("E-mailadres moet eindigen op @frontoffice.nl.");
            return;
        }

        const payload = {
            frontofficeMedewerkerId: uuidv4(),
            medewerkerNaam: nieuweMedewerker.medewerkerNaam,
            medewerkerEmail: nieuweMedewerker.medewerkerEmail,
            wachtwoord: nieuweMedewerker.wachtwoord,
            emailBevestigingToken: uuidv4(),
            isEmailBevestigd: false,
            aspNetUserId: "string",
        };

        try {
            console.log(payload);
            const response = await axios.post(`https://localhost:5033/api/Account/register-frontoffice`, payload);
            setFrontOfficeMedewerkers((prev) => [...prev, response.data]);
            setNieuweMedewerker({ medewerkerNaam: "", medewerkerEmail: "", wachtwoord: "" });
        } catch (error) {
            alert("Fout bij toevoegen van medewerker.");
            console.error(error);
        }
    };

    // Selecteer een medewerker om gegevens te bewerken
    const selecteerMedewerkerVoorWijziging = (medewerker) => {
        setGeselecteerdeMedewerker({ ...medewerker });
    };

    // Update de gewijzigde gegevens van een medewerker
    const wijzigMedewerker = async () => {
        try {
            await axios.put(`/api/Account/frontoffice-medewerkers/${geselecteerdeMedewerker.frontofficeMedewerkerId}`, geselecteerdeMedewerker);
            setFrontOfficeMedewerkers((prev) =>
                prev.map((medewerker) =>
                    medewerker.frontofficeMedewerkerId === geselecteerdeMedewerker.frontofficeMedewerkerId ? geselecteerdeMedewerker : medewerker
                )
            );
            setGeselecteerdeMedewerker(null);
        } catch (error) {
            alert("Fout bij wijzigen van medewerker.");
            console.error(error);
        }
    };

    return (
        <div>
            <h2>Frontoffice Medewerker Beheer</h2>

            {/* Formulier voor het toevoegen van een nieuwe medewerker */}
            <div>
                <h3>Nieuwe Medewerker Toevoegen</h3>
                <input
                    type="text"
                    name="medewerkerNaam"
                    value={nieuweMedewerker.medewerkerNaam}
                    onChange={handleInputChange}
                    placeholder="Naam"
                />
                <input
                    type="email"
                    name="medewerkerEmail"
                    value={nieuweMedewerker.medewerkerEmail}
                    onChange={handleInputChange}
                    placeholder="E-mailadres"
                />
                <input
                    type="password"
                    name="wachtwoord"
                    value={nieuweMedewerker.wachtwoord}
                    onChange={handleInputChange}
                    placeholder="Wachtwoord"
                />
                <button onClick={voegMedewerkerToe}>Toevoegen</button>
            </div>

            {/* Lijst met huidige medewerkers */}
            <h3>Huidige Medewerkers</h3>
            <ul>
                {frontOfficeMedewerkers.map((medewerker) => (
                    <li key={medewerker.frontofficeMedewerkerId}>
                        {medewerker.medewerkerNaam} - {medewerker.medewerkerEmail}
                        <button onClick={() => selecteerMedewerkerVoorWijziging(medewerker)}>Wijzig Gegevens</button>
                    </li>
                ))}
            </ul>

            {/* Formulier voor het wijzigen van een medewerker */}
            {geselecteerdeMedewerker && (
                <div>
                    <h3>Medewerker Gegevens Wijzigen</h3>
                    <input
                        type="text"
                        name="medewerkerNaam"
                        value={geselecteerdeMedewerker.medewerkerNaam}
                        onChange={(e) => setGeselecteerdeMedewerker({ ...geselecteerdeMedewerker, medewerkerNaam: e.target.value })}
                        placeholder="Naam"
                    />
                    <input
                        type="email"
                        name="medewerkerEmail"
                        value={geselecteerdeMedewerker.medewerkerEmail}
                        onChange={(e) => setGeselecteerdeMedewerker({ ...geselecteerdeMedewerker, medewerkerEmail: e.target.value })}
                        placeholder="E-mailadres"
                    />
                    <button onClick={wijzigMedewerker}>Opslaan</button>
                    <button onClick={() => setGeselecteerdeMedewerker(null)}>Annuleren</button>
                </div>
            )}
        </div>
    );
}

export default FrontofficeRegister;
