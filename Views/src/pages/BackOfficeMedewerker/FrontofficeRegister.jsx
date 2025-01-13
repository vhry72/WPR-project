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
            console.log();
            // Voeg de nieuwe medewerker toe aan de lijst
            setFrontOfficeMedewerkers((prev) => [...prev, response.data]);
            setNieuweMedewerker({ medewerkerNaam: "", medewerkerEmail: "", wachtwoord: "" });
        } catch (error) {
            alert("Fout bij toevoegen van medewerker.");
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
                        <button onClick={() => deleteMedewerker(medewerker.frontofficeMedewerkerId)}>
                            Verwijderen
                        </button>
                    </li>
                ))}
            </ul>
        </div>
    );
}

export default FrontofficeRegister;
