import React, { useState } from "react";
import "../styles/styles.css";
import zakelijkeHuurderRequestService from "../services/requests/ZakelijkeHuurderRequestService"; 
import particulierHuurdersRequestService from "../services/requests/ParticulierHuurderRequestService";
import { v4 as uuidv4 } from 'uuid';

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
        Zakelijkemail: "",
        Zakelijkwachtwoord: "",
        bedrijfsnaam: "",
        kantoorAdres: "",
        zakelijkTelefoonnummer: "",
        kvkNummer: "",

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

    const handlePostParticulier = async (event) => {
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
                particulierId: uuidv4(),
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

    const handlePostZakelijk = async (event) => {
        event.preventDefault();
        setIsLoading(true);
        setErrorMessage("");

        try {
            // Valideer verplichte velden
            if (!formData.Zakelijkemail || !formData.Zakelijkwachtwoord) {
                setErrorMessage("Vul alle verplichte velden in!");
                setIsLoading(false);
                return;
            }

            // Maak payload
            const payload = {
                zakelijkeId: uuidv4(),
                adres: formData.kantoorAdres,
                kvkNummer: formData.kvkNummer,
                bedrijsEmail: formData.Zakelijkemail,
                emailBevestigingToken: 'string',
                isEmailBevestigd: false,
                telNummer: formData.zakelijkTelefoonnummer,
                bedrijfsnaam: formData.bedrijfsnaam,
                wachtwoord: formData.Zakelijkwachtwoord,
            };

            console.log("Verstuurde payload:", payload);

            // Verzoek naar backend
            const response = await zakelijkeHuurderRequestService.register(payload);
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
                <form id="ParticulierForm" className="form" onSubmit={handlePostParticulier}>
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
                <form id="ZakelijkForm" className="form" onSubmit={handlePostZakelijk}>
                    <label htmlFor="kantoorAdres">Kantoor Adres</label>
                    <input
                        type="text"
                        id="kantoorAdres"
                        name="kantoorAdres"
                        value={formData.kantoorAdres}
                        onChange={handleChange}
                        required
                    />

                    <label htmlFor="kvkNummer">KVK-nummer</label>
                    <input
                        type="text"
                        id="kvkNummer"
                        name="kvkNummer"
                        value={formData.kvkNummer}
                        onChange={handleChange}
                        required
                    />

                    <label htmlFor="Zakelijkemail">Email</label>
                    <input
                        type="text"
                        id="Zakelijkemail"
                        name="Zakelijkemail"
                        value={formData.Zakelijkemail}
                        onChange={handleChange}
                        required
                    />

                    <label htmlFor="zakelijkTelefoonnummer">Telefoonnummer</label>
                    <input
                        type="text"
                        id="zakelijkTelefoonnummer"
                        name="zakelijkTelefoonnummer"
                        value={formData.zakelijkTelefoonnummer}
                        onChange={handleChange}
                        required
                    />

                    <label htmlFor="bedrijfsnaam">Bedrijfsnaam</label>
                    <input
                        type="text"
                        id="bedrijfsnaam"
                        name="bedrijfsnaam"
                        value={formData.bedrijfsnaam}
                        onChange={handleChange}
                        required
                    />

                    <label htmlFor="Zakelijkwachtwoord">Wachtwoord</label>
                    <input
                        type="text"
                        id="Zakelijkwachtwoord"
                        name="Zakelijkwachtwoord"
                        value={formData.Zakelijkwachtwoord}
                        onChange={handleChange}
                        required
                    />

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