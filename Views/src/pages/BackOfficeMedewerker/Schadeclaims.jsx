import React, { useEffect, useState } from 'react';
import axios from 'axios';
import { Navigate } from 'react-router-dom';

const SchadeClaimsList = () => {
    const [schademeldingen, setSchademeldingen] = useState([]);
    const [error, setError] = useState(null);
    const [loading, setLoading] = useState(true);



    useEffect(() => {
        axios.get('https://localhost:5033/api/Schademelding')
            .then(response => {
                setSchademeldingen(response.data);
                setLoading(false);
            })
            .catch(err => {
                setError(err.message);
                setLoading(false);
            });
    }, []);

    const inBehandeling = (id) => {
        axios.put(`https://localhost:5033/api/Schademelding/inBehandeling/${id}/"In Behandeling"`)
            .then(() => {
                setSchademeldingen(prevState => prevState.filter(req => req.schademeldingId !== id));
                alert('Schademelding op In Behandeling gezet.');
            })
            .catch(err => alert(`Fout bij goedkeuren: ${err.message}`));
    };
    const Afgehandeld = (id) => {
        axios.put(`https://localhost:5033/api/Schademelding/Afgehandeld/${id}/"Afgehandeld"`)
            .then(() => {
                setSchademeldingen(prevState => prevState.filter(req => req.schademeldingId !== id));
                alert('Schademelding afgehandeld.');
            })
            .catch(err => alert(`Fout bij status aanpassing: ${err.message}`));
    };




    if (loading) return <div>Loading...</div>;
    if (error) return <div>Error: {error}</div>;
    return (
        <div>
            <h1>Schadeclaims</h1>
            {error && <p>{error}</p>} {/* Toon een foutmelding als er een fout optreedt */}

            {schademeldingen.length === 0 ? (
                <p>Geen Schadeclaims op het moment.</p>
            ) : (
                <ul>
                    {schademeldingen.map((schademelding) => (
                        <li key={schademelding.schademeldingId}> {/* Zorg ervoor dat de juiste ID wordt gebruikt */}
                            <p>Voertuig: {schademelding.voertuig.merk} {schademelding.voertuig.model}</p>
                            <p>Status: {schademelding.status}</p>
                            <p>Beschrijving: {schademelding.beschrijving}</p>
                            <p>Datum: {new Date(schademelding.datum).toLocaleDateString()}</p>
                            <button onClick={() => inBehandeling(schademelding.schademeldingId)}>
                                Zet op In Behandeling.
                            </button>
                            <button onClick={() => Afgehandeld(schademelding.schademeldingId)}>
                                Zet op Afgehandeld
                            </button>
                        </li>
                    ))}
                </ul>
            )}
        </div>
    );
};

export default SchadeClaimsList;