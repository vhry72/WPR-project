import React, { useState, useEffect } from "react";
import "../../styles/styles.css";
import "../../styles/Register.css";
import JwtService from "../../services/JwtService";
import axios from 'axios';

const BedrijfsMedewerkerRegister = () => {
    const [formData, setFormData] = useState({
        medewerkerNaam: "",
        email: "",
        wachtwoord: "",
    });
    const [notificatie, setNotificatie] = useState("");
    const [isLoading, setIsLoading] = useState(false);
    const [wagenparkBeheerderId, setWagenparkBeheerderId] = useState(null);
    const [zakelijkeHuurderId, setZakelijkeHuurderId] = useState(null);

    const handleInputChange = (e) => {
        const { name, value } = e.target;
        setFormData((prev) => ({ ...prev, [name]: value }));
    };

    useEffect(() => {
        const fetchUserId = async () => {
            try {
                const userId = await JwtService.getUserId();
                console.log("User ID:", userId);
                if (userId) {
                    setWagenparkBeheerderId(userId);
                } else {
                    console.error("Huurder ID kon niet worden opgehaald via de API.");
                }
            } catch (error) {
                console.error("Fout bij het ophalen van de huurder ID:", error);
            }
        };

        fetchUserId();
    }, []);

    const fetchZakelijkeHuurderId = async () => {
        if (!wagenparkBeheerderId) return;
        try {
            const response = await axios.get(`https://localhost:5033/api/WagenparkBeheerder/${wagenparkBeheerderId}/zakelijkeId`);
            setZakelijkeHuurderId(response.data.zakelijkeId);
        } catch (error) {
            console.error("Error fetching zakelijke huurder data:", error);
        }
    };

    const validateFormData = () => {
        const errors = [];
        const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;

        if (!formData.medewerkerNaam || formData.medewerkerNaam.length < 2 || formData.medewerkerNaam.length > 50) {
            errors.push("Medewerkernaam moet tussen 2 en 50 tekens zijn.");
        }

        if (!emailRegex.test(formData.email)) {
            errors.push("Vul een geldig e-mailadres in.");
        }

        if (!formData.wachtwoord || formData.wachtwoord.length < 8 || !/[A-Z]/.test(formData.wachtwoord) || !/[!@#$&*]/.test(formData.wachtwoord)) {
            errors.push("Wachtwoord moet minimaal 8 tekens bevatten, inclusief een hoofdletter en een uniek teken (!@#$&*).");
        }

        return errors;
    };

    const handleRegister = async (e) => {
        e.preventDefault();

        const errors = validateFormData();
        if (errors.length > 0) {
            setNotificatie(errors.join(" "));
            setTimeout(() => setNotificatie(""), 5000);
            return;
        }

        setIsLoading(true);

        try {
            // Zorg ervoor dat zakelijkeHuurderId wordt opgehaald
            await fetchZakelijkeHuurderId(); // Ophalen van zakelijkeHuurderId

            // Controleer opnieuw of zakelijkeHuurderId is ingesteld
            if (!zakelijkeHuurderId) {
                throw new Error("Zakelijke Huurder ID kon niet worden opgehaald. Probeer opnieuw.");
            }

            const data = {
                medewerkerNaam: formData.medewerkerNaam,
                medewerkerEmail: formData.email,
                wachtwoord: formData.wachtwoord,
                zakelijkeHuurderId: zakelijkeHuurderId,
                wagenparkBeheerderId: wagenparkBeheerderId,
                AspNetUserId: "string",
            };

            console.log("Data verstuurd naar register-bedrijfsmedewerker:", data);

            // API-aanroep met headers
            const response = await axios.post(
                `https://localhost:5033/api/Account/register-bedrijfsmedewerker`,
                data,
                {
                    headers: {
                        'Content-Type': 'application/json',
                    },
                }
            );

            console.log("Registratie succesvol:", response.data);
            setNotificatie(`Een e-mail is verstuurd naar ${formData.email}.`);
        } catch (error) {
            console.error("Error bij registratie:", error.response?.data || error.message);

            // Gedetailleerde foutmelding tonen
            setNotificatie(
                error.response?.data?.title ||
                error.response?.data?.message ||
                "Fout bij registratie. Probeer later opnieuw."
            );
        } finally {
            setTimeout(() => setNotificatie(""), 5000);
            setIsLoading(false);
            setFormData({ medewerkerNaam: "", email: "", wachtwoord: "" });
        }
    };

    return (
        <div className="register-container">
            <h1>Registreer Bedrijfsmedewerker</h1>
            <form className="form" onSubmit={handleRegister}>
                <label htmlFor="medewerkerNaam">Naam van de medewerker</label>
                <input
                    type="text"
                    id="medewerkerNaam"
                    name="medewerkerNaam"
                    value={formData.medewerkerNaam}
                    onChange={handleInputChange}
                    placeholder="Naam van medewerker"
                    required
                    aria-required="true"
                />

                <label htmlFor="email">E-mailadres</label>
                <input
                    type="email"
                    id="email"
                    name="email"
                    value={formData.email}
                    onChange={handleInputChange}
                    placeholder="E-mailadres"
                    required
                    aria-required="true"
                />

                <label htmlFor="wachtwoord">Wachtwoord</label>
                <input
                    type="password"
                    id="wachtwoord"
                    name="wachtwoord"
                    value={formData.wachtwoord}
                    onChange={handleInputChange}
                    placeholder="Wachtwoord"
                    required
                    aria-required="true"
                />

                <button type="submit" disabled={isLoading} aria-live="polite">
                    {isLoading ? "Bezig..." : "Registreer"}
                </button>
            </form>

            {notificatie && (
                <div className="notificatie-box" role="alert" aria-live="assertive">
                    {notificatie}
                </div>
            )}
        </div>
    );
};

export default BedrijfsMedewerkerRegister;
