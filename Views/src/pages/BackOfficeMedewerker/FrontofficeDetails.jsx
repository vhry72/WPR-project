import React, { useEffect, useState } from "react";
import { useNavigate, useLocation } from "react-router-dom";
import axios from "axios";
import "../../styles/FrontofficeBeheer.css";

const API_URL = "https://localhost:5033";

const FrontofficeDetails = () => {
    const location = useLocation();
    const { medewerkerId } = location.state;
    const navigate = useNavigate();
    const [medewerker, setMedewerker] = useState(null);
    const [formData, setFormData] = useState({ medewerkerNaam: "", medewerkerEmail: "" });
    const [error, setError] = useState(null);
    const [loading, setLoading] = useState(true);

    useEffect(() => {
        const fetchMedewerkerDetails = async () => {
            try {
                const response = await axios.get(`${API_URL}/api/FrontOfficeMedewerker/${medewerkerId}/gegevens`);
                setMedewerker(response.data);
                setFormData({
                    medewerkerNaam: response.data.medewerkerNaam,
                    medewerkerEmail: response.data.medewerkerEmail,
                });
            } catch (err) {
                console.error("Fout bij ophalen medewerkerdetails:", err);
                setError("Kan medewerker niet ophalen. Controleer of het ID klopt.");
            } finally {
                setLoading(false);
            }
        };

        if (medewerkerId) {
            fetchMedewerkerDetails();
        } else {
            setError("Medewerker ID ontbreekt in de URL.");
            setLoading(false);
        }
    }, [medewerkerId]);

    const handleInputChange = (e) => {
        const { name, value } = e.target;
        setFormData((prev) => ({ ...prev, [name]: value }));
    };

    const handleMedewerkerChange = async (e) => {
        e.preventDefault();
        try {
            const response = await axios.put(
                `${API_URL}/api/FrontOfficeMedewerker/${medewerkerId}`,
                formData
            );
            if (response.status === 200) {
                alert("Gegevens succesvol bijgewerkt!");
                setMedewerker((prev) => ({ ...prev, ...formData }));
            }
        } catch (err) {
            console.error("Fout bij het bijwerken van de gegevens:", err);
            setError("Gegevens bijwerken mislukt. Probeer opnieuw.");
        }
    };

    const handleDelete = async () => {
        const bevestigen = window.confirm("Weet je zeker dat je deze medewerker wilt verwijderen?");
        if (bevestigen) {
            try {
                await axios.delete(`${API_URL}/api/FrontOfficeMedewerker/Delete/${medewerkerId}`);
                alert("Medewerker succesvol verwijderd!");
                navigate(`/FrontofficeTonen`);
            } catch (err) {
                console.error("Fout bij het verwijderen van medewerker:", err);
                setError("Verwijderen mislukt. Probeer opnieuw.");
            }
        }
    };

    if (loading) {
        return <div>Gegevens laden...</div>;
    }

    if (error) {
        return <div className="error">{error}</div>;
    }

    if (!medewerker) {
        return <div>Geen gegevens gevonden voor deze medewerker.</div>;
    }

    return (
        <div className="container">
            <h2>Medewerker Details</h2>
            <form onSubmit={handleMedewerkerChange}>
                <div>
                    <label>Naam:</label>
                    <input
                        type="text"
                        name="medewerkerNaam"
                        value={formData.medewerkerNaam}
                        onChange={handleInputChange}
                    />
                </div>
                <div>
                    <label>E-mailadres:</label>
                    <input
                        type="email"
                        name="medewerkerEmail"
                        value={formData.medewerkerEmail}
                        onChange={handleInputChange}
                    />
                </div>
                <button type="submit" className="button">
                    Verander Gegevens
                </button>
            </form>
            <button onClick={handleDelete} className="button delete">
                Verwijderen
            </button>
            <button onClick={() => navigate(`/FrontofficeTonen`)} className="button">
                Terug naar Overzicht
            </button>
        </div>
    );
};

export default FrontofficeDetails;
