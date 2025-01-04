import React, { useEffect, useState } from 'react';

const SchademeldingenList = () => {
    const [schademeldingen, setSchademeldingen] = useState([]);
    const [error, setError] = useState(null);
    const [editedOpmerkingen, setEditedOpmerkingen] = useState({});

    useEffect(() => {
        fetch('https://localhost:5033/api/Schademelding')
            .then((response) => {
                if (!response.ok) {
                    throw new Error('Netwerk reactie was niet ok');
                }
                return response.json();
            })
            .then((data) => {
                setSchademeldingen(data); // Zet de opgehaalde data
            })
            .catch((error) => {
                setError('Er is een fout opgetreden bij het ophalen van de schademeldingen.');
            });
    }, []);

    // Functie om de status van een schademelding te veranderen naar 'In reparatie'
    const updateStatus = (id) => {
        setSchademeldingen((prevSchademeldingen) =>
            prevSchademeldingen.map((melding) =>
                melding.schademeldingId === id
                    ? { ...melding, status: 'In reparatie' }
                    : melding
            )
        );
    };

    // Functie om opmerkingen aan te passen
    const handleOpmerkingenChange = (id, newOpmerkingen) => {
        setEditedOpmerkingen((prev) => ({
            ...prev,
            [id]: newOpmerkingen,
        }));
    };

    // Functie om de gewijzigde opmerkingen op te slaan
    const saveOpmerkingen = (id) => {
        setSchademeldingen((prevSchademeldingen) =>
            prevSchademeldingen.map((melding) =>
                melding.schademeldingId === id
                    ? { ...melding, opmerkingen: editedOpmerkingen[id] || melding.opmerkingen }
                    : melding
            )
        );
    };

    return (
        <div>
            <h1>Schademeldingen</h1>
            {error && <p>{error}</p>} {/* Toon een foutmelding als er een fout optreedt */}
            <ul>
                {schademeldingen.map((schademelding) => (
                    <li key={schademelding.schademeldingId}>
                        <p>{schademelding.beschrijving}</p>
                        <p>Status: {schademelding.status}</p>
                        <p>Voertuig: {schademelding.voertuig.merk} {schademelding.voertuig.model}</p>
                        <p>Datum: {new Date(schademelding.datum).toLocaleDateString()}</p>


                        {/* Bewerken van opmerkingen */}
                        <div>
                            <input
                                type="text"
                                value={editedOpmerkingen[schademelding.schademeldingId] || schademelding.opmerkingen}
                                onChange={(e) => handleOpmerkingenChange(schademelding.schademeldingId, e.target.value)}
                            />
                            <button onClick={() => saveOpmerkingen(schademelding.schademeldingId)}>
                                Opslaan opmerkingen
                            </button>
                        </div>

                        {/* Knop om de status te wijzigen naar "In reparatie" */}
                        <button onClick={() => updateStatus(schademelding.schademeldingId)}>
                            Zet op "In reparatie"
                        </button>
                    </li>
                ))}
            </ul>
        </div>
    );
};

export default SchademeldingenList;

