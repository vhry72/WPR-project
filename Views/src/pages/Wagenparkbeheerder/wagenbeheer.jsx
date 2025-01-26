import React, { useState, useEffect } from "react";
import axios from "axios";
import JwtService from "../../services/JwtService";
import "../../styles/styles.css";
import "../../styles/Wagenparkbeheerder.css";
import "../../styles/Notificatie.css";

const WagenbeheerDashboard = () => {

    const [medewerkers, setMedewerkers] = useState([]);
    const [alleMedewerkers, setAlleMedewerkers] = useState([]);
    const [selectedMedewerker, setSelectedMedewerker] = useState("");
    const [notificatie, setNotificatie] = useState("");
    const [error, setError] = useState("");
    const [isLoading, setIsLoading] = useState(false);
    const [beheerderId, setBeheerderId] = useState("");

   // Haal gegevens op
    useEffect(() => {
        const fetchMedewerkers = async () => {
            try {
                setIsLoading(true);
                const userId = await JwtService.getUserId();
                setBeheerderId(userId);
                const response = await axios.get(
                    `https://localhost:5033/api/WagenparkBeheerder/${userId}/medewerker-object`, { withCredentials: true });
                setAlleMedewerkers(response.data);
                // Filter de medewerkers met een abonnement en voeg ze toe aan de lijst
                const medewerkersMetAbonnement = response.data.filter(m => m.abonnementId !== null);
                setMedewerkers(medewerkersMetAbonnement);
            } catch (error) {
                console.error("Fout bij het ophalen van de medewerkers:", error);
                setError("Er is een fout opgetreden bij het laden van de medewerkers.");
            } finally {
                setIsLoading(false);
            }
        };
        fetchMedewerkers();
    }, []);

    const voegMedewerkerToeAanAbonnement = async () => {
        if (!selectedMedewerker) {
            setNotificatie("Selecteer een medewerker om toe te voegen.");
            setTimeout(() => setNotificatie(""), 3000);
            return;
        }

        setIsLoading(true);

        try {
            const medewerker = alleMedewerkers.find((m) => m.medewerkerEmail === selectedMedewerker);

            if (!medewerker) {
                throw new Error("Medewerker niet gevonden.");
            }

            if (medewerker.abonnementId) {
                setNotificatie("Deze medewerker heeft al een abonnement.");
                setTimeout(() => setNotificatie(""), 2000);
                return;
            }

            await axios.post(
                `https://localhost:5033/api/Abonnement/${beheerderId}/medewerker/toevoegen/${medewerker.bedrijfsMedewerkerId}`, { withCredentials: true });

            const updatedMedewerkers = [...medewerkers, medewerker];
            setMedewerkers(updatedMedewerkers);
            // Verwijder medewerker uit alleMedewerkers als deze is toegevoegd aan abonnement
            const remainingMedewerkers = alleMedewerkers.filter(m => m.medewerkerEmail !== medewerker.medewerkerEmail);
            setAlleMedewerkers(remainingMedewerkers);

            setNotificatie(`Medewerker ${medewerker.medewerkerEmail} toegevoegd aan abonnement.`);
        } catch (error) {
            console.error("Fout bij het toevoegen van medewerker aan abonnement:", error);
            setNotificatie("Er is een fout opgetreden bij het toevoegen van de medewerker.");
        } finally {
            setSelectedMedewerker("");
            setTimeout(() => setNotificatie(""), 2000);
            setIsLoading(false);
        }
    };

    const verwijderMedewerkerVanAbonnement = async (email) => {
        setIsLoading(true);

        try {
            const medewerker = medewerkers.find((m) => m.medewerkerEmail === email);

            await axios.delete(
                `https://localhost:5033/api/Abonnement/${beheerderId}/medewerker/verwijderen/${medewerker.bedrijfsMedewerkerId}`, { withCredentials: true });

            const updatedMedewerkers = medewerkers.filter((m) => m.medewerkerEmail !== email);
            setMedewerkers(updatedMedewerkers);
            // Voeg medewerker weer toe aan alleMedewerkers als deze is verwijderd uit abonnement
            setAlleMedewerkers([...alleMedewerkers, medewerker]);

            setNotificatie(`Medewerker met email ${email} verwijderd uit abonnement.`);
        } catch (error) {
            console.error("Fout bij het verwijderen van medewerker uit abonnement:", error);
            setNotificatie("Er is een fout opgetreden bij het verwijderen van de medewerker.");
        } finally {
            setTimeout(() => setNotificatie(""), 2000);
            setIsLoading(false);
        }
    };


    const handleMedewerkerSelect = (email) => {
        setSelectedMedewerker(email);
    };

    return (
        <>
            <header>
                <h1 className="H1tekst-wagenbeheer">Wagenparkbeheerder Dashboard</h1>
            </header>
            <div className="index-container">
                <div className="options">
                   
                    {/* Formulier voor het toevoegen van medewerker aan abonnement */}
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
                                .filter(
                                    (m) => !medewerkers.some((medewerker) => medewerker.medewerkerEmail === m.medewerkerEmail) && !m.abonnementId
                                )
                                .map((medewerker) => (
                                    <option key={medewerker.bedrijfsMedewerkerId} value={medewerker.medewerkerEmail}>
                                        {medewerker.medewerkerNaam} - {medewerker.medewerkerEmail}
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

                    <h3>Huidige Medewerkers in Abonnement</h3>
                    <ul aria-labelledby="medewerkerLijst">
                        {medewerkers.map((medewerker) => (
                            <li key={medewerker.bedrijfsMedewerkerId}>
                                <span>
                                    <strong>{medewerker.medewerkerNaam}</strong> - {medewerker.medewerkerEmail}
                                </span>
                                <button
                                    onClick={() => verwijderMedewerkerVanAbonnement(medewerker.medewerkerEmail)}
                                    disabled={isLoading}
                                    aria-live="polite"
                                    aria-label={`Verwijder medewerker ${medewerker.medewerkerNaam} met email ${medewerker.medewerkerEmail} uit abonnement`}
                                >
                                    Verwijderen
                                </button>
                            </li>
                        ))}
                    </ul>
                </div>

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
