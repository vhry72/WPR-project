import React, { useState, useEffect } from "react";
import { Link } from "react-router-dom";
import "../styles/ParticulierVoertuigTonen.css";
import VoertuigRequestService from "../services/requests/VoertuigRequestService";

const ParticulierVoertuigTonen = () => {
    const [voertuigen, setVoertuigen] = useState([]);
    const [filterType, setFilterType] = useState("alle");
    const [sortCriteria, setSortCriteria] = useState("prijs");
    const [searchTerm, setSearchTerm] = useState("");

    // **API Call: Voertuigen ophalen**
    useEffect(() => {
        const fetchVoertuigen = async () => {
            try {
                const queryParams = new URLSearchParams({
                    voertuigType: filterType === "alle" ? "" : filterType,
                    sorteerOptie: sortCriteria,
                });

                const response = await VoertuigRequestService.getAll(`/Voertuig/filter?${queryParams}`);
                if (!response.ok) throw new Error("Failed to fetch voertuigen");

                const data = await response.json();
                setVoertuigen(data);
            } catch (error) {
                console.error("Error fetching voertuigen:", error);
            }
        };

        fetchVoertuigen();
    }, [filterType, sortCriteria]); // Opnieuw ophalen als filter of sortering wijzigt

    // Filteren en zoeken op frontend
    const gefilterdeVoertuigen = voertuigen.filter((voertuig) =>
        voertuig.merk.toLowerCase().includes(searchTerm.toLowerCase())
    );

    return (
        <div className="particulier-voertuig-tonen">
            <h1>Voertuigen Selecteren</h1>
            <div className="filters">
                <label>
                    Type:
                    <select
                        value={filterType}
                        onChange={(e) => setFilterType(e.target.value)}
                    >
                        <option value="alle">Alle</option>
                        <option value="auto">Auto</option>
                        <option value="camper">Camper</option>
                        <option value="caravan">Caravan</option>
                    </select>
                </label>
                <label>
                    Sorteren op:
                    <select
                        value={sortCriteria}
                        onChange={(e) => setSortCriteria(e.target.value)}
                    >
                        <option value="prijs">Prijs</option>
                        <option value="merk">Merk</option>
                        <option value="beschikbaarheid">Beschikbaarheid</option>
                    </select>
                </label>
                <label>
                    Zoeken:
                    <input
                        type="text"
                        placeholder="Zoek op merk of model"
                        value={searchTerm}
                        onChange={(e) => setSearchTerm(e.target.value)}
                    />
                </label>
            </div>
            <ul className="voertuigen-lijst">
                {gefilterdeVoertuigen.length > 0 ? (
                    gefilterdeVoertuigen.map((voertuig) => (
                        <li key={voertuig.id} className="voertuig-item">
                            <h2>
                                {voertuig.merk} - {voertuig.model}
                            </h2>
                            <p>Type: {voertuig.type}</p>
                            <p>Prijs: €{voertuig.prijs}</p>
                            <p>{voertuig.beschikbaar ? "Beschikbaar" : "Niet beschikbaar"}</p>
                        </li>
                    ))
                ) : (
                    <p>Geen voertuigen gevonden.</p>
                )}
            </ul>
        </div>
    );
};

export default ParticulierVoertuigTonen;
