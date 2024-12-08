import React, { useState } from "react";
import "../styles/styles.css";
import particulierHuurdersRequestService from "../services/requests/ParticulierHuurderRequestService";

const Register = () => {
    const [activeTab, setActiveTab] = useState("particulier");
    const [formData, setFormData] = useState({
        particulierEmail: "",
        particulierNaam: "",
        wachtwoord: "",
        adress: "",
        postcode: "",
        woonplaats: "",
        telefoonnummer: "",
    });

    const [isLoading, setIsLoading] = useState(false);
    const [errorMessage, setErrorMessage] = useState("");

    const handleTabClick = (tab) => {
        setActiveTab(tab);
    };

    const handleChange = (event) => {
        const { id, value } = event.target;
        setFormData((prev) => ({ ...prev, [id]: value }));
    };

    const handlePost = async (event) => {
        event.preventDefault();
        setIsLoading(true);
        setErrorMessage("");

        try {
            // Valideer verplichte velden
            if (!formData.particulierEmail || !formData.wachtwoord) {
                setErrorMessage("Vul alle verplichte velden in!");
                setIsLoading(false);
                return;
            }

            // Maak payload
            const payload = {
                particulierId: '100',
                particulierEmail: formData.particulierEmail,
                particulierNaam: formData.particulierNaam,
                EmailBevestigingToken: '6asdadwq',
                IsEmailBevestigd: false,
                wachtwoord: formData.wachtwoord,
                adress: formData.adress,
                postcode: formData.postcode,
                woonplaats: formData.woonplaats,
                telefoonnummer: formData.telefoonnummer,
            };

            console.log("Verstuurde payload:", payload);

            // Verzoek naar backend
            const response = await particulierHuurdersRequestService.register(payload);
            alert("Gebruiker succesvol geregistreerd!");
            console.log("User registered successfully:", response.data);

        } catch (error) {
            // Foutafhandeling
            if (error.response) {
                setErrorMessage(error.response.data?.message || "Serverfout, probeer later opnieuw.");
            } else if (error.request) {
                setErrorMessage("Geen antwoord van de server. Controleer je verbinding.");
            } else {
                setErrorMessage(`Onbekende fout: ${error.message}`);
            }
        } finally {
            setIsLoading(false);
        }
    };

    return (
        <div className="register-container">
            <h1>Registreren</h1>
            <div className="tabs">
                <div
                    className={`tab ${activeTab === "particulier" ? "active" : ""}`}
                    onClick={() => handleTabClick("particulier")}
                >
                    Particulier
                </div>
                <div
                    className={`tab ${activeTab === "zakelijk" ? "active" : ""}`}
                    onClick={() => handleTabClick("zakelijk")}
                >
                    Zakelijk
                </div>
            </div>

            {activeTab === "particulier" && (
                <form id="ParticulierForm" className="form" onSubmit={handlePost}>
                    <label htmlFor="particulierEmail">E-mail</label>
                    <input
                        type="email"
                        id="particulierEmail"
                        name="particulierEmail"
                        value={formData.particulierEmail}
                        onChange={handleChange}
                        required
                    />
                    <label htmlFor="particulierNaam">Naam</label>
                    <input
                        type="text"
                        id="particulierNaam"
                        name="particulierNaam"
                        value={formData.particulierNaam}
                        onChange={handleChange}
                        required
                    />
                    <label htmlFor="wachtwoord">Wachtwoord</label>
                    <input
                        type="password"
                        id="wachtwoord"
                        name="wachtwoord"
                        value={formData.wachtwoord}
                        onChange={handleChange}
                        required
                    />
                    <label htmlFor="adress">Adres</label>
                    <input
                        type="text"
                        id="adress"
                        name="adress"
                        value={formData.adress}
                        onChange={handleChange}
                        required
                    />
                    <label htmlFor="postcode">Postcode</label>
                    <input
                        type="text"
                        id="postcode"
                        name="postcode"
                        value={formData.postcode}
                        onChange={handleChange}
                        required
                    />
                    <label htmlFor="woonplaats">Woonplaats</label>
                    <input
                        type="text"
                        id="woonplaats"
                        name="woonplaats"
                        value={formData.woonplaats}
                        onChange={handleChange}
                        required
                    />
                    <label htmlFor="telefoonnummer">Telefoonnummer</label>
                    <input
                        type="text"
                        id="telefoonnummer"
                        name="telefoonnummer"
                        value={formData.telefoonnummer}
                        onChange={handleChange}
                        required
                    />
                    <button type="submit" className="register-button" disabled={isLoading}>
                        {isLoading ? "Verwerken..." : "Registreren"}
                    </button>
                </form>
            )}
            {/* Zakelijk Formulier */}
            {activeTab === "zakelijk" && (
                <form id="ZakelijkForm" className="form" action="/abonnement" method="get">
                    <label htmlFor="email-zakelijk">E-mail</label>
                    <input type="email" id="email-zakelijk" name="email" required />
                    <label htmlFor="wachtwoord-zakelijk">Wachtwoord</label>
                    <input type="password" id="wachtwoord-zakelijk" name="wachtwoord" required />
                    <label htmlFor="bedrijfsnaam">Bedrijfsnaam</label>
                    <input type="text" id="bedrijfsnaam" name="bedrijfsnaam" required />
                    <label htmlFor="adres-zakelijk">Adres</label>
                    <input type="text" id="adres-zakelijk" name="adres" required />
                    <label htmlFor="kvk-nummer">KVK-nummer</label>
                    <input type="text" id="kvk-nummer" name="kvk-nummer" required />
                    <button type="submit" className="register-button">
                        Registreren
                    </button>
                </form>
            )}
            {errorMessage && <div className="error-message">{errorMessage}</div>}
        </div>
    );
};

export default Register;