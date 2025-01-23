import React, { useEffect, useState } from 'react';
import axios from 'axios';

const HuurVerzoekenList = () => {
    const [huurverzoeken, setHuurverzoeken] = useState([]);
    const [error, setError] = useState(null);
    const [loading, setLoading] = useState(true);
    const [currentInname, setCurrentInname] = useState(null); // Huidig voertuig dat wordt ingenomen
    const [schade, setSchade] = useState(''); // Schade-invoer

    const fetchHuurverzoeken = () => {
        axios.get('https://localhost:5033/api/Huurverzoek/GetAllGoedGekeurde')
            .then(response => {
                setHuurverzoeken(response.data);
                setLoading(false);
            })
            .catch(err => {
                setError(err.message);
                setLoading(false);
            });
    };

    useEffect(() => {
        fetchHuurverzoeken();

        const interval = setInterval(() => {
            fetchHuurverzoeken();
        }, 5000);

        return () => clearInterval(interval);
    }, []);

    const neemIn = (id, voertuigId) => {
        if (!voertuigId) {
            alert("Voertuig ID is niet beschikbaar.");
            return;
        }
        setCurrentInname({ id, voertuigId });
    };

    const handleSchadeSubmit = () => {
        const { id, voertuigId } = currentInname;

        axios.put(`https://localhost:5033/api/Voertuig/veranderBeschikbaar/${voertuigId}/true`)
            .then(() => {
                if (schade) {
                    // Verstuur schadegegevens naar de backend
                    axios.post(`https://localhost:5033/api/SchadeRapport`, {
                        voertuigId,
                        schadeBeschrijving: schade,
                    })
                        .then(() => {
                            alert('Voertuig ingenomen en schade geregistreerd.');
                        })
                        .catch(err => alert(`Fout bij schade-registratie: ${err.message}`));
                } else {
                    alert('Voertuig ingenomen zonder schade.');
                }

                // Werk de lijst van huurverzoeken bij
                setHuurverzoeken(prevState => prevState.filter(req => req.huurderID !== id));
                setCurrentInname(null);
                setSchade('');
            })
            .catch(err => alert(`Fout bij inname: ${err.message}`));
    };

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

    const zetOpVerhuurd = (id, VoertuigStatusId) => {
        if (!VoertuigStatusId) {
            alert('VoertuigStatusId is niet beschikbaar');
            return;
        }
        axios.put(`https://localhost:5033/api/VoertuigStatus/Verhuur/${VoertuigStatusId}/false`)
            .then(() => {
                setHuurverzoeken(prevState =>
                    prevState.map(req =>
                        req.huurderID === id
                            ? {
                                ...req,
                                voertuig: {
                                    ...req.voertuig,
                                    voertuigStatusId: 'Niet Verhuurd',
                                }
                            }
                            : req
                    )
                );
                alert('Voertuig is nu niet verhuurd');
            })
            .catch(err => alert(`Fout bij verhuren: ${err.message}`));
    };

    if (loading) return <div>Loading...</div>;
    if (error) return <div>Error: {error}</div>;

    return (
        <div>
            <h1>Mogelijke Uitgave</h1>
            {currentInname && (
                <div>
                    <h2>Voertuig Inname</h2>
                    <textarea
                        value={schade}
                        onChange={(e) => setSchade(e.target.value)}
                        placeholder="Voer eventuele schade in (optioneel)"
                        rows="4"
                        cols="50"
                    />
                    <br />
                    <button onClick={handleSchadeSubmit}>
                        Bevestig Inname
                    </button>
                    <button onClick={() => setCurrentInname(null)}>
                        Annuleer
                    </button>
                </div>
            )}
            {huurverzoeken.length === 0 ? (
                <p>Geen Huurverzoeken aangevraagd.</p>
            ) : (
                <ul>
                    {huurverzoeken.map((huurverzoek) => {
                        const voertuig = huurverzoek.voertuig;
                        const voertuigId = voertuig?.voertuigId;
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
