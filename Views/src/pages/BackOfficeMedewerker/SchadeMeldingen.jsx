import React, { useEffect, useState } from 'react';
import axios from 'axios';

const SchadeMeldingenList = () => {
    const [schademeldingen, setSchademeldingen] = useState([]);
    const [error, setError] = useState(null);
    const [loading, setLoading] = useState(true);

    const fetchSchademeldingen = () => {
        axios.get('https://localhost:5033/api/Schademelding')
            .then(response => {
                setSchademeldingen(response.data);
                setLoading(false);
            })
            .catch(err => {
                setError(err.message);
                setLoading(false);
            });
    };

    useEffect(() => {
        fetchSchademeldingen(); // Initieel ophalen van gegevens

        // Periodiek ophalen van gegevens (polling)
        const interval = setInterval(() => {
            fetchSchademeldingen();
        }, 5000); // Elke 5 seconden

        return () => clearInterval(interval); // Opruimen bij unmounten
    }, []);

    const inReparatie = (id) => {
        axios.put(`https://localhost:5033/api/Schademelding/InReparatie/${id}/"In Reparatie"`)
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
            {error && <p>{error}</p>}

            {schademeldingen.length === 0 ? (
                <p>Geen Schademeldingen op het moment.</p>
            ) : (
                <ul>
                    {schademeldingen.map((schademelding) => (
                        <li key={schademelding.schademeldingId}>
                            <p>Voertuig: {schademelding.voertuig.merk} {schademelding.voertuig.model}</p>
                            <p>Status: {schademelding.status}</p>
                            <p>Beschrijving: {schademelding.beschrijving}</p>
                            <p>Datum: {new Date(schademelding.datum).toLocaleDateString()}</p>

                            <button onClick={() => inReparatie(schademelding.schademeldingId)}>
                                Zet op In Reparatie.
                            </button>
                        </li>
                    ))}
                </ul>
            )}
        </div>
    );
};

export default SchadeMeldingenList;