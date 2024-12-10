import React, { useState, useEffect } from "react";
import { Link } from "react-router-dom";
import "../styles/ParticulierVoertuigTonen.css";
import VoertuigRequestService from "../services/requests/VoertuigRequestService";

const ParticulierVoertuigTonen = () => {
    const [voertuigen, setVoertuigen] = useState([]);
    const [filterType, setFilterType] = useState("auto");
    const [sortCriteria, setSortCriteria] = useState("prijs");
    const [searchTerm, setSearchTerm] = useState("");

    const handleChange = (event) => {
        setFilterType(event.target.value); // Bijwerken van de filterType waarde
    };

    const handleVoertuigType = async () => {
        try {
            console.log("Voertuigen worden opgevraagd");
            await VoertuigRequestService.getAll(filterType)
        } catch {
            console.error("Het is niet gelukt om de voertuigtype op te halen", error)
        }
    };


    return (
        <div style={{ marginBottom: '20px' }}>
            <input
                type="text"
                placeholder="Enter VoertuigType"
                value={filterType}
                onChange={ handleChange }
            />
            <button onClick={handleVoertuigType}>Voer voertuigType in</button>
        </div>
        );
    };






export default ParticulierVoertuigTonen;
