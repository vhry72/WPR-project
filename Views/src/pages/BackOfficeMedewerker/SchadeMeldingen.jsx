import React, { useEffect, useState } from "react";
import axios from "axios";

const SchadeMeldingenList = () => {
    const [schademeldingen, setSchademeldingen] = useState([]);
    const [voertuigen, setVoertuigen] = useState({});
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null);

    const fetchSchademeldingen = () => {
        axios.get('https://localhost:5033/api/Schademelding')
            .then(response => {
                setSchademeldingen(response.data);
                setLoading(false);
                response.data.forEach(schademelding => {
                    fetchVoertuigen(schademelding.voertuigId);
                });
            })
            .catch(err => {
                setError(err.message);
                setLoading(false);
            });
    };

    const fetchVoertuigen = (voertuigId) => {
        if (!voertuigen[voertuigId]) { // Voorkom herhaalde verzoeken voor hetzelfde voertuig
            axios.get(`https://localhost:5033/api/Voertuig/${voertuigId}`)
                .then(response => {
                    setVoertuigen(prev => ({ ...prev, [voertuigId]: response.data }));
                })
                .catch(err => {
                    setError(err.message);
                    setLoading(false);
                });
        }
    };

    useEffect(() => {
        fetchSchademeldingen(); // Initieel ophalen van gegevens
        const interval = setInterval(fetchSchademeldingen, 5000); // Polling elke 5 seconden
        return () => clearInterval(interval); // Opruimen bij unmounten
    }, []);

    const inReparatie = (id) => {
        axios.put(`https://localhost:5033/api/Schademelding/InReparatie/${id}/InReparatie`, { withCredentials: true })
            .then(() => {
                setSchademeldingen(prevState => prevState.filter(req => req.schademeldingId !== id));
                alert('Schademelding op In Reparatie gezet.');
            })
            .catch(err => alert(`Fout bij het verwerken: ${err.message}`));
    };

    if (loading) return <div>Loading...</div>;
    if (error) return <div>Error: {error}</div>;

    return (
        <div>
            <h1>Schademeldingen</h1>
            {schademeldingen.length === 0 ? (
                <p>Geen Schademeldingen op het moment.</p>
            ) : (
                <ul>
                    {schademeldingen.map((schademelding) => (
                        <li key={schademelding.schademeldingId}>
                            {voertuigen[schademelding.voertuigId] && (
                                <p>Voertuig: {voertuigen[schademelding.voertuigId].merk} {voertuigen[schademelding.voertuigId].model}</p>
                            )}
                            <p>Status: {schademelding.status}</p>
                            <p>Beschrijving: {schademelding.beschrijving}</p>
                            <p>Datum: {new Date(schademelding.datum).toLocaleDateString()}</p>
                            <button onClick={() => inReparatie(schademelding.schademeldingId)}>
                                Zet op In Reparatie
                            </button>
                        </li>
                    ))}
                </ul>
            )}
        </div>
    );
};

export default SchadeMeldingenList;
