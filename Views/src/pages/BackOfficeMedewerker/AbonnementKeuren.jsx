import React, { useEffect, useState } from 'react';
import axios from 'axios';
import { toast } from 'react-toastify';

const API_URL = "https://localhost:5033";

const AbonnementenKeuren = () => {
    const [abonnementen, setAbonnementen] = useState([]);
    const [huurderDetails, setHuurderDetails] = useState({});
    const [geselecteerdAbonnementId, setGeselecteerdAbonnementId] = useState('');
    const [loading, setLoading] = useState(true);

    useEffect(() => {
        const fetchAbonnementen = async () => {
            try {
                const response = await axios.get(`${API_URL}/api/Abonnement`);
                const nu = new Date();
                const tienDagenGeleden = new Date(nu.setDate(nu.getDate() - 10));

                const relevanteAbonnementen = response.data.filter(ab => {
                    const begindatum = new Date(ab.beginDatum);
                    return begindatum > tienDagenGeleden && ab.status === false;
                });

                setAbonnementen(relevanteAbonnementen);
                await Promise.all(relevanteAbonnementen.map(abonnement =>
                    fetchZakelijkeHuurder(abonnement.zakelijkeId)
                ));
            } catch (error) {
                console.error("Fout bij het ophalen van de abonnementen:", error);
            } finally {
                setLoading(false);
            }
        };

        const fetchZakelijkeHuurder = async (zakelijkeId) => {
            if (!huurderDetails[zakelijkeId]) {
                try {
                    const response = await axios.get(`${API_URL}/api/ZakelijkeHuurder/${zakelijkeId}`);
                    setHuurderDetails(prev => ({ ...prev, [zakelijkeId]: response.data }));
                } catch (error) {
                    console.error("Fout bij het ophalen van zakelijke huurder gegevens:", error);
                }
            }
        };

        fetchAbonnementen();
    }, []);

    const handleSelectChange = (event) => {
        setGeselecteerdAbonnementId(event.target.value);
    };

    const handleKeuring = async (abonnementId, keuring) => {
        try {
            await axios.post(`${API_URL}/api/BackOfficeMedewerker/${abonnementId}/Keuring/${keuring}`);
            toast.success("Keuring is succesvol uitgevoerd");
            if (keuring) { 
                setAbonnementen(prev => prev.filter(ab => ab.abonnementId !== abonnementId));
                setGeselecteerdAbonnementId('');
            }
        } catch (error) {
            console.error("Fout bij het keuren van een abonnement:", error);
        }
    };

    const geselecteerdAbonnement = abonnementen.find(ab => ab.abonnementId === geselecteerdAbonnementId);
    const zakelijkeHuurder = huurderDetails[geselecteerdAbonnement?.zakelijkeId];

    if (loading) return <p>Loading abonnementen...</p>;

    return (
        <div>
            <h1>Abonnementen Beheer</h1>
            <select value={geselecteerdAbonnementId} onChange={handleSelectChange}>
                <option value="">Selecteer een bedrijf</option>
                {abonnementen.map((abonnement) => (
                    <option key={abonnement.abonnementId} value={abonnement.abonnementId}>
                        {huurderDetails[abonnement.zakelijkeId]?.bedrijfsNaam || 'Onbekend'} ({abonnement.naam})
                    </option>
                ))}
            </select>
            {geselecteerdAbonnement && zakelijkeHuurder && (
                <div>
                    <h3>{geselecteerdAbonnement.naam}</h3>
                    <p>Bedrijfsnaam: {zakelijkeHuurder.bedrijfsNaam}</p>
                    <p>Adres: {zakelijkeHuurder.adres}</p>
                    <p>Email: {zakelijkeHuurder.bedrijfsEmail}</p>
                    <p>Telefoonnummer: {zakelijkeHuurder.telNummer}</p>
                    <p>Is Actief: {zakelijkeHuurder.isActive ? 'Ja' : 'Nee'}</p>
                    <button onClick={() => handleKeuring(geselecteerdAbonnement.abonnementId, true)}>Goedkeuren</button>
                    <button onClick={() => handleKeuring(geselecteerdAbonnement.abonnementId, false)}>Afkeuren</button>
                </div>
            )}
        </div>
    );
};

export default AbonnementenKeuren;
