import React, { useEffect, useState } from 'react';
import axios from 'axios';

const HuurVerzoekenList = () => {
    const [actieveVerzoeken, setActieveVerzoeken] = useState([]);
    const [afgekeurdeVerzoeken, setAfgekeurdeVerzoeken] = useState([]);
    const [error, setError] = useState(null);
    const [loading, setLoading] = useState(true);

    const fetchActieveVerzoeken = () => {
        axios.get('https://localhost:5033/api/Huurverzoek/GetAllActive')
            .then(response => {
                setActieveVerzoeken(response.data);
                setLoading(false);
            })
            .catch(err => {
                setError('Er is een fout opgetreden bij het ophalen van actieve huurverzoeken.');
                setLoading(false);
            });
    };

    const fetchAfgekeurdeVerzoeken = () => {
        axios.get('https://localhost:5033/api/Huurverzoek/GetAllAfgekeurde')
            .then(response => {
                setAfgekeurdeVerzoeken(response.data);
            })
            .catch(err => {
                setError('Er is een fout opgetreden bij het ophalen van afgekeurde huurverzoeken.');
            });
    };

    useEffect(() => {
        fetchActieveVerzoeken(); // Haal actieve verzoeken op
        fetchAfgekeurdeVerzoeken(); // Haal afgekeurde verzoeken op

        // Periodieke updates
        const interval = setInterval(() => {
            fetchActieveVerzoeken();
            fetchAfgekeurdeVerzoeken();
        }, 5000);

        return () => clearInterval(interval); // Ruim interval op bij unmount
    }, []);

    const approveRequest = (id) => {
        axios.put(`https://localhost:5033/api/Huurverzoek/keuring/${id}/true`)
            .then(() => {
                setActieveVerzoeken(prev => prev.filter(req => req.huurderID !== id));
                alert('Huurverzoek goedgekeurd!');
            })
            .catch(err => alert(`Fout bij goedkeuren: ${err.message}`));
    };

    const weigerRequest = (id) => {
        axios.put(`https://localhost:5033/api/Huurverzoek/keuring/${id}/false`)
            .then(() => {
                setActieveVerzoeken(prev => prev.filter(req => req.huurderID !== id));
                fetchAfgekeurdeVerzoeken(); // Werk afgekeurde verzoeken bij
                alert('Huurverzoek geweigerd!');
            })
            .catch(err => alert(`Fout bij weigeren: ${err.message}`));
    };

    if (loading) return <div>Loading...</div>;
    if (error) return <div>Error: {error}</div>;

    return (
        <>
            <div>
                <h1>Actieve Huurverzoeken</h1>
                {actieveVerzoeken.length === 0 ? (
                    <p>Geen actieve huurverzoeken.</p>
                ) : (
                    <ul>
                        {actieveVerzoeken.map((huurverzoek) => (
                            <li key={huurverzoek.huurderID}>
                                <p>Voertuig: {huurverzoek.voertuig.merk} {huurverzoek.voertuig.model}</p>
                                <p>Begin Datum: {new Date(huurverzoek.beginDate).toLocaleDateString()}</p>
                                <p>Eind Datum: {new Date(huurverzoek.endDate).toLocaleDateString()}</p>
                                <p>Voertuig Beschikbaar: {huurverzoek.voertuig?.voertuigBeschikbaar ? 'Ja' : 'Nee'}</p>
                                <button onClick={() => approveRequest(huurverzoek.huurderID)}>
                                    Keur verzoek goed
                                </button>
                                <button onClick={() => weigerRequest(huurverzoek.huurderID)}>
                                    Weiger verzoek
                                </button>
                            </li>
                        ))}
                    </ul>
                )}
            </div>

            <div>
                <h1>Afgekeurde Huurverzoeken</h1>
                {afgekeurdeVerzoeken.length === 0 ? (
                    <p>Geen afgekeurde huurverzoeken.</p>
                ) : (
                    <ul>
                        {afgekeurdeVerzoeken.map((huurverzoek) => (
                            <li key={huurverzoek.huurderID}>
                                <p>Voertuig: {huurverzoek.voertuig.merk} {huurverzoek.voertuig.model}</p>
                                <p>Begin Datum: {new Date(huurverzoek.beginDate).toLocaleDateString()}</p>
                                <p>Eind Datum: {new Date(huurverzoek.endDate).toLocaleDateString()}</p>
                                <p>Status: Afgekeurd</p>
                            </li>
                        ))}
                    </ul>
                )}
            </div>
        </>
    );
};

export default HuurVerzoekenList;
