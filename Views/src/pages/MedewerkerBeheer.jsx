import React, { useState, useEffect } from "react";
import axios from 'axios';
import JwtService from "../services/JwtService";

function MedewerkerBeheer({ zakelijkeId }) {
    const [medewerkers, setMedewerkers] = useState([]);
    const [huurderId, setHuurderId] = useState(null);
    const [nieuweMedewerker, setNieuweMedewerker] = useState({
        naam: "",
        email: "",
        wachtwoord: ""
    });

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
        // Fetch de huidige medewerkers bij component load
        const fetchMedewerkers = async () => {
            try {
                const response = await axios.get(`/api/abonnement/${zakelijkeId}/medewerkers`);
                setMedewerkers(response.data);
            } catch (error) {
                console.error("Fout bij ophalen van medewerkers:", error);
            }
        };
        fetchMedewerkers();
    }, [zakelijkeId]);

    const handleInputChange = (e) => {
        const { name, value } = e.target;
        setNieuweMedewerker((prev) => ({ ...prev, [name]: value }));
    };

    const voegMedewerkerToe = async () => {
        if (!nieuweMedewerker.email.endsWith("@bedrijf.nl")) {
            alert("Email moet een bedrijfs-e-mailadres zijn.");
            return;
        }

        try {
            await axios.post(`/api/abonnement/${zakelijkeId}/medewerker/toevoegen`, nieuweMedewerker);
            setMedewerkers((prev) => [...prev, nieuweMedewerker]);
            setNieuweMedewerker({ naam: "", email: "", wachtwoord: "" });
        } catch (error) {
            alert("Fout bij toevoegen van medewerker.");
            console.error(error);
        }
    };

    const verwijderMedewerker = async (medewerkerId) => {
        if (!window.confirm("Weet je zeker dat je deze medewerker wilt verwijderen?")) {
            return;
        }

        try {
            await axios.delete(`/api/abonnement/${zakelijkeId}/medewerker/verwijderen/${medewerkerId}`);
            setMedewerkers((prev) => prev.filter((m) => m.bedrijfsMedewerkerId !== medewerkerId));
        } catch (error) {
            alert("Fout bij verwijderen van medewerker.");
            console.error(error);
        }
    };

    return (
        <div>
            <h2>Medewerker Beheer</h2>

            {/* Formulier voor het toevoegen van een medewerker */}
            <div>
                <h3>Voeg een nieuwe medewerker toe</h3>
                <input
                    type="text"
                    name="naam"
                    value={nieuweMedewerker.naam}
                    onChange={handleInputChange}
                    placeholder="Naam"
                />
                <input
                    type="email"
                    name="email"
                    value={nieuweMedewerker.email}
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

            {/* Lijst met medewerkers */}
            <h3>Huidige Medewerkers</h3>
            <ul>
                {medewerkers.map((medewerker) => (
                    <li key={medewerker.bedrijfsMedewerkerId}>
                        {medewerker.naam} - {medewerker.email}
                        <button onClick={() => verwijderMedewerker(medewerker.bedrijfsMedewerkerId)}>
                            Verwijderen
                        </button>
                    </li>
                ))}
            </ul>
        </div>
    );
}

export default MedewerkerBeheer;
