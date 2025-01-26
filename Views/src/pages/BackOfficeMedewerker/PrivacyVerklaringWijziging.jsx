import React, { useState, useEffect } from 'react';
import axios from 'axios';
import JwtService from "../../services/JwtService";
import "../../styles/PrivacyVerklaringWijziging.css"; 
import { toast } from 'react-toastify';
import { diffWordsWithSpace } from 'diff';

const PrivacyVerklaringWijziging = () => {
    const [huurderId, setHuurderId] = useState(null);
    const [privacyVerklaringen, setPrivacyVerklaringen] = useState([]);
    const [huidigePrivacyVerklaring, setHuidigePrivacyVerklaring] = useState('');
    const [nieuwePrivacyVerklaring, setNieuwePrivacyVerklaring] = useState('');


    const fetchLatestPrivacyVerklaring = async () => {
        try {
            const response = await axios.get('https://localhost:5033/api/PrivacyVerklaring/Laatste-variant-privacyVerklaring');
            if (response.data && response.data.verklaring) {
                setHuidigePrivacyVerklaring(response.data.verklaring);
                setNieuwePrivacyVerklaring(response.data.verklaring);
            } else {
                console.error("Geen verklaring gevonden in de API response.");
            }
            fetchPrivacyVerklaringen(); // Laad alle privacyverklaringen na het instellen van de meest recente
        } catch (error) {
            console.error("Fout bij het ophalen van de laatste privacyverklaring:", error);
        }
    };

    useEffect(() => {
        const fetchUserId = async () => {
            try {
                const userId = await JwtService.getUserId();
                if (userId) {
                    setHuurderId(userId);
                } else {
                    console.error("Huurder ID kon niet worden opgehaald via de API.");
                }
            } catch (error) {
                console.error("Fout bij het ophalen van de huurder ID:", error);
            }
        };

        fetchUserId();
        fetchLatestPrivacyVerklaring();
    }, []);

    const fetchPrivacyVerklaringen = async () => {
        try {
            const response = await axios.get(`https://localhost:5033/api/PrivacyVerklaring/AllePrivacyVerklaringen`, { withCredentials: true });
            setPrivacyVerklaringen(response.data);
        } catch (error) {
            console.error("Fout bij het ophalen van privacyverklaringen:", error);
        }
    };

    const handleVerstuurNieuwe = async () => {
        try {
            const genormaliseerdeVerklaring = nieuwePrivacyVerklaring.replace(/\n/g, '\\n');

            const config = {
                headers: {
                    'Content-Type': 'application/json'
                }
            };

            const nieuweVerklaring = {
                medewerkerId: huurderId,
                verklaring: genormaliseerdeVerklaring
            };

            await axios.post('https://localhost:5033/api/PrivacyVerklaring/Voeg-privacyVerklaring-toe', nieuweVerklaring, config, { withCredentials: true });
            toast.success("Succesvol de nieuwe privacyverklaring geupload!");
            fetchPrivacyVerklaringen();
            fetchLatestPrivacyVerklaring()

        } catch (error) {
            console.error("Fout bij het toevoegen van de nieuwe privacyverklaring:", error);
        }
    };

    const formatDate = (dateString) => {
        const options = {
            year: 'numeric',
            month: '2-digit',
            day: '2-digit',
            hour: '2-digit',
            minute: '2-digit',
            second: '2-digit',
            hour12: false // Gebruikt 24-uurs tijd in plaats van 12-uurs tijd.
        };
        return new Date(dateString).toLocaleString('nl-NL', options);
    };

    const getHighlightedText = (currentText, newText) => {
        const differences = diffWordsWithSpace(currentText, newText);

        return (
            <div style={{ whiteSpace: 'pre-wrap' }}>
                {differences.map((part, index) => {
                    const style = part.added
                        ? { backgroundColor: 'yellow' } // Toegevoegd
                        : part.removed
                            ? { backgroundColor: 'red', textDecoration: 'line-through' } // Verwijderd
                            : {}; // Ongewijzigd

                    return (
                        <span key={index} style={style}>
                            {part.value}
                        </span>
                    );
                })}
            </div>
        );
    };





    return (
        <div className="privacyverklaring-container">
            <h1>Privacyverklaring Wijzigen</h1>
            <div className="verklaringen-wrapper">
                <div className="huidige-verklaring">
                    <h2>Huidige Privacy Verklaring</h2>
                    <textarea
                        value={huidigePrivacyVerklaring}
                        rows="20"
                        cols="50"
                        readOnly
                    />
                </div>
                <div className="nieuwe-verklaring">
                    <h2>Nieuwe Privacy Verklaring</h2>
                    <textarea
                        placeholder="Nieuwe Privacyverklaring Tekst"
                        value={nieuwePrivacyVerklaring}
                        onChange={(e) => setNieuwePrivacyVerklaring(e.target.value)}
                        rows="20"
                        cols="50"
                    />
                </div>
            </div>
            <select onChange={(e) => setHuidigePrivacyVerklaring(e.target.value)}>
                {privacyVerklaringen.map(pv => (
                    <option key={pv.verklaringId} value={pv.verklaring}>
                        {formatDate(pv.updateDatum)}
                    </option>
                ))}
            </select>
            <button onClick={handleVerstuurNieuwe} className="verstuur-knop">Nieuwe Privacyverklaring Toevoegen</button>
            <div>
                {getHighlightedText(huidigePrivacyVerklaring, nieuwePrivacyVerklaring)}
            </div>
        </div>
    );

}

export default PrivacyVerklaringWijziging;

