import React, { useState, useEffect } from 'react';
import { useLocation, useNavigate } from 'react-router-dom';
import JwtService from '../../services/JwtService';
import axios from 'axios';
import '../../styles/Abonnement.css';

function AbonnementWijzigen() {
    const location = useLocation();
    const navigate = useNavigate();
    const [beheerderId, setBeheerderId] = useState(null);
    const [abonnementDetails, setAbonnementDetails] = useState(location.state?.selectedAbonnement || {});
    const [nieuweTermijn, setNieuweTermijn] = useState(abonnementDetails.termijn);
    const [directZichtbaar, setDirectZichtbaar] = useState(false);

    useEffect(() => {
        const fetchBeheerderId = async () => {
            try {
                const userId = await JwtService.getUserId();
                if (userId) {
                    setBeheerderId(userId);
                }
            } catch (error) {
                console.error("Fout bij ophalen beheerderId:", error);
            }
        };
        fetchBeheerderId();
    }, []);

    const handleWijzigAbonnement = async () => {
        if (!beheerderId) {
            alert("Beheerder ID ontbreekt. Probeer opnieuw.");
            return;
        }

        const abonnementWijzigDTO = {
            abonnementId: abonnementDetails.id,
            abonnementType: nieuweTermijn,
            directZichtbaar: directZichtbaar,
            aantalDagen: directZichtbaar ? null : 30
        };

        try {
            await axios.post(`https://localhost:5001/api/Abonnement/${beheerderId}/abonnement/wijzig/${abonnementDetails.id}`, abonnementWijzigDTO);
            alert("Abonnement succesvol gewijzigd!");
            navigate('/');
        } catch (error) {
            console.error('Fout bij wijzigen van abonnement:', error);
            alert('Fout bij wijzigen van abonnement. Probeer het later opnieuw.');
        }
    };

    return (
        <div className="subscription-container">
            <h1>Wijzig je Abonnement</h1>
            <p>Abonnement: {abonnementDetails.naam}</p>
            <label>
                Kies nieuwe termijn:
                <select value={nieuweTermijn} onChange={(e) => setNieuweTermijn(e.target.value)}>
                    <option value="Maandelijks">Maandelijks</option>
                    <option value="Kwartaal">Kwartaal</option>
                    <option value="Jaarlijks">Jaarlijks</option>
                </select>
            </label>

            <label>
                Direct Zichtbaar:
                <input
                    type="checkbox"
                    checked={directZichtbaar}
                    onChange={(e) => setDirectZichtbaar(e.target.checked)}
                />
            </label>

            <button onClick={handleWijzigAbonnement}>Wijzig Abonnement</button>
        </div>
    );
}

export default AbonnementWijzigen;
