import React, { useState, useEffect } from 'react';
import axios from 'axios';
import '../../styles/WagenparkBeheerderForm.css';
import JwtService from '../../services/JwtService';
import { v4 as uuidv4 } from 'uuid';

const API_URL = "https://localhost:5033";

const WagenparkBeheerderForm = () => {
    const [zakelijkeHuurderId, setZakelijkeHuurderId] = useState('');
    const [beheerderNaam, setBeheerderNaam] = useState('');
    const [bedrijfsEmail, setBedrijfsEmail] = useState('');
    const [wachtwoord, setWachtwoord] = useState('');
    const [adres, setAdres] = useState('');
    const [KVKNummer, setKVKNummer] = useState('');
    const [telefoonNummer, setTelefoonNummer] = useState('');
    const [zakelijkeId, setZakelijkeId] = useState('');
    const [AbonnementId, setAbonnementId] = useState('');
    const [updateDatumAbonnement, setUpdateDatumAbonnement] = useState('');
    const [AbonnementType, setAbonnementType] = useState('');
    const [errors, setErrors] = useState({});
    const [isLoading, setIsLoading] = useState(true);

    useEffect(() => {
        const fetchInitialData = async () => {
            try {
                const userId = await JwtService.getUserId();
                if (userId) {
                    setZakelijkeHuurderId(userId);
                    await Promise.all([
                        fetchZakelijkeHuurderData(userId),
                        fetchAbonnementData(userId)
                    ]);
                }
            } catch (error) {
                console.error("Fout bij het ophalen van de huurder ID:", error);
            }
            setIsLoading(false);
        };

        fetchInitialData();
    }, []);

    const fetchZakelijkeHuurderData = async (id) => {
        try {
            const response = await axios.get(`${API_URL}/api/zakelijkehuurder/${id}`);
            if (response.data) {
                setAdres(response.data.adres);
                setKVKNummer(response.data.kvkNummer);
                setTelefoonNummer(response.data.telNummer);
                setZakelijkeId(response.data.zakelijkeId);
                setBedrijfsEmail(response.data.bedrijfsEmail);
            }
        } catch (error) {
            console.error(error.response);
        }
    };

    const fetchAbonnementData = async (id) => {
        try {
            const response = await axios.get(`${API_URL}/api/ZakelijkeHuurder/${id}/AbonnementId`);
            console.log(response);
            if (response.data) {
                setAbonnementId(response.data.abonnementId);
                setAbonnementType(response.data.abonnementType);
            }
        } catch (error) {
            console.error(error.response);
        }
    };

    const handleInputChange = (event) => {
        const { name, value } = event.target;
        switch (name) {
            case 'beheerderNaam': setBeheerderNaam(value); break;
            case 'bedrijfsEmail': setBedrijfsEmail(value); break;
            case 'wachtwoord': setWachtwoord(value); break;
            case 'zakelijkeHuurderId': setZakelijkeHuurderId(value); break;
            default: break;
        }
    };

    

    const handleSubmit = async (event) => {
        event.preventDefault();
        try {
            const payload = {
                beheerderId: uuidv4(),
                beheerderNaam: beheerderNaam,
                bedrijfsEmail: bedrijfsEmail,
                adres: adres,
                kvkNummer: String(KVKNummer), 
                abonnementId: AbonnementId,
                abonnementType: AbonnementType,
                telefoonNummer: telefoonNummer,
                wachtwoord: wachtwoord,
                emailBevestigingToken: uuidv4(),
                isEmailBevestigd: false,
                aspNetUserId: "string",
                zakelijkeId: zakelijkeId
            };
            console.log(payload);
            await axios.post(`${API_URL}/api/Account/register-wagenparkbeheerder`, payload, {
                headers: {
                    'Content-Type': 'application/json'
                }
            });

            alert('Wagenparkbeheerder succesvol geregistreerd!');
        } catch (error) {
            console.error(error.response);
        }
    };

    if (isLoading) {
        return <div>Loading...</div>;
    }

    return (
        <div className="wagenparkRegister-form-container">
            <form className="wagenparkRegister-form" onSubmit={handleSubmit}>
                <h2 className="wagenparkRegister-header">Registreer Wagenparkbeheerder</h2>
                <div className="wagenparkRegister-details">
                    {zakelijkeId && (
                        <>
                            <p><strong>Adres:</strong> {adres}</p>
                            <p><strong>KVK-nummer:</strong> {KVKNummer}</p>
                            <p><strong>Telefoonnummer:</strong> {telefoonNummer}</p>
                        </>
                    )}
                </div>
                <div className="wagenparkRegister-form-group">
                    <label>Beheerdersnaam:</label>
                    <input
                        type="text"
                        name="beheerderNaam"
                        value={beheerderNaam}
                        onChange={handleInputChange}
                        placeholder="Voer beheerdersnaam in"
                    />
                </div>
                <div className="wagenparkRegister-form-group">
                    <label>Bedrijfs Email:</label>
                    <input
                        type="email"
                        name="bedrijfsEmail"
                        value={bedrijfsEmail}
                        onChange={handleInputChange}
                    />
                </div>
                <div className="wagenparkRegister-form-group">
                    <label>Wachtwoord:</label>
                    <input
                        type="password"
                        name="wachtwoord"
                        value={wachtwoord}
                        onChange={handleInputChange}
                        placeholder="Voer wachtwoord in"
                    />
                </div>
                <div className="wagenparkRegister-form-group">
                    <button className="wagenparkRegister-submit-button" type="submit">Registreer</button>
                </div>
                {Object.values(errors).map((error, index) => (
                    <p key={index} className="wagenparkRegister-error">{error}</p>
                ))}
            </form>
        </div>
    );
};

export default WagenparkBeheerderForm;
