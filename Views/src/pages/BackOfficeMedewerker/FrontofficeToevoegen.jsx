import React, { useState } from "react";
import axios from "axios";
import { useNavigate } from "react-router-dom";
import "../../styles/FrontofficeBeheer.css";

const API_URL = "https://localhost:5033";

const FrontofficeToevoegen = () => {
    const [formData, setFormData] = useState({
        medewerkerNaam: "",
        medewerkerEmail: "",
        wachtwoord: "",
    });
    const navigate = useNavigate();

    const handleInputChange = (e) => {
        const { name, value } = e.target;
        setFormData({ ...formData, [name]: value });
    };

    const handleToevoegen = async () => {
        try {
            await axios.post(`${API_URL}/api/Account/register-frontoffice`, {
                ...formData,
                frontofficeMedewerkerId: uuidv4(),
                emailBevestigingToken: uuidv4(),
                isEmailBevestigd: false,
                aspNetUserId: "string",
            });
            alert("Medewerker succesvol toegevoegd!");
            navigate(`/FrontofficeTonen`);
        } catch (error) {
            console.error("Fout bij het toevoegen van medewerker:", error);
        }
    };

    return (
        <div className="container">
            <h2>Nieuwe Medewerker Toevoegen</h2>
            <form>
                <div className="form-group">
                    <label>Naam:</label>
                    <input
                        type="text"
                        name="medewerkerNaam"
                        value={formData.medewerkerNaam}
                        onChange={handleInputChange}
                        required
                    />
                </div>
                <div className="form-group">
                    <label>E-mailadres:</label>
                    <input
                        type="email"
                        name="medewerkerEmail"
                        value={formData.medewerkerEmail}
                        onChange={handleInputChange}
                        required
                    />
                </div>
                <div className="form-group">
                    <label>Wachtwoord:</label>
                    <input
                        type="password"
                        name="wachtwoord"
                        value={formData.wachtwoord}
                        onChange={handleInputChange}
                        required
                    />
                </div>
                <button type="button" onClick={handleToevoegen} className="button">
                    Toevoegen
                </button>
            </form>
        </div>
    );
};

export default FrontofficeToevoegen;
