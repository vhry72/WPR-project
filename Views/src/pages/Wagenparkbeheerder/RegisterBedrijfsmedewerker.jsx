import React, { useState } from "react";
import BedrijfsMedewerkerRequestService from "../../services/requests/BedrijfsMedewerkerRequestService";
import "../../styles/styles.css";
import "../../styles/Register.css";

const BedrijfsMedewerkerRegister = () => {
    const [formData, setFormData] = useState({
        medewerkerNaam: "",
        email: "",
        wachtwoord: "",
    });
    const [notificatie, setNotificatie] = useState("");
    const [isLoading, setIsLoading] = useState(false);

    const handleInputChange = (e) => {
        const { name, value } = e.target;
        setFormData((prev) => ({ ...prev, [name]: value }));
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
            // Voeg hier AspNetUserId toe op basis van de huidige sessie/ingelogde gebruiker
            const data = {
                ...formData,
                medewerkerEmail: formData.email, // Zorg dat dit overeenkomt met de backend-DTO
                AspNetUserId: "GUID-VAN-HUIDIGE-GEBRUIKER" // Voeg hier een geldige GUID toe
            };

            delete data.email; // Verwijder de verkeerde 'email' key

            console.log("Data verstuurd naar register-bedrijfsmedewerker:", data);

            await BedrijfsMedewerkerRequestService.register(data);
            setNotificatie(`Een e-mail is verstuurd naar ${formData.email}.`);
        } catch (error) {
            console.error("Error bij registratie:", error.response?.data || error.message);
            setNotificatie(
                error.response?.data?.title || "Fout bij registratie. Probeer later opnieuw."
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
