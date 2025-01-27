import React, { useEffect, useState } from 'react';
import axios from 'axios';
import { useNavigate } from 'react-router-dom'; 
axios.defaults.withCredentials = true;



const SchadeClaimsList = () => {
    const [schademeldingen, setSchademeldingen] = useState([]);
    const [error, setError] = useState(null);
    const [loading, setLoading] = useState(true);
    const navigate = useNavigate(); // React Router hook for navigation


    useEffect(() => {
        axios.get(`https://localhost:5033/api/Schademelding`, { withCredentials: true })
            .then(response => {
                const filteredSchademeldingen = response.data.filter(schademelding => !schademelding.isAfgehandeld);
                setSchademeldingen(filteredSchademeldingen);
                setLoading(false);
            })
            .catch(err => {
                setError(err.message);
                setLoading(false);
            });
    }, []);

    const inBehandeling = async (id) => {
        const status = "InBehandeling";
        await axios.put(`https://localhost:5033/api/Schademelding/Afgehandeld/${id}/${status}`, { withCredentials: true })
            .then(() => {
                setSchademeldingen(prevState => prevState.filter(req => req.schademeldingId !== id));
                alert('Schademelding op In Behandeling gezet.');
            })
            .catch(err => alert(`Fout bij goedkeuren: ${err.message}`));
    };


    const Afgehandeld = async (id) => { 
        const status = "Afgehandeld";
        try {
            await axios.put(`https://localhost:5033/api/Schademelding/Afgehandeld/${id}/${status}`, { withCredentials: true });
            setSchademeldingen(prevState => prevState.filter(req => req.schademeldingId !== id));
            alert('Schademelding afgehandeld.');

            await axios.post(`https://localhost:5033/api/FrontOfficeMedewerker/${id}/true/Schademelding-afgehandeld`, { withCredentials: true });
        } catch (err) {
            alert(`Fout bij status aanpassing: ${err.message}`);
        }
    };

    const handleMaakSchadeClaimClick = () => {
        navigate(`/SchadeClaimMaken`); // Gebruik routeparameters
    };


    if (loading) return <div>Loading...</div>;
    if (error) return <div>Error: {error}</div>;
    return (
        <div>
            <h1>Schadeclaims</h1>
            {error && <p>{error}</p>} {/* Toon een foutmelding als er een fout optreedt */}
            <button onClick= {handleMaakSchadeClaimClick}>
                Nieuwe Schadeclaim Toevoegen
            </button>
            {schademeldingen.length === 0 ? (
                <p>Geen Schadeclaims op het moment.</p>
            ) : (
                <ul>
                        {schademeldingen.filter(schademelding => !schademelding.isAfgehandeld).map((schademelding) => (
                            <li key={schademelding.schademeldingId}> {/* Zorg ervoor dat de juiste ID wordt gebruikt */}
                            <p>Status: {schademelding.status}</p>
                            <p>Beschrijving: {schademelding.beschrijving}</p>
                            <p>Datum: {new Date(schademelding.datum).toLocaleDateString()}</p>
                            <p>soort onderoud: { schademelding.soortOnderhoud}</p>
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