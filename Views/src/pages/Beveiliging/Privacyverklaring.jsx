import React, { useState, useEffect } from 'react';
import axios from 'axios';
import DOMPurify from 'dompurify';
import '../../styles/privacyverklaring.css';

const PrivacyPolicy = () => {
    const [privacyVerklaring, setPrivacyVerklaring] = useState({
        updateDatum: '',
        verklaring: ''
    });
    // fetcht de wijzigingen van ons privacybeleid
    useEffect(() => {
        const fetchLatestPrivacyPolicy = async () => {
            try {
                const response = await axios.get('https://localhost:5033/api/PrivacyVerklaring/Laatste-variant-privacyVerklaring');
                if (response.data) {
                    const formattedVerklaring = response.data.verklaring
                        .split(/\n\n/)
                        .map(paragraph => `<p>${DOMPurify.sanitize(paragraph)}</p>`)
                        .join('');
                    setPrivacyVerklaring({
                        updateDatum: response.data.updateDatum,
                        verklaring: formattedVerklaring
                    });
                }
            } catch (error) {
                console.error("Error fetching the latest privacy policy:", error);
            }
        };

        fetchLatestPrivacyPolicy();
    }, []);

    return (
        <div className="privacy-policy-container">
            <h1>Privacyverklaring</h1>
            <p>Laatst bijgewerkt: {privacyVerklaring.updateDatum}</p>
            <div dangerouslySetInnerHTML={{ __html: privacyVerklaring.verklaring }}></div>
        </div>
    );
}

export default PrivacyPolicy;
