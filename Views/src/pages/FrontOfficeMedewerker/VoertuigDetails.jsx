import React, { useState } from "react";
import { useNavigate } from "react-router-dom";
import "../../styles/ParticulierVoertuigTonen.css";
import VoertuigRequestService from "../../services/requests/VoertuigRequestService";

const VoertuigTonen = () => {
    const [voertuigen, setVoertuigen] = useState([]);
    const [filterType, setFilterType] = useState("auto");
    const navigate = useNavigate();

    const handleChange = (event) => {
        setFilterType(event.target.value);
    };

    const handleVoertuigType = async () => {
        try {
            console.log("Voertuigen worden opgevraagd");
            const response = await VoertuigRequestService.getAll(filterType);
            setVoertuigen(response);
        } catch (error) {
            console.error("Het is niet gelukt om de voertuigtype op te halen", error);
        }
    };

    const handleSort = (criteria) => {
        const sortedVoertuigen = [...voertuigen].sort((a, b) => {
            if (a[criteria] < b[criteria]) return -1;
            if (a[criteria] > b[criteria]) return 1;
            return 0;
        });
        setVoertuigen(sortedVoertuigen);
    };
    const handleVoertuigClick = (voertuig) => {
        navigate(`/VoertuigNotitieToevoegen/${voertuig.voertuigId}`); // Gebruik routeparameters
    };

   

    return (
        <div className="container">
            <div className="input-container">
                <select value={filterType} onChange={handleChange} className="input-field">
                    <option value="auto">Auto</option>
                    <option value="camper">Camper</option>
                    <option value="caravan">Caravan</option>
                </select>
                <button onClick={handleVoertuigType} className="button">
                    Voer voertuigType in
                </button>
            </div>
            <div className="button-container">
                <button onClick={() => handleSort("merk")} className="sort-button">
                    Sorteer op Merk
                </button>
                <button onClick={() => handleSort("model")} className="sort-button">
                    Sorteer op Model
                </button>
                <button onClick={() => handleSort("prijsPerDag")} className="sort-button">
                    Sorteer op Prijs
                </button>
                <button onClick={() => handleSort("bouwjaar")} className="sort-button">
                    Sorteer op Bouwjaar
                </button>
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
                        <th>Beschikbaar</th>
                        <th>Notities</th>
                    </tr>
                </thead>
                <tbody>
                    {voertuigen.map((voertuig, index) => (
                        <tr key={index}>
                            <td
                                onClick={() => handleVoertuigClick(voertuig)}
                                style={{
                                    cursor: "pointer",
                                    color: "blue",
                                    textDecoration: "underline",
                                }}
                            >
                                {voertuig.merk}
                            </td>
                            <td>{voertuig.model}</td>
                            <td>{voertuig.prijsPerDag}</td>
                            <td>{voertuig.voertuigType}</td>
                            <td>{voertuig.bouwjaar}</td>
                            <td>{voertuig.kenteken}</td>
                            <td>{voertuig.kleur}</td>
                            <td>{voertuig.voertuigBeschikbaar ? "Ja" : "Nee"}</td>
                            <td>{voertuig.notitie}</td>
                        </tr>
                    ))}
                </tbody>
            </table>
        </div>
    );
};

export default VoertuigTonen;
