import React, { useState, useEffect } from "react";
import { useParams, useNavigate } from "react-router-dom";
import axios from "axios";
import "../../styles/FrontofficeBeheer.css";

const API_URL = "https://localhost:5033";

const FrontofficeDetails = () => {
    const { medewerkerId } = useParams();
    const [medewerker, setMedewerker] = useState(null);
    const navigate = useNavigate();

    useEffect(() => {
        const fetchMedewerker = async () => {
            try {
                const response = await axios.get(`${API_URL}/api/FrontOfficeMedewerker/Get/${medewerkerId}`);
                setMedewerker(response.data);
            } catch (error) {
                console.error("Fout bij het ophalen van medewerker:", error);
            }
        };

        fetchMedewerker();
    }, [medewerkerId]);

    const handleDelete = async () => {
        const bevestigen = window.confirm("Weet je zeker dat je deze medewerker wilt verwijderen?");
        if (bevestigen) {
            try {
                await axios.delete(`${API_URL}/api/FrontOfficeMedewerker/Delete/${medewerkerId}`);
                alert("Medewerker succesvol verwijderd!");
                navigate(`/FrontofficeTonen`);
            } catch (error) {
                console.error("Fout bij het verwijderen van medewerker:", error);
            }
        }
    };

    if (!medewerker) {
        return <div>Gegevens laden...</div>;
    }

    return (
        <div className="container">
            <h2>Medewerker Details</h2>
            <p><strong>Naam:</strong> {medewerker.medewerkerNaam}</p>
            <p><strong>E-mailadres:</strong> {medewerker.medewerkerEmail}</p>
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
