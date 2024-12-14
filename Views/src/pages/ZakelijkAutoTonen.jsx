import React, { useEffect, useState } from 'react';
import "../styles/ParticulierVoertuigTonen.css";
import VoertuigRequestService from "../services/requests/VoertuigRequestService";
import { useNavigate } from 'react-router-dom';

const ZakelijkAutoTonen = () => {
    const [voertuigen, setVoertuigen] = useState([]);
    const [filterType, setFilterType] = useState("auto");
    const navigate = useNavigate();

    useEffect(() => {
        const fetchVoertuigen = async () => {
            try {
                console.log("Voertuigen worden opgevraagd");
                const response = await VoertuigRequestService.getAll(filterType);
                setVoertuigen(response);
            } catch (error) {
                console.error("Het is niet gelukt om de voertuigtype op te halen", error);
            }
        };

        fetchVoertuigen();
    }, [filterType]); // fetch vehicles whenever filterType changes

    const handleSort = (criteria) => {
        const sortedVoertuigen = [...voertuigen].sort((a, b) => {
            if (a[criteria] < b[criteria]) return -1;
            if (a[criteria] > b[criteria]) return 1;
            return 0;
        });
        setVoertuigen(sortedVoertuigen);
    };

    const handleVoertuigClick = (kenteken) => {
        navigate(`/huurzakelijkvoertuig/${kenteken}`);
    };

    return (
        <div className="container">
            <div className="button-container">
                <button onClick={() => handleSort("merk")} className="sort-button">Sorteer op Merk</button>
                <button onClick={() => handleSort("model")} className="sort-button">Sorteer op Model</button>
                <button onClick={() => handleSort("prijsPerDag")} className="sort-button">Sorteer op Prijs</button>
                <button onClick={() => handleSort("bouwjaar")} className="sort-button">Sorteer op Bouwjaar</button>
            </div>
            <table className="styled-table">
                <thead>
                    <tr>
                        <th>Merk</th>
                        <th>Model</th>
                        <th>Prijs Per Dag</th>
                        <th>Voertuig Type</th>
                        <th>Bouwjaar</th>
                        <th>Kenteken</th>
                        <th>Kleur</th>
                    </tr>
                </thead>
                <tbody>
                    {voertuigen.map((voertuig, index) => (
                        <tr key={index}>
                            <td onClick={() => handleVoertuigClick(voertuig.kenteken)} style={{ cursor: 'pointer', color: 'blue', textDecoration: 'underline' }}>{voertuig.merk}</td>
                            <td>{voertuig.model}</td>
                            <td>{voertuig.prijsPerDag}</td>
                            <td>{voertuig.voertuigType}</td>
                            <td>{voertuig.bouwjaar}</td>
                            <td>{voertuig.kenteken}</td>
                            <td>{voertuig.kleur}</td>
                        </tr>
                    ))}
                </tbody>
            </table>
        </div>
    );
};

export default ZakelijkAutoTonen;
