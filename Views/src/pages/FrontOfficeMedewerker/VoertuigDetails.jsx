import React, { useEffect, useState } from 'react';


const VoertuigenList = () => {
    const [voertuigen, setVoertuigen] = useState([]);
    const [error, setError] = useState(null);


    useEffect(() => {
        fetch('https://localhost:5033/api/Voertuigen')
            .then((response) => {
                if (!response.ok) {
                    throw new Error('Netwerk reactie was niet ok');
                }
                return response.json();
            })
            .then((data) => {
                setVoertuigen(data); // Zet de opgehaalde data
            })
            .catch((error) => {
                setError('Er is een fout opgetreden bij het ophalen van de Voertuigen.');
            });
    }, []);
   
    return (
        <div>
            <h1>Voertuigen</h1>
            {error && <p>{error}</p>} {/* Toon een foutmelding als er een fout optreedt */}
            {voertuigen.length === 0 ? (
                <p>Geen voertuigen beschikbaar.</p>
            ) : (
                <ul>
                    {voertuigen.map((voertuig) => (
                        <li key={voertuig.voertuigId}> {/* Zorg ervoor dat de juiste ID wordt gebruikt */}
                            <p>Voertuig: {voertuig.merk} {voertuig.model}</p>                            
                        </li>
                    ))}
                </ul>
            )}
        </div>
    );
};

export default VoertuigenList;
