import React, { useEffect, useState } from 'react';


const HuurVerzoekenList = () => {
    const [huurverzoeken, setHuurverzoeken] = useState([]);
    const [error, setError] = useState(null);
    const [editedOpmerkingen, setEditedOpmerkingen] = useState({});
   

    useEffect(() => {
        fetch('https://localhost:5033/api/Huurverzoek/GetAllAfgekeurde')
            .then((response) => {
                if (!response.ok) {
                    throw new Error('Netwerk reactie was niet ok');
                }
                return response.json();
            })
            .then((data) => {
                setHuurverzoeken(data); // Zet de opgehaalde data
            })
            .catch((error) => {
                setError('Er is een fout opgetreden bij het ophalen van de huurverzoeken.');
            });
    }, []);

    // update of het verzoek is goedgekeurd
    const updateGoedkeuring = (id) => {
        setHuurverzoeken((prevHuurverzoeken) =>
            prevHuurverzoeken.map((verzoek) =>
                verzoek.HuurverzoekId === id
                    ? { ...verzoek, approved: 1 }
                    : verzoek
            )
        );
    };

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