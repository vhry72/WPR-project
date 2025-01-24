import React, { useState, useEffect } from "react";
import "../../styles/styles.css";
import ParticulierHuurdersRequestService from "../../services/requests/ParticulierHuurderRequestService";
import { useNavigate, useLocation } from "react-router-dom";
import JwtService from "../../services/JwtService";
import axios from "axios";
import { toast } from 'react-toastify';
import BackOfficeMedewerker from "./BackOfficeMedewerker";

const AccountwijzigingBackOffice = () => {
    const [formData, setFormData] = useState({
        name: "",
        email: "",
    });
    const [isLoading, setIsLoading] = useState(false);
    const [errorMessage, setErrorMessage] = useState("");
    const navigate = useNavigate();
    const [medewerkerId, setMedewerkerId] = useState(null);

    useEffect(() => {
        const fetchUserId = async () => {
            try {
                const userId = await JwtService.getUserId(); // Haal de gebruikers-ID op via de API
                console.log(userId);
                if (userId) {
                    setMedewerkerId(userId);
                } else {
                    console.error("Huurder ID kon niet worden opgehaald via de API.");
                }
            } catch (error) {
                console.error("Fout bij het ophalen van de huurder ID:", error);
            }
        };

        fetchUserId();
    }, []);

    // Haal de gebruikersgegevens op 
    useEffect(() => {
        const fetchUserDetails = async () => {
            setIsLoading(true);
            try {
                const response = await axios.get(`https://localhost:5033/api/BackOfficeMedewerker/${medewerkerId}/gegevens`)
                console.log(response.data)
                if (response && response.data) {
                    setFormData({
                        name: response.data.medewerkerNaam,
                        email: response.data.medewerkerEmail,
                    });
                } else {
                    setErrorMessage("Gebruikersgegevens konden niet worden geladen.");
                }
            } catch (error) {
                setErrorMessage("Er is een fout opgetreden bij het laden van de gegevens.");
            } finally {
                setIsLoading(false);
            }
        };

        if (medewerkerId) fetchUserDetails();
    }, [medewerkerId]);


    const handleChange = (event) => {
        const { id, value } = event.target;
        setFormData((prev) => ({ ...prev, [id]: value }));
    };

    const handleSubmit = async (event) => {
        event.preventDefault();
        setIsLoading(true);
        setErrorMessage("");

        try {
            const payload = {
                medewerkerNaam: formData.name,
                medewerkerEmail: formData.email,
            };

            const response = await axios.put(`https://localhost:5033/api/BackOfficeMedewerker/${medewerkerId}`, payload);
            if (response.status === 200) {
                toast.success("Gebruikersgegevens succesvol bijgewerkt!");
                navigate("/");
            } else {
                setErrorMessage("Er is een fout opgetreden bij het bijwerken van de gegevens.");
            }
        } catch (error) {
            setErrorMessage("Er is een fout opgetreden bij het opslaan van de gegevens.");
            console.log(error);
        } finally {
            setIsLoading(false);
        }
    };

    return (
        <div className="accountwijziging-container">
            <h1 style={{ color: '#000000' }}>Gebruikersgegevens Wijzigen</h1>
            <form className="form" onSubmit={handleSubmit}>
                <label htmlFor="name">Naam</label>
                <input
                    type="text"
                    id="name"
                    name="name"
                    value={formData.name}
                    onChange={handleChange}
                    required
                />

                <label htmlFor="email">Email</label>
                <input
                    type="email"
                    id="email"
                    name="email"
                    value={formData.email}
                    onChange={handleChange}
                    required
                />

                <button type="submit" className="submit-button" disabled={isLoading}>
                    {isLoading ? "Bezig met opslaan..." : "Opslaan"}
                </button>

                {errorMessage && <p className="error-message">{errorMessage}</p>}
            </form>
        </div>
    );
};

export default AccountwijzigingBackOffice;