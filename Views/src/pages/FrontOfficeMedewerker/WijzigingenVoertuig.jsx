import React, { useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import axios from "axios";

const VoertuigNotities = () => {
    const { voertuigId } = useParams();
    const [notities, setNotities] = useState([]);
    const [error, setError] = useState(null);
    const [isLoading, setIsLoading] = useState(true);

    useEffect(() => {
        const fetchNotities = async () => {
            try {
                const response = await axios.get(`https://localhost:5033/api/VoertuigNotities/${voertuigId}`);
                setNotities(response.data);
                setIsLoading(false);
            } catch (err) {
                console.error("Fout bij ophalen notities:", err);
                setError(err);
                setIsLoading(false);
            }
        };

        fetchNotities();
    }, [voertuigId]);

    if (isLoading) {
        return <div>Gegevens laden...</div>;
    }

    if (error) {
        return <div>Er is een fout opgetreden: {error.message}</div>;
    }

    return (
        <div>
            <h1>Notities voor Voertuig</h1>
            {notities.length > 0 ? (
                <ul>
                    {notities.map((notitie) => (
                        <li key={notitie.notitieId}>
                            <p><strong>Datum:</strong> {new Date(notitie.notitieDatum).toLocaleDateString()}</p>
                            <p><strong>Notitie:</strong> {notitie.notitie}</p>
                        </li>
                    ))}
                </ul>
            ) : (
                <p>Geen notities gevonden voor dit voertuig.</p>
            )}
        </div>
    );
};

export default VoertuigNotities;
