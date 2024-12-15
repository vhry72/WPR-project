import React, { useState, useEffect } from 'react';

const HuurVerzoekList = () => {
    // Gebruik van useState om de huurverzoeken op te slaan
    const [huurVerzoeken, setHuurVerzoeken] = useState([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null);

    // Haal de huurverzoeken op wanneer de component laadt
    useEffect(() => {
        const fetchHuurVerzoeken = async () => {
            try {
                const response = await fetch('/api/HuurVerzoek');
                if (!response.ok) {
                    throw new Error('Netwerkantwoord was niet ok');
                }

                const data = await response.json(); // Direct de response omzetten naar JSON
                setHuurVerzoeken(data);
            } catch (err) {
                setError(`Fout bij het ophalen van de huurverzoeken: ${err.message}`);
            } finally {
                setLoading(false);
            }
        };

        fetchHuurVerzoeken();
    }, []); // Lege dependency array betekent dat dit alleen wordt uitgevoerd bij de eerste render

    // Toon een laadtijd-indicator
    if (loading) {
        return <div>Loading...</div>;
    }

    // Toon een foutmelding als er een fout is
    if (error) {
        return <div>Error: {error}</div>;
    }

    return (
        <div>
            <h1>Huurverzoeken</h1>
            <ul>
                {huurVerzoeken.map((verzoek) => (
                    <li key={verzoek.HuurderID}>
                        <p><strong>Huurder ID:</strong> {verzoek.HuurderID}</p>
                        <p><strong>Voertuig ID:</strong> {verzoek.voertuigId}</p>
                        <p><strong>Begindatum:</strong> {new Date(verzoek.beginDate).toLocaleDateString()}</p>
                        <p><strong>Einddatum:</strong> {new Date(verzoek.endDate).toLocaleDateString()}</p>
                        <p><strong>Goedkeuring:</strong> {verzoek.approved ? 'Goedgekeurd' : 'Afgekeurd'}</p>
                    </li>
                ))}
            </ul>
        </div>
    );
};

export default HuurVerzoekList;
