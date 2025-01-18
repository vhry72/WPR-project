import React, { useState, useEffect } from "react";
import BedrijfsMedewerkerRequestService from "../../services/requests/BedrijfsMedewerkerRequestService";
import "../../styles/styles.css";
import "../../styles/Wagenparkbeheerder.css";
import "../../styles/Notificatie.css";

const WagenbeheerDashboard = () => {
    const [medewerkers, setMedewerkers] = useState([]); // Medewerkers in abonnement
    const [alleMedewerkers, setAlleMedewerkers] = useState([]); // Alle medewerkers
    const [selectedMedewerker, setSelectedMedewerker] = useState(""); // Geselecteerde medewerker
    const [notificatie, setNotificatie] = useState("");
    const [isLoading, setIsLoading] = useState(false);

    // Haal gegevens op
    useEffect(() => {
        const fetchMedewerkers = async () => {
            try {
                const response = await BedrijfsMedewerkerRequestService.getAll();
                setAlleMedewerkers(response.data);
            } catch (error) {
                setNotificatie("Fout bij het ophalen van alle medewerkers.");
                setTimeout(() => setNotificatie(""), 2000);
            }
        };

        const fetchAbonnementMedewerkers = async () => {
            try {
                const abonnementResponse = await BedrijfsMedewerkerRequestService.getAbonnementMedewerkers();
                setMedewerkers(abonnementResponse.data);
            } catch (error) {
                setNotificatie("Fout bij het ophalen van medewerkers in abonnement.");
                setTimeout(() => setNotificatie(""), 3000);
            }
        };

        fetchMedewerkers();
        fetchAbonnementMedewerkers();
    }, []);

    const voegMedewerkerToeAanAbonnement = async () => {
        if (!selectedMedewerker) {
            setNotificatie("Selecteer een medewerker om toe te voegen.");
            setTimeout(() => setNotificatie(""), 3000);
            return;
        }

        setIsLoading(true);

        try {
            await BedrijfsMedewerkerRequestService.addToAbonnement(selectedMedewerker);
            const medewerker = alleMedewerkers.find((m) => m.email === selectedMedewerker);
            setMedewerkers((prev) => [...prev, medewerker]);
            setNotificatie(`Medewerker ${medewerker.naam} toegevoegd aan abonnement.`);
        } catch (error) {
            setNotificatie("Fout bij het toevoegen van medewerker aan abonnement.");
        } finally {
            setSelectedMedewerker("");
            setTimeout(() => setNotificatie(""), 3000);
            setIsLoading(false);
        }
    };

    const verwijderMedewerkerVanAbonnement = async (email) => {
        setIsLoading(true);

        try {
            await BedrijfsMedewerkerRequestService.removeFromAbonnement(email);
            setMedewerkers((prev) => prev.filter((m) => m.email !== email));
            setNotificatie(`Medewerker met email ${email} verwijderd uit abonnement.`);
        } catch (error) {
            setNotificatie("Fout bij het verwijderen van medewerker uit abonnement.");
        } finally {
            setTimeout(() => setNotificatie(""), 3000);
            setIsLoading(false);
        }
    };

    return (
        <>
            <header>
                <h1 className="H1Tekst-Index">Wagenparkbeheerder Dashboard</h1>
            </header>
            <div className="index-container">
                <div className="options">
                    <h2>Beheer Abonnement Medewerkers</h2>

                    <div className="medewerker-form">
                        <label htmlFor="medewerkerSelect">Selecteer een medewerker</label>
                        <select
                            id="medewerkerSelect"
                            value={selectedMedewerker}
                            onChange={(e) => setSelectedMedewerker(e.target.value)}
                            aria-label="Selecteer een medewerker om toe te voegen aan het abonnement"
                        >
                            <option value="" disabled>
                                -- Selecteer een medewerker --
                            </option>
                            {alleMedewerkers
                                .filter((m) => !medewerkers.some((medewerker) => medewerker.email === m.email))
                                .map((medewerker) => (
                                    <option key={medewerker.id} value={medewerker.email}>
                                        {medewerker.naam} - {medewerker.email}
                                    </option>
                                ))}
                        </select>
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
                            <li key={medewerker.id}>
                                <span>
                                    <strong>{medewerker.naam}</strong> - {medewerker.email}
                                </span>
                                <button
                                    onClick={() => verwijderMedewerkerVanAbonnement(medewerker.email)}
                                    disabled={isLoading}
                                    aria-live="polite"
                                    aria-label={`Verwijder medewerker ${medewerker.naam} met email ${medewerker.email} uit abonnement`}
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