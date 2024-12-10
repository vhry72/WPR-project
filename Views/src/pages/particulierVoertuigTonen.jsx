import React, { useState } from "react";
import "../styles/ParticulierVoertuigTonen.css";
import VoertuigRequestService from "../services/requests/VoertuigRequestService";

const ParticulierVoertuigTonen = () => {
    const [voertuigen, setVoertuigen] = useState([]);
    const [filterType, setFilterType] = useState("auto");

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

    return (
        <div className="container">
            <div className="input-container">
                <input
                    type="text"
                    placeholder="Enter VoertuigType"
                    value={filterType}
                    onChange={handleChange}
                    className="input-field"
                />
                <button onClick={handleVoertuigType} className="button">
                    Voer voertuigType in
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
                    </tr>
                </thead>
                <tbody>
                    {voertuigen.map((voertuig, index) => (
                        <tr key={index}>
                            <td>{voertuig.merk}</td>
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

export default ParticulierVoertuigTonen;
