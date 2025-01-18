import React, { useState, useEffect } from "react";
import axios from "axios";
import JwtService from "../../services/JwtService";
import "../../styles/styles.css";
import "../../styles/Wagenparkbeheerder.css";
import "../../styles/Notificatie.css";

const WagenbeheerDashboard = () => {
    const [medewerkers, setMedewerkers] = useState([]); // Medewerkers in abonnement
    const [alleMedewerkers, setAlleMedewerkers] = useState([]); // Alle medewerkers
    const [selectedMedewerker, setSelectedMedewerker] = useState(""); // Geselecteerde medewerker (email)
    const [notificatie, setNotificatie] = useState(""); // Notificaties
    const [error, setError] = useState(""); // Fouten
    const [isLoading, setIsLoading] = useState(false); // Laadstatus

    // Haal gegevens op
    useEffect(() => {
        const fetchMedewerkers = async () => {
            try {
                setIsLoading(true);
                const userId = await JwtService.getUserId();
                const response = await axios.get(
                    `https://localhost:5033/api/WagenparkBeheerder/${userId}/medewerker-object`
                );
                setAlleMedewerkers(response.data); // Sla medewerkers op
            } catch (error) {
                console.error("Fout bij het ophalen van de medewerkers:", error);
                setError("Er is een fout opgetreden bij het laden van de medewerkers.");
            } finally {
                setIsLoading(false);
            }
        };

        fetchMedewerkers();
    }, []);

    // Voeg medewerker toe aan abonnement
    const voegMedewerkerToeAanAbonnement = async () => {
        if (!selectedMedewerker) {
            setNotificatie("Selecteer een medewerker om toe te voegen.");
            setTimeout(() => setNotificatie(""), 3000);
            return;
        }

        setIsLoading(true);

        try {
            const medewerker = alleMedewerkers.find((m) => m.email === selectedMedewerker);

            // API-aanroep om medewerker toe te voegen
            await axios.post(
                `https://localhost:5033/api/WagenparkBeheerder/medewerkers/toevoegen`,
                { email: selectedMedewerker }
            );

            setMedewerkers((prev) => [...prev, medewerker]);
            setNotificatie(`Medewerker ${medewerker.email} toegevoegd aan abonnement.`);
        } catch (error) {
            console.error("Fout bij het toevoegen van medewerker aan abonnement:", error);
            setNotificatie("Er is een fout opgetreden bij het toevoegen van de medewerker.");
        } finally {
            setSelectedMedewerker("");
            setTimeout(() => setNotificatie(""), 3000);
            setIsLoading(false);
        }
    };

    // Verwijder medewerker uit abonnement
    const verwijderMedewerkerVanAbonnement = async (email) => {
        setIsLoading(true);

        try {
            // API-aanroep om medewerker te verwijderen
            await axios.delete(
                `https://localhost:5033/api/WagenparkBeheerder/medewerkers/verwijderen`,
                { data: { email } }
            );

            setMedewerkers((prev) => prev.filter((m) => m.email !== email));
            setNotificatie(`Medewerker met email ${email} verwijderd uit abonnement.`);
        } catch (error) {
            console.error("Fout bij het verwijderen van medewerker uit abonnement:", error);
            setNotificatie("Er is een fout opgetreden bij het verwijderen van de medewerker.");
        } finally {
            setTimeout(() => setNotificatie(""), 3000);
            setIsLoading(false);
        }
    };

    const handleMedewerkerSelect = (email) => {
        setSelectedMedewerker(email);
    };

    return (
        <>
            <header>
                <h1 className="H1Tekst-Index">Wagenparkbeheerder Dashboard</h1>
            </header>
            <div className="index-container">
                <div className="options">
                    <h2>Beheer Abonnement Medewerkers</h2>

                    {/* Formulier voor het toevoegen van medewerkers */}
                    <div className="medewerker-form">
                        <label htmlFor="medewerkerSelect">Selecteer een medewerker</label>
                        <select
                            id="medewerkerSelect"
                            value={selectedMedewerker}
                            onChange={(e) => handleMedewerkerSelect(e.target.value)}
                            aria-label="Selecteer een medewerker om toe te voegen aan het abonnement"
                        >
                            <option value="" disabled>
                                -- Selecteer een medewerker --
                            </option>
                            {alleMedewerkers
                                .filter((m) => !medewerkers.some((medewerker) => medewerker.email === m.email))
                                .map((medewerker) => (
                                    <option key={medewerker.bedrijfsMedewerkerId} value={medewerker.email}>
                                        {medewerker.medewerkerNaam} - {medewerker.email}
                                    </option>
                                ))}
                        </select>
                        <p>Geselecteerde medewerker: {selectedMedewerker || "Geen"}</p>
                        <button
                            onClick={voegMedewerkerToeAanAbonnement}
                            disabled={isLoading || !selectedMedewerker}
                            aria-live="polite"
                        >
                            {isLoading ? "Bezig met toevoegen..." : "Toevoegen"}
                        </button>
                    </div>

                    {/* Lijst van huidige medewerkers */}
                    <h3>Huidige Medewerkers in Abonnement</h3>
                    <ul aria-labelledby="medewerkerLijst">
                        {medewerkers.map((medewerker) => (
                            <li key={medewerker.bedrijfsMedewerkerId}>
                                <span>
                                    <strong>{medewerker.medewerkerNaam}</strong> - {medewerker.email}
                                </span>
                                <button
                                    onClick={() => verwijderMedewerkerVanAbonnement(medewerker.email)}
                                    disabled={isLoading}
                                    aria-live="polite"
                                    aria-label={`Verwijder medewerker ${medewerker.medewerkerNaam} met email ${medewerker.email} uit abonnement`}
                                >
                                    Verwijderen
                                </button>
                            </li>
                        ))}
                    </ul>
                </div>

                {/* Notificaties */}
                {notificatie && (
                    <div
                        className="notificatie-box"
                        role="alert"
                        aria-live="assertive"
                        aria-atomic="true"
                    >
                        {notificatie}
                    </div>
                )}
            </div>
        </>
    );
};

export default WagenbeheerDashboard;