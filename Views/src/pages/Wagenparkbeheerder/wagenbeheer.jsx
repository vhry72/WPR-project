import React, { useState, useEffect } from "react";
import WagenparkService from "../../services/requests/WagenparkService";
import BedrijfsMedewerkerRequestService from "../../services/requests/bedrijfsMedewerkerRequestService"
import "../../styles/styles.css";
import "../../styles/Wagenparkbeheerder.css";
import "../../styles/Notificatie.css";

const WagenbeheerDashboard = () => {
    const [medewerkers, setMedewerkers] = useState([]);
    const [nieuweMedewerker, setNieuweMedewerker] = useState({
        naam: "",
        email: "",
        wachtwoord: "",
    });
    const [notificatie, setNotificatie] = useState("");
    const [isLoading, setIsLoading] = useState(false);

    useEffect(() => {
        const fetchMedewerkers = async () => {
            try {
                const data = await WagenparkService.getMedewerkers();
                setMedewerkers(data);
                console.log(data);
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
                        <label htmlFor="naam">Naam</label>
                        <input
                            type="text"
                            id="naam"
                            name="naam"
                            value={nieuweMedewerker.naam}
                            onChange={handleInputChange}
                            placeholder="Naam"
                            required
                            aria-required="true"
                        />
                        <label htmlFor="email">Bedrijfs-E-mailadres</label>
                        <input
                            type="email"
                            id="email"
                            name="email"
                            value={nieuweMedewerker.email}
                            onChange={handleInputChange}
                            placeholder="Bedrijfs-E-mailadres"
                            required
                            aria-required="true"
                        />
                        <label htmlFor="wachtwoord">Wachtwoord</label>
                        <input
                            type="password"
                            id="wachtwoord"
                            name="wachtwoord"
                            value={nieuweMedewerker.wachtwoord}
                            onChange={handleInputChange}
                            placeholder="Wachtwoord"
                            required
                            aria-required="true"
                        />
                        <button onClick={voegMedewerkerToe} disabled={isLoading} aria-live="polite">
                            {isLoading ? "Bezig..." : "Toevoegen"}
                        </button>
                    </div>

                    <h3>Huidige Medewerkers</h3>
                    <ul>
                        {medewerkers.map((medewerker, index) => (
                            <li key={index}>
                                <span>{medewerker.naam} - {medewerker.email}</span>
                                <button
                                    onClick={() => verwijderMedewerker(medewerker.email)}
                                    disabled={isLoading}
                                    aria-live="polite"
                                >
                                    Verwijderen
                                </button>
                            </li>
                        ))}
                    </ul>
                </div>
                {notificatie && <div className="notificatie-box" role="alert" aria-live="assertive">{notificatie}</div>}
            </div>
        </>
    );
};

export default WagenbeheerDashboard;
