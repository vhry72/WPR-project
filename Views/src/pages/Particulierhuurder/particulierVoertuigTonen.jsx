import React, { useState, useEffect } from "react";
import { useNavigate } from "react-router-dom";
import "../../styles/ParticulierVoertuigTonen.css";
import VoertuigRequestService from "../../services/requests/VoertuigRequestService";
import JwtService from "../../services/JwtService";

const ParticulierVoertuigTonen = () => {
    const [voertuigen, setVoertuigen] = useState([]);
    const [filterType, setFilterType] = useState("auto");
    const [filterCriteria, setFilterCriteria] = useState("");
    const navigate = useNavigate();

    useEffect(() => {
        handleVoertuigType();
    }, []);

    const handleChange = (event) => {
        setFilterType(event.target.value);
    };

    const handleCriteriaChange = (event) => {
        setFilterCriteria(event.target.value.toLowerCase());
    };

    const handleVoertuigType = async () => {
        try {
            console.log("Voertuigen worden opgevraagd");
            const response = await VoertuigRequestService.getAll(filterType);
            const filteredVoertuigen = response.filter(v => v.voertuigBeschikbaar);
            setVoertuigen(filteredVoertuigen);
        } catch (error) {
            console.error("Het is niet gelukt om de voertuigen op te halen", error);
        }
    };

    const filteredVoertuigen = voertuigen.filter((v) =>
        Object.values(v).some(value => value?.toString().toLowerCase().includes(filterCriteria))
    );

    const handleSort = (criteria) => {
        const sortedVoertuigen = [...filteredVoertuigen].sort((a, b) => {
            if (a[criteria] < b[criteria]) return -1;
            if (a[criteria] > b[criteria]) return 1;
            return 0;
        });
        setVoertuigen(sortedVoertuigen);
    };

    const handleVoertuigClick = (voertuig) => {
        navigate(
            `/huurVoertuig?kenteken=${voertuig.kenteken}&VoertuigID=${voertuig.voertuigId}&SoortHuurder=Particulier`
        );
    };

    return (
        <div className="container">
            <div className="input-container">
                <select value={filterType} onChange={handleChange} className="input-field">
                    <option value="auto">Auto</option>
                    <option value="camper">Camper</option>
                    <option value="caravan">Caravan</option>
                </select>
                <input
                    type="text"
                    placeholder="Filter op kenmerken (bijv. kleur, aantal deuren)"
                    value={filterCriteria}
                    onChange={handleCriteriaChange}
                    className="input-field"
                />
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
                        <th>Beschikbaar tot</th>
                    </tr>
                </thead>
                <tbody>
                    {filteredVoertuigen.map((voertuig, index) => (
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
                            <td>{voertuig.beschikbaarTot}</td>
                        </tr>
                    ))}
                </tbody>
            </table>
        </div>
    );
};

export default ParticulierVoertuigTonen;
