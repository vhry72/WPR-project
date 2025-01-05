import React, { useEffect, useState } from 'react';
import axios from 'axios';

const HuurVerzoekenList = () => {
    const [huurverzoeken, setHuurverzoeken] = useState([]);
    const [error, setError] = useState(null);
    const [loading, setLoading] = useState(true);

    const fetchHuurverzoeken = () => {
        axios.get('https://localhost:5033/api/Huurverzoek/GetAllGoedGekeurde')
            .then(response => {
                setHuurverzoeken(response.data); // Sla de volledige response op
                setLoading(false);
            })
            .catch(err => {
                setError(err.message);
                setLoading(false);
            });
    };

    useEffect(() => {
        fetchHuurverzoeken(); // Initieel ophalen van gegevens

        // Periodiek ophalen van gegevens (polling)
        const interval = setInterval(() => {
            fetchHuurverzoeken();
        }, 5000); // Elke 5 seconden

        return () => clearInterval(interval); // Opruimen bij unmounten
    }, []);

    const geefUit = (id, voertuigId) => {
        if (!voertuigId) {
            alert("Voertuig ID is niet beschikbaar.");
            return;
        }
        axios.put(`https://localhost:5033/api/Voertuig/veranderBeschikbaar/${voertuigId}/false`)
            .then(() => {
                setHuurverzoeken(prevState => prevState.filter(req => req.huurderID !== id));
                alert('Voertuig Uitgegeven');
            })
            .catch(err => alert(`Fout bij uitgeven: ${err.message}`));
    };

    const neemIn = (id, voertuigId) => {
        if (!voertuigId) {
            alert("Voertuig ID is niet beschikbaar.");
            return;
        }
        axios.put(`https://localhost:5033/api/Voertuig/veranderBeschikbaar/${voertuigId}/true`)
            .then(() => {
                setHuurverzoeken(prevState => prevState.filter(req => req.huurderID !== id));
                alert('Voertuig ingenomen');
            })
            .catch(err => alert(`Fout bij inname: ${err.message}`));
    };
    const zetOpVerhuurd = (id, VoertuigStatusId) => {
        if (!VoertuigStatusId) {
            alert('VoertuigStatusId is niet beschikbaar');
            return;
        }
        axios.put(`https://localhost:5033/api/VoertuigStatus/Verhuur/${VoertuigStatusId}/false`)
            .then(() => {
                // Werk het voertuig in de state bij zonder de lijst opnieuw te filteren
                setHuurverzoeken(prevState =>
                    prevState.map(req =>
                        req.huurderID === id
                            ? {
                                ...req,
                                voertuig: {
                                    ...req.voertuig,
                                    voertuigStatusId: 'Niet Verhuurd', // Status direct bijwerken
                                }
                            }
                            : req
                    )
                );
                alert('Voertuig is nu niet verhuurd');
            })
            .catch(err => alert(`Fout bij verhuren: ${err.message}`));
    }



    if (loading) return <div>Loading...</div>;
    if (error) return <div>Error: {error}</div>;

    return (
        <div>
            <h1>Mogelijke Uitgave</h1>
            {huurverzoeken.length === 0 ? (
                <p>Geen Huurverzoeken aangevraagd.</p>
            ) : (
                <ul>
                    {huurverzoeken.map((huurverzoek) => {
                        const voertuig = huurverzoek.voertuig; // Verkrijg voertuigobject
                        const voertuigId = voertuig?.voertuigId; // Verkrijg voertuigId
                        const VoertuigStatusId = voertuig?.voertuigStatusId;
                        return (
                            <li key={huurverzoek.huurderID}>
                                <p>Voertuig: {voertuig?.merk} {voertuig?.model}</p>
                                <p>Kleur: {voertuig?.kleur}</p>
                                <p>Bouwjaar: {voertuig?.bouwjaar}</p>
                                <p>Begin Datum: {new Date(huurverzoek.beginDate).toLocaleDateString()}</p>
                                <p>Eind Datum: {new Date(huurverzoek.endDate).toLocaleDateString()}</p>
                                <p>Voertuig Beschikbaar: {voertuig?.voertuigBeschikbaar ? 'Ja' : 'Nee'}</p>

                                <button onClick={() => geefUit(huurverzoek.huurderID, voertuigId)}>
                                    Geef Uit
                                </button>
                                <button onClick={() => neemIn(huurverzoek.huurderID, voertuigId)}>
                                    Neem In
                                </button>
                                <button onClick={() => zetOpVerhuurd(huurverzoek.huurderID, VoertuigStatusId)}>
                                    Niet Verhuurd
                                </button>

                            </li>
                        );
                    })}
                </ul>
            )}
        </div>
    );
};

export default HuurVerzoekenList;