import React, { useEffect, useState } from 'react';
import "../../styles/ParticulierVoertuigTonen.css";
import { useNavigate, useLocation } from 'react-router-dom';
import axios from 'axios';

const ZakelijkAutoTonen = () => {
    const [voertuigen, setVoertuigen] = useState([]);
    const [alleVoertuigen, setAlleVoertuigen] = useState([]);
    const [filterType, setFilterType] = useState("auto");
    const [filterCriteria, setFilterCriteria] = useState("");
    const location = useLocation();
    const { startDateTime, endDateTime, userRole } = location.state;
    const navigate = useNavigate();

    const formatDateForDB = (dateString) => {
        return new Date(dateString).toISOString().replace('T', ' ').slice(0, 19);
    };

    const startdatum = formatDateForDB(startDateTime);
    const einddatum = formatDateForDB(endDateTime);

    const fetchVoertuigen = async () => {
        try {
            const response = await axios.get(
                `https://localhost:5033/api/Huurverzoek/BeschikbareVoertuigen/${startdatum}/${einddatum}`
            );
            setAlleVoertuigen(response.data); // Verondersteld dat de response een array is
            filterVoertuigen(filterType, response.data);
        } catch (error) {
            console.error("Het is niet gelukt om de voertuigen op te halen", error);
        }
    };

    useEffect(() => {
        fetchVoertuigen();
    }, []);

    const filterVoertuigen = (type, voertuigenArray) => {
        const filtered = voertuigenArray.filter(
            (v) => v.voertuigType.toLowerCase() === type.toLowerCase()
        );
        setVoertuigen(filtered);
    };

    const handleCriteriaChange = (event) => {
        setFilterCriteria(event.target.value.toLowerCase());
    };

    const filteredVoertuigen = voertuigen.filter((v) =>
        Object.values(v).some((value) =>
            value?.toString().toLowerCase().includes(filterCriteria)
        )
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
        const kenteken = voertuig.kenteken;
        const VoertuigID = voertuig.voertuigId;

        const reservationData = {
            startDateTime,
            endDateTime,
            userRole,
            kenteken,
            VoertuigID,
        };

        navigate(`/bevestigingHuur`, { state: reservationData });
    };

    return (
        <div className="container">
            <div className="input-container">
                <input
                    type="text"
                    placeholder="Filter op kenmerken (bijv. kleur, aantal deuren)"
                    value={filterCriteria}
                    onChange={handleCriteriaChange}
                    className="input-field"
                />
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
            <div className="card-container">
                {filteredVoertuigen.map((voertuig, index) => (
                    <div
                        key={index}
                        className="card"
                        onClick={() => handleVoertuigClick(voertuig)}
                    >
                        <img
                            src={`data:image/jpeg;base64,${voertuig.afbeelding}`}
                            alt={`${voertuig.merk} ${voertuig.model}`}
                            className="card-image"
                        />
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
                        <div className="card-actions">
                            <button>Reserveer</button>
                            <button>Details</button>
                        </div>
                    </div>
                ))}
            </div>
        </div>
    );
};

export default ZakelijkAutoTonen;
