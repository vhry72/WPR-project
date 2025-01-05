import React, { useState, useEffect } from "react";
import "../styles/styles.css";
import ParticulierHuurdersRequestService from "../services/requests/ParticulierHuurderRequestService";
import { useNavigate, useLocation } from "react-router-dom";

const AccountwijzigingHuurders = () => {
    const [formData, setFormData] = useState({
        name: "",
        email: "",
        password: "",
    });
    const [isLoading, setIsLoading] = useState(false);
    const [errorMessage, setErrorMessage] = useState("");
    const navigate = useNavigate();
    const location = useLocation();
    const id = new URLSearchParams(location.search).get("id");

    useEffect(() => {
        const fetchUserDetails = async () => {
            setIsLoading(true);
            try {
                const response = await ParticulierHuurdersRequestService.getById(id);
                if (response && response.data) {
                    setFormData({
                        name: response.data.particulierNaam,
                        email: response.data.particulierEmail,
                        password: "", // Laat wachtwoord leeg
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

        if (id) fetchUserDetails();
    }, [id]);

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
                particulierId: id,
                particulierNaam: formData.name,
                particulierEmail: formData.email,
                wachtwoord: formData.password,
            };

            const response = await ParticulierHuurdersRequestService.update(id, payload);
            if (response.status === 200 || response.status === 204) {
                alert("Gebruikersgegevens succesvol bijgewerkt!");
                navigate("/home"); // Terug naar home of een andere pagina
            } else {
                setErrorMessage("Er is een fout opgetreden bij het bijwerken van de gegevens.");
            }
        } catch (error) {
            setErrorMessage("Er is een fout opgetreden bij het opslaan van de gegevens.");
        } finally {
            setIsLoading(false);
        }
    };

    return (
        <div className="accountwijziging-container">
            <h1 style={{ color: '#000000'}}>Gebruikersgegevens Wijzigen</h1>
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

                <label htmlFor="password">Wachtwoord</label>
                <input
                    type="password"
                    id="password"
                    name="password"
                    value={formData.password}
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

export default AccountwijzigingHuurders;
