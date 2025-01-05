import React, { useState, useRef, useEffect } from "react";
import { QRCodeSVG } from "qrcode.react";
import ParticulierHuurdersRequestService from "../services/requests/ParticulierHuurderRequestService";
import ZakelijkeHuurderRequestService from "../services/requests/ZakelijkeHuurderRequestService";
import "../styles/Register.css";

const Register = () => {
    const [formData, setFormData] = useState({
        particulierEmail: "",
        particulierNaam: "",
        wachtwoord: "",
        adress: "",
        postcode: "",
        woonplaats: "",
        telefoonnummer: "",
        kantoorAdres: "",
        kvkNummer: "",
        Zakelijkemail: "",
        zakelijkTelefoonnummer: "",
        bedrijfsnaam: "",
        Zakelijkwachtwoord: "",
    });
    const [qrCode, setQrCode] = useState("");
    const [isLoading, setIsLoading] = useState(false);
    const [errorMessage, setErrorMessage] = useState("");
    const [activeTab, setActiveTab] = useState("particulier");
    const qrCodeRef = useRef(null);

    useEffect(() => {
        if (qrCode && qrCodeRef.current) {
            qrCodeRef.current.scrollIntoView({ behavior: "smooth" });
        }
    }, [qrCode]);

    const handleChange = (e) => {
        setFormData({ ...formData, [e.target.name]: e.target.value });
    };

    const handleTabClick = (tab) => {
        setActiveTab(tab);
        setQrCode("");
    };

    const handlePostParticulier = async (event) => {
        event.preventDefault();
        setIsLoading(true);
        setErrorMessage("");

        try {
            const payload = {
                particulierEmail: formData.particulierEmail,
                particulierNaam: formData.particulierNaam,
                wachtwoord: formData.wachtwoord,
                adress: formData.adress,
                postcode: formData.postcode,
                woonplaats: formData.woonplaats,
                telefoonnummer: formData.telefoonnummer,
            };

            const response = await ParticulierHuurdersRequestService.register(payload);
            const { qrCodeUri } = response.data;

            setQrCode(qrCodeUri);
            alert("Registratie succesvol! Scan de QR-code om tweestapsverificatie in te schakelen.");
        } catch (error) {
            setErrorMessage(
                error.response?.data?.message || "Serverfout, probeer later opnieuw."
            );
        } finally {
            setIsLoading(false);
        }
    };

    const handlePostZakelijk = async (event) => {
        event.preventDefault();
        setIsLoading(true);
        setErrorMessage("");

        try {
            const payload = {
                adres: formData.kantoorAdres,
                KVKNummer: parseInt(formData.kvkNummer, 10),
                bedrijfsEmail: formData.Zakelijkemail,
                telNummer: formData.zakelijkTelefoonnummer,
                bedrijfsNaam: formData.bedrijfsnaam,
                wachtwoord: formData.Zakelijkwachtwoord,
            };

            const response = await ZakelijkeHuurderRequestService.register(payload);
            const { qrCodeUri } = response.data;

            setQrCode(qrCodeUri);
            alert("Zakelijke registratie succesvol! Scan de QR-code om tweestapsverificatie in te schakelen.");
        } catch (error) {
            setErrorMessage(
                error.response?.data?.message || "Serverfout, probeer later opnieuw."
            );
        } finally {
            setIsLoading(false);
        }
    };

    return (
        <div className="register-container">
            <h1>Registreren</h1>
            <div className="register-tabs">
                <button
                    className={`tab ${activeTab === "particulier" ? "active" : ""}`}
                    onClick={() => handleTabClick("particulier")}
                    aria-selected={activeTab === "particulier"}
                >
                    Particulier
                </button>
                <button
                    className={`tab ${activeTab === "zakelijk" ? "active" : ""}`}
                    onClick={() => handleTabClick("zakelijk")}
                    aria-selected={activeTab === "zakelijk"}
                >
                    Zakelijk
                </button>
            </div>

            {activeTab === "particulier" && (
                <form id="ParticulierForm" className="form" onSubmit={handlePostParticulier}>
                    <label htmlFor="particulierEmail">E-mailadres</label>
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
                    <button type="button" className="login-button" onClick={() => window.location.href = '/LoginVoorWijziging'}>
                        Ga naar het inlogscherm
                    </button>

                    {/* QR-code tonen na succesvolle registratie */}
                    {qrCode && (
                        <div className="qr-code-container" ref={qrCodeRef} aria-live="polite">
                            <p>Scan de onderstaande QR-code in je authenticator-app:</p>
                            <QRCodeSVG value={qrCode} size={200} aria-label="QR-code voor authenticatie" />
                            <div className="qr-code-details" role="region" aria-labelledby="qr-code-details-heading">
                                <h2 id="qr-code-details-heading">Handmatige configuratie</h2>
                                <p><strong>Accountnaam:</strong> {formData.particulierEmail}</p>
                                <p><strong>Geheime sleutel:</strong> {new URLSearchParams(qrCode.split('?')[1]).get('secret')}</p>
                            </div>
                        </div>
                    )}
                </form>
            )}

            {activeTab === "zakelijk" && (
                <form id="ZakelijkForm" className="form" onSubmit={handlePostZakelijk}>
                    <label htmlFor="kantoorAdres">Kantooradres</label>
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
                    <label htmlFor="Zakelijkemail">E-mailadres</label>
                    <input
                        type="email"
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
                        type="password"
                        id="Zakelijkwachtwoord"
                        name="Zakelijkwachtwoord"
                        value={formData.Zakelijkwachtwoord}
                        onChange={handleChange}
                        required
                    />
                    <button type="submit" className="register-button" disabled={isLoading}>
                        {isLoading ? "Verwerken..." : "Registreren"}
                    </button>
                    <button type="button" className="login-button" onClick={() => window.location.href = '/LoginVoorWijziging'}>
                        Ga naar het inlogscherm
                    </button>   

                    {/* QR-code tonen na succesvolle registratie */}
                    {qrCode && (
                        <div className="qr-code-container" ref={qrCodeRef} aria-live="polite">
                            <p>Scan de onderstaande QR-code in je authenticator-app:</p>
                            <QRCodeSVG value={qrCode} size={200} aria-label="QR-code voor authenticatie" />
                            <div className="qr-code-details" role="region" aria-labelledby="qr-code-details-heading">
                                <h2 id="qr-code-details-heading">Handmatige configuratie</h2>
                                <p><strong>Accountnaam:</strong> {formData.Zakelijkemail}</p>
                                <p><strong>Geheime sleutel:</strong> {new URLSearchParams(qrCode.split('?')[1]).get('secret')}</p>
                            </div>
                        </div>
                    )}
                </form>
            )}

            {errorMessage && <div className="error-message">{errorMessage}</div>}
            
        </div>
    );
};

export default Register;