import React, { useState } from 'react';

// Dummy data voor schademeldingen met status en opmerkingen
const initialSchadeMeldingen = [
    {
        id: 1,
        foto: 'https://via.placeholder.com/150',
        beschrijving: 'Deuk aan de zijkant van de auto, veroorzaakt door een botsing met een paaltje.',
        datum: '2024-12-01',
        status: 'Nieuw', // Initiale status
        opmerkingen: '', // Leeg veld voor opmerkingen
    },
    {
        id: 2,
        foto: 'https://via.placeholder.com/150',
        beschrijving: 'Gebroken achterlicht door een achteraanrijding.',
        datum: '2024-12-05',
        status: 'Nieuw',
        opmerkingen: '',
    },
    {
        id: 3,
        foto: 'https://via.placeholder.com/150',
        beschrijving: 'Kras over de motorkap na een mislukte poging om een bocht te nemen.',
        datum: '2024-12-10',
        status: 'Nieuw',
        opmerkingen: '',
    },
    {
        id: 4,
        foto: 'https://via.placeholder.com/150',
        beschrijving: 'Schade aan de bumper door een aanrijding met een ander voertuig op de parkeerplaats.',
        datum: '2024-12-12',
        status: 'Nieuw',
        opmerkingen: '',
    },
];

const SchademeldingenLijst = () => {
    const [schadeMeldingen, setSchadeMeldingen] = useState(initialSchadeMeldingen);

    // Functie om de status naar "In reparatie" te veranderen en opmerkingen toe te voegen
    const setInReparatie = (id, opmerkingen) => {
        setSchadeMeldingen(
            schadeMeldingen.map((melding) =>
                melding.id === id
                    ? { ...melding, status: 'In reparatie', opmerkingen: opmerkingen }
                    : melding
            )
        );
    };

    // Functie om de opmerkingen bij te werken
    const handleOpmerkingChange = (id, value) => {
        setSchadeMeldingen(
            schadeMeldingen.map((melding) =>
                melding.id === id ? { ...melding, opmerkingen: value } : melding
            )
        );
    };

    return (
        <div>
            <h1>Schademeldingen</h1>
            <ul>
                {schadeMeldingen.map((melding) => (
                    <li key={melding.id} style={{ marginBottom: '20px' }}>
                        <div style={{ display: 'flex', alignItems: 'center' }}>
                            {/* Foto van de schade */}
                            <img
                                src={melding.foto}
                                alt={`Schade ${melding.id}`}
                                style={{ width: '150px', height: '150px', marginRight: '20px' }}
                            />
                            {/* Beschrijving, datum en status */}
                            <div>
                                <p>
                                    <strong>Beschrijving:</strong> {melding.beschrijving}
                                </p>
                                <p>
                                    <strong>Datum:</strong> {melding.datum}
                                </p>
                                <p>
                                    <strong>Status:</strong> {melding.status}
                                </p>

                                {/* Invoerveld voor opmerkingen */}
                                {melding.status === 'Nieuw' && (
                                    <div>
                                        <label htmlFor={`opmerking-${melding.id}`} style={{ marginRight: '10px' }}>
                                            Opmerkingen:
                                        </label>
                                        <input
                                            id={`opmerking-${melding.id}`}
                                            type="text"
                                            value={melding.opmerkingen}
                                            onChange={(e) => handleOpmerkingChange(melding.id, e.target.value)}
                                            placeholder="Voer een opmerking in"
                                            style={{
                                                padding: '5px',
                                                width: '250px',
                                                marginBottom: '10px',
                                                display: 'block',
                                            }}
                                        />
                                    </div>
                                )}

                                {/* Knop om de status naar "In reparatie" te veranderen */}
                                {melding.status !== 'In reparatie' && melding.opmerkingen && (
                                    <button
                                        onClick={() => setInReparatie(melding.id, melding.opmerkingen)}
                                        style={{ marginTop: '10px', backgroundColor: 'blue', color: 'white' }}
                                    >
                                        Zet in reparatie
                                    </button>
                                )}
                            </div>
                        </div>
                    </li>
                ))}
            </ul>
        </div>
    );
};

export default SchademeldingenLijst;
