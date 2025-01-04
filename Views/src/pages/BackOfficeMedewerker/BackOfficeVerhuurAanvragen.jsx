import React, { useEffect, useState } from 'react';

const HuurVerzoekenList = () => {
    const [huurverzoeken, setHuurverzoeken] = useState([]);
    const [error, setError] = useState(null);

    useEffect(() => {
        fetch('https://localhost:5033/api/Huurverzoek/GetAllActive')
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

    // update de goedkeuring van het verzoek
    const updateGoedkeuring = (id) => {
        // Stel de DTO voor de goedkeuring samen
        const dto = {
             // Zorg ervoor dat dit het juiste veld is dat overeenkomt met de route parameter in je API
            approved: true // Zet approved op true, want je wilt het goedkeuren
        };

        // Stuur een PUT verzoek naar de API
        fetch(`https://localhost:5033/api/Huurverzoek/KeurGoed/${id}`, {
            method: 'PUT',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(dto),
        })
            .then((response) => {
                if (!response.ok) {
                    throw new Error('Er is een fout opgetreden bij het bijwerken van de goedkeuring.');
                }
                return response.json();
            })
            .then(() => {
                // Werk de state bij om de goedkeuring te reflecteren zonder opnieuw te hoeven laden
                setHuurverzoeken((prevHuurverzoeken) =>
                    prevHuurverzoeken.map((verzoek) =>
                        verzoek.huurderID === id
                            ? { ...verzoek, approved: true } // Update de goedkeuring in de UI
                            : verzoek
                    )
                );
            })
            .catch((error) => {
                setError('Er is een fout opgetreden bij het bijwerken van de goedkeuring.');
                console.error(error);
            });
        console.log(huurverzoeken); // Voeg dit toe om te zien of de HuurverzoekId's uniek zijn

    };

    return (
        <div>
            <h1>Huurverzoeken</h1>
            {error && <p>{error}</p>} {/* Toon een foutmelding als er een fout optreedt */}
            <ul>
                {huurverzoeken.map((huurverzoek) => (
                    <li key={huurverzoek.huuderID}> {/* Zorg ervoor dat HuurverzoekId uniek is */}
                        <p>Voertuig: {huurverzoek.voertuig ? huurverzoek.voertuig.naam : 'Onbekend'}</p>
                        <p>Begin Datum: {new Date(huurverzoek.beginDate).toLocaleDateString()}</p>
                        <p>Eind Datum: {new Date(huurverzoek.eindDate).toLocaleDateString()}</p>
                        <p>Goedkeuring: {huurverzoek.approved ? 'Goedgekeurd' : 'Afgewezen'}</p>

                        <button onClick={() => updateGoedkeuring(huurverzoek.huuderID)}>
                            Zet op "Goedgekeurd"
                        </button>
                    </li>
                ))}
            </ul>

        </div>
    );
};

export default HuurVerzoekenList;


