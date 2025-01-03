import React, { useEffect, useState } from 'react';
import "../styles/styles.css";

const AbonnementenList = () => {
    const [abonnementen, setAbonnementen] = useState([]);
    const [error, setError] = useState(null);

    useEffect(() => {
        fetch('https://localhost:5033/api/Abonnement/bijna-verlopen')
            .then((response) => {
                if (!response.ok) {
                    throw new Error('Netwerk reactie was niet ok');
                }
                return response.json();
            })
            .then((data) => {
                setAbonnementen(data); // Zet de opgehaalde data
            })
            .catch((error) => {
                setError('Er is een fout opgetreden bij het ophalen van de meldingen.');
            });
    }, []);

    return (
        <div>
            <h1>Meldingen</h1>
            {error && <p>{error}</p>}
            <ul>
                {abonnementen.map((abonnement) => (
                    <li key={abonnement.abonnementId}>
                        <p>Vervaldatum: {new Date(abonnement.vervaldatum).toLocaleDateString()}</p>
                    </li>
                ))}
            </ul>
        </div>
    );
};

export default AbonnementenList;
