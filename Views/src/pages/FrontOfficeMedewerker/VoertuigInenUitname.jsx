import React, { useEffect, useState } from 'react';
import axios from 'axios';
import { v4 as uuidv4 } from "uuid";
import { toast } from 'react-toastify';

const HuurVerzoekenList = () => {
    const [huurverzoeken, setHuurverzoeken] = useState([]);
    const [error, setError] = useState(null);
    const [loading, setLoading] = useState(true);
    const [currentInname, setCurrentInname] = useState(null); // Huidig voertuig dat wordt ingenomen
    const [schade, setSchade] = useState(''); // Schade-invoer

    const fetchHuurverzoeken = () => {
        axios.get('https://localhost:5033/api/Huurverzoek/GetAllGoedGekeurde', { withCredentials: true })
            .then(response => {
                const filteredHuurverzoeken = response.data.filter(huurverzoek => !huurverzoek.isCompleted);
                setHuurverzoeken(filteredHuurverzoeken);
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
        const keuring = true;

        axios.put(`https://localhost:5033/api/Voertuig/veranderBeschikbaar/${voertuigId}/true`)
            .then(() => {
                if (schade) {
                    const payload = {
                        schademeldingId: uuidv4(),
                        beschrijving: schade,
                        datum: new Date().toISOString(),
                        status: "Nieuw",
                        opmerkingen: "geen",
                        voertuigId: voertuigId,
                    };

                    axios.post(`https://localhost:5033/api/Schademelding/maak`, payload)
                        .then(() => {
                            toast.success('Voertuig ingenomen en schade geregistreerd.');
                        })
                        .catch(err => {
                            toast.error(`Fout bij schade-registratie: ${err.message}`);
                        });
                } else {
                    toast.success('Voertuig ingenomen zonder schade.');
                }

                
                axios.post(`https://localhost:5033/api/FrontOfficeMedewerker/${id}/${keuring}/Huurverzoek-isCompleted`)


                setHuurverzoeken(prevState => prevState.filter(req => req.huurVerzoekId !== id));
                setCurrentInname(null);
                setSchade('');
            })
            .catch(err => {
                toast.error(`Fout bij inname: ${err.message}`);
            });
    };

    const geefUit = (id, voertuigId) => {
        if (!voertuigId) {
            alert("Voertuig ID is niet beschikbaar.");
            return;
        }
        axios.put(`https://localhost:5033/api/Voertuig/veranderBeschikbaar/${voertuigId}/false`)
            .then(() => {
                setHuurverzoeken(prevState => prevState.filter(req => req.huurVerzoekId !== id));
                alert('Voertuig Uitgegeven');
            })
            .catch(err => alert(`Fout bij uitgeven: ${err.message}`));
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
                                <li key={`${huurverzoek.huurVerzoekId}`}>
                                    <p>Voertuig: {voertuig?.merk} {voertuig?.model}</p>
                                    <p>Kleur: {voertuig?.kleur}</p>
                                    <p>Bouwjaar: {voertuig?.bouwjaar}</p>
                                    <p>Begin Datum: {new Date(huurverzoek.beginDate).toLocaleDateString()}</p>
                                    <p>Eind Datum: {new Date(huurverzoek.endDate).toLocaleDateString()}</p>
                                    <p>Voertuig Beschikbaar: {voertuig?.voertuigBeschikbaar ? 'Ja' : 'Nee'}</p>

                                    <button onClick={() => geefUit(huurverzoek.huurVerzoekId, voertuigId)}>
                                        Geef Uit
                                    </button>
                                    <button onClick={() => neemIn(huurverzoek.huurVerzoekId, voertuigId)}>
                                        Neem In
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
