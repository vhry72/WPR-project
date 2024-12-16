import React, { useEffect, useState } from 'react';

const SchademeldingenList = () => {
    const [schademeldingen, setSchademeldingen] = useState([]);
    const [error, setError] = useState(null);
    const [editedOpmerkingen, setEditedOpmerkingen] = useState({});

    // Dummy lijst van schademeldingen (voor als er een fout optreedt)
    const dummySchademeldingen = [
        {
            schademeldingId: '1',
            beschrijving: 'Kras op de deur',
            status: 'Open',
            opmerkingen: 'Moet nog worden gerepareerd',
            voertuig: { naam: 'Auto 1' },
            datum: new Date().toISOString(),
            fotoUrl: 'https://example.com/image1.jpg', // Dummy afbeelding
        },
        {
            schademeldingId: '2',
            beschrijving: 'Schade aan voorruit',
            status: 'Afgehandeld',
            opmerkingen: 'Verzekering heeft betaald',
            voertuig: { naam: 'Auto 2' },
            datum: new Date().toISOString(),
            fotoUrl: 'https://example.com/image2.jpg', // Dummy afbeelding
        },
        {
            schademeldingId: '3',
            beschrijving: 'Deuk in de bumper',
            status: 'In behandeling',
            opmerkingen: 'Wachten op onderdelen',
            voertuig: { naam: 'Auto 3' },
            datum: new Date().toISOString(),
            fotoUrl: 'https://example.com/image3.jpg', // Dummy afbeelding
        },
    ];

    useEffect(() => {
        fetch('https://localhost:5033/api/schademeldingen')
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
                setSchademeldingen(dummySchademeldingen); // Zet de dummy lijst bij fout
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
                        <h3>{schademelding.beschrijving}</h3>
                        <p>Status: {schademelding.status}</p>
                        <p>Voertuig: {schademelding.voertuig ? schademelding.voertuig.naam : 'Onbekend'}</p>
                        <p>Datum: {new Date(schademelding.datum).toLocaleDateString()}</p>

                        {/* Toon de afbeelding van de schademelding */}
                        {schademelding.fotoUrl && <img src={schademelding.fotoUrl} alt="Schade" width="200" />}

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

