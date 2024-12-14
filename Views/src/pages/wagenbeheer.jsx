import React, { useState, useEffect } from "react";
import WagenparkService from "../services/requests/WagenparkService";
import "../styles/styles.css";
import "../styles/Wagenparkbeheerder.css";
import "../styles/Notificatie.css";

const WagenbeheerDashboard = () => {
    const [medewerkers, setMedewerkers] = useState([]);
    const [nieuweMedewerker, setNieuweMedewerker] = useState({
        naam: "",
        email: "",
        wachtwoord: "",
    });
    const [notificatie, setNotificatie] = useState("");
    const [isLoading, setIsLoading] = useState(false);

    // Ophalen van bestaande medewerkers
    useEffect(() => {
        const fetchMedewerkers = async () => {
            try {
                const data = await WagenparkService.getMedewerkers();
                setMedewerkers(data);
            } catch (error) {
                setNotificatie(error.message);
                setTimeout(() => setNotificatie(""), 3000);
            }
        };

        fetchMedewerkers();
    }, []);

    const handleInputChange = (e) => {
        const { name, value } = e.target;
        setNieuweMedewerker((prev) => ({ ...prev, [name]: value }));
    };

    const voegMedewerkerToe = async () => {
        const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;

        if (!emailRegex.test(nieuweMedewerker.email)) {
            alert("Email moet een geldig e-mailadres zijn.");
            return;
        }

        setIsLoading(true);

        try {
            const nieuweData = await WagenparkService.voegMedewerkerToe(nieuweMedewerker);
            setMedewerkers((prev) => [...prev, nieuweData]);
            setNotificatie(`Toegevoegd: ${nieuweMedewerker.naam}`);
        } catch (error) {
            setNotificatie(error.message);
        } finally {
            setNieuweMedewerker({ naam: "", email: "", wachtwoord: "" });
            setTimeout(() => setNotificatie(""), 3000);
            setIsLoading(false);
        }
    };

    const verwijderMedewerker = async (email) => {
        setIsLoading(true);

        try {
            await WagenparkService.verwijderMedewerker(email);
            setMedewerkers((prev) => prev.filter((m) => m.email !== email));
            setNotificatie(`Verwijderd: ${email}`);
        } catch (error) {
            setNotificatie(error.message);
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
                    <div className="medewerker-form">
                        <h2>Voeg een nieuwe medewerker toe</h2>
                        <input
                            type="text"
                            name="naam"
                            value={nieuweMedewerker.naam}
                            onChange={handleInputChange}
                            placeholder="Naam"
                            required
                        />
                        <input
                            type="email"
                            name="email"
                            value={nieuweMedewerker.email}
                            onChange={handleInputChange}
                            placeholder="Bedrijfs-E-mailadres"
                            required
                        />
                        <input
                            type="password"
                            name="wachtwoord"
                            value={nieuweMedewerker.wachtwoord}
                            onChange={handleInputChange}
                            placeholder="Wachtwoord"
                            required
                        />
                        <button onClick={voegMedewerkerToe} disabled={isLoading}>
                            {isLoading ? "Bezig..." : "Toevoegen"}
                        </button>
                    </div>

                    <h3>Huidige Medewerkers</h3>
                    <ul>
                        {medewerkers.map((medewerker, index) => (
                            <li key={index}>
                                {medewerker.naam} - {medewerker.email}
                                <button
                                    onClick={() => verwijderMedewerker(medewerker.email)}
                                    disabled={isLoading}
                                >
                                    Verwijderen
                                </button>
                            </li>
                        ))}
                    </ul>
                </div>
                {notificatie && <div className="notificatie-box">{notificatie}</div>}
            </div>
        </>
    );
};

export default WagenbeheerDashboard;
