import React, { useEffect, useState } from "react";
import { useParams, useNavigate } from "react-router-dom";
import axios from "axios";

const VoertuigDetails = () => {
    const { voertuigId } = useParams();
    const navigate = useNavigate();
    const [voertuig, setVoertuig] = useState(null);
    const [error, setError] = useState(null);
    const [notitie, setNotitie] = useState(""); // Notitie state voor de input
    const [isLoading, setIsLoading] = useState(true); // Toestemming voor het laden van data
    const [id, setVoertuigId] = useState(null);

    useEffect(() => {
        const fetchVoertuigDetails = async () => {
            try {
                const response = await axios.get(
                    `https://localhost:5033/api/Voertuig/${voertuigId}`);
                setVoertuig(response);
                console.log(response);
                setNotitie(response.notitie || ""); // Stel de notitie in, indien aanwezig
                setIsLoading(false);
                setVoertuigId(response.voertuigId);
            } catch (err) {
                console.error("Fout bij ophalen voertuigdetails:", err);
                setError(err);
                setIsLoading(false);
            }
        };

        fetchVoertuigDetails();
    }, [voertuigId]);

    const handleNotitieChange = (event) => {
        setNotitie(event.target.value); // Wijzig de notitie in de state
    };

    const handleNotitieSubmit = async (event) => {
        event.preventDefault();
        try {
            const payload =
            {
                notitie
            };
            console.log(voertuigId);
            console.log(payload);
            const response = await axios.put(`https://localhost:5033/api/Voertuig/maaknotitie/${id}`, payload, { withCredentials: true });

            if (response.status === 200) {
                alert("Notitie succesvol bijgewerkt!");
            }
        } catch (err) {
            console.error("Fout bij het bijwerken van de notitie:", err); // Log volledige fout
            setError(err);
        }
    };
    const handleWijzigingenInkijkClick = () => {
        navigate(`/WijzigingenVoertuig/${voertuig.voertuigId}`);
    };





    if (error) {
        return <div>Er is een fout opgetreden: {error.message}</div>;
    }

    if (isLoading) {
        return <div>Gegevens laden...</div>;
    }

    return (
        <div>
            <h1>Voertuig Notitie Toevoegen</h1>
            <p>Merk: {voertuig.merk}</p>
            <p>Model: {voertuig.model}</p>
            <p>Prijs Per Dag: {voertuig.prijsPerDag}</p>
            <p>Kleur: {voertuig.kleur}</p>
            <p>Bouwjaar: {voertuig.bouwjaar}</p>
            <p>Notitie: {voertuig.notitie}</p>

            <form onSubmit={handleNotitieSubmit}>
                <div>
                    <label htmlFor="notitie">Notitie:</label>
                    <textarea
                        id="notitie"
                        value={notitie}
                        onChange={handleNotitieChange}
                        rows="4"
                        cols="50"
                    ></textarea>
                </div>
                <button type="submit">Notitie Bijwerken</button>
            </form>
            <button onClick= {handleWijzigingenInkijkClick}>
                Kijk naar Wijzigingen
            </button>
        </div>
    );
};

export default VoertuigDetails;