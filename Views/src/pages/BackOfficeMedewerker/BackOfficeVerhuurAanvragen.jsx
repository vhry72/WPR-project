import React, { useEffect, useState } from 'react';
import axios from 'axios';
import { Navigate } from 'react-router-dom';

const HuurVerzoekenList = () => {
    const [huurverzoeken, setHuurverzoeken] = useState([]);
    const [error, setError] = useState(null);
    const [loading, setLoading] = useState(true);



    useEffect(() => {
        axios.get('https://localhost:5033/api/Huurverzoek/GetAllActive')
            .then(response => {
                setHuurverzoeken(response.data);
                setLoading(false);
            })
            .catch(err => {
                setError(err.message);
                setLoading(false);
            });
    }, []);

    const approveRequest = (id) => {
        axios.put(`https://localhost:5033/api/Huurverzoek/approve/${id}/true`)
            .then(() => {
                setHuurverzoeken(prevState => prevState.filter(req => req.huurderID !== id));
                alert('Huurverzoek goedgekeurd!');
            })
            .catch(err => alert(`Fout bij goedkeuren: ${err.message}`));
    };
    const weigerRequest = (id) => {
        axios.put(`https://localhost:5033/api/Huurverzoek/weiger/${id}/false`)
            .then(() => {
                setHuurverzoeken(prevState => prevState.filter(req => req.huurderID !== id));
                alert('Huurverzoek goedgekeurd!');
            })
            .catch(err => alert(`Fout bij goedkeuren: ${err.message}`));
    };




    if (loading) return <div>Loading...</div>;
    if (error) return <div>Error: {error}</div>;
    return (
        <div>
            <h1>Huurverzoeken</h1>
            {error && <p>{error}</p>} {/* Toon een foutmelding als er een fout optreedt */}

            {huurverzoeken.length === 0 ? (
                <p>Geen Huurverzoeken aangevraagd.</p>
            ) : (
                <ul>
                        {huurverzoeken.map((huurverzoek) => (
                            <li key={huurverzoek.huurderID}> {/* Zorg ervoor dat de juiste ID wordt gebruikt */}
                                <p>Voertuig: {huurverzoek.voertuig.merk} {huurverzoek.voertuig.model}</p>
                                <p>Begin Datum: {new Date(huurverzoek.beginDate).toLocaleDateString()}</p>
                                <p>Eind Datum: {new Date(huurverzoek.endDate).toLocaleDateString()}</p>                              
                                <button onClick={() => approveRequest(huurverzoek.huurderID)}>
                                    Keur verzoek goed.
                                </button>
                                <button onClick={() => weigerRequest(huurverzoek.huurderID)}>
                                    Weiger verzoek.
                                </button>
                        </li>
                    ))}
                </ul>
            )}
        </div>
    );
};

export default HuurVerzoekenList;


