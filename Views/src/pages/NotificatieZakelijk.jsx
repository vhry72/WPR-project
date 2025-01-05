import { Link, useLocation } from "react-router-dom";
import React, { useState, useEffect } from "react";
import axios from 'axios';




// ZakelijkHuurderDashBoard Component
export const ZakelijkHuurderMeldingen = () => {
    const [huurverzoeken, setHuurverzoeken] = useState([]);
    const location = useLocation();
    const HuurderId = new URLSearchParams(location.search).get("HuurderID");
    const [error, setError] = useState(null);

    useEffect(() => {
        // Check if there are any answered requests or notifications
        axios.get(`https://localhost:5033/api/Huurverzoek/check-Beantwoorde/${HuurderId}`)
            .then(response => {
               
                setMeldingen(response.data);
                
            })
            .catch(err => {
                setError(err.message);
                console.error("Error fetching data:", err);
            });
    }, [HuurderId]); // Run the effect when HuurderId changes

    return (
        <div>
            <h1>Meldingen</h1>
            {error && <p>{error}</p>} {/* Toon een foutmelding als er een fout optreedt */}

            {huurverzoeken.length === 0 ? (
                <p>Geen meldingen op het moment.</p>
            ) : (
                <ul>
                    {meldingen.map((me) => (
                        <li key={huurverzoek.huurderID}> {/* Zorg ervoor dat de juiste ID wordt gebruikt */}
                            <p> Uw verzoek is goedgekeurd. </p>
                        </li>
                    ))}
                </ul>
            )}
        </div>
    );
};

export default ZakelijkHuurderMeldingen;    