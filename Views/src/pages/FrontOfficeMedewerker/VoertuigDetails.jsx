import React, { useState } from "react";
import { useNavigate } from "react-router-dom";
import "../../styles/ParticulierVoertuigTonen.css";
import axios from "axios";

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
            const response = await axios.get(
                `https://localhost:5033/api/Voertuig/VoertuigType?voertuigType=${filterType}`
            );
            setVoertuigen(response.data);
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
                <button onClick={() => handleSort("aantalDeuren")} className="sort-button">
                    Sorteer op deuren
                </button>
                <button onClick={() => handleSort("aantalSlaapplekken")} className="sort-button">
                    Sorteer op slaapplekken
                </button>
            </div>
            <div className="card-container">
                {voertuigen.map((voertuig, index) => (
                    <div key={index} className="card">
                        <img
                            src={`data:image/jpeg;base64,${voertuig.afbeelding}`}
                            alt={`${voertuig.merk} ${voertuig.model}`}
                            className="card-image"
                        />
                        <div className="card-content">
                            <h3>{voertuig.merk} {voertuig.model}</h3>
                            <p><strong>Type:</strong> {voertuig.voertuigType}</p>
                            <p><strong>Prijs per dag:</strong> {new Intl.NumberFormat('nl-NL', { style: 'currency', currency: 'EUR' }).format(voertuig.prijsPerDag)}</p>
                            <p><strong>Bouwjaar:</strong> {voertuig.bouwjaar}</p>
                            <p><strong>Kenteken:</strong> {voertuig.kenteken}</p>
                            <p><strong>Kleur:</strong> {voertuig.kleur}</p>
                            <p><strong>Aantal Deuren:</strong> {voertuig.aantalDeuren}</p>
                            <p><strong>Aantal Slaapplekken:</strong> {voertuig.aantalSlaapplekken}</p>
                        </div>
                    </div>
                ))}
            </div>
        </div>
    );
};

export default VoertuigTonen;
