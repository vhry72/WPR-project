import React, { useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import VoertuigRequestService from "../../services/requests/VoertuigRequestService";

const VoertuigDetails = () => {
    const { voertuigId } = useParams(); // Haalt het ID uit de URL
    const [voertuig, setVoertuig] = useState(null);
    const [error, setError] = useState(null);

    useEffect(() => {
        const fetchVoertuigDetails = async () => {
            try {
                const response = await VoertuigRequestService.getById(voertuigId);
                setVoertuig(response); // Zet de opgehaalde data
            } catch (err) {
                console.error("Fout bij ophalen voertuigdetails:", err);
                setError(err);
            }
        };

        fetchVoertuigDetails();
    }, [voertuigId]);

    if (error) {
        return <div>Er is een fout opgetreden: {error.message}</div>;
    }

    if (!voertuig) {
        return <div>Gegevens laden...</div>;
    }

    return (
        <div>
            <h1>Voertuig Details</h1>
            <p>Merk: {voertuig.merk}</p>
            <p>Model: {voertuig.model}</p>
            <p>Prijs Per Dag: {voertuig.prijsPerDag}</p>
            <p>Kleur: {voertuig.kleur}</p>
        </div>
    );
};

export default VoertuigDetails;

