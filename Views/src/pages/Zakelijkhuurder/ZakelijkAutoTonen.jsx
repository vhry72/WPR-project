import React, { useEffect, useState } from 'react';
import "../../styles/ParticulierVoertuigTonen.css";
import { useNavigate } from 'react-router-dom';

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
    }

    const startdatum = formatDateForDB(startDateTime);
    const einddatum = formatDateForDB(endDateTime)


    useEffect(() => {
        const fetchVoertuigen = async () => {
            try {
                const response = await axios.get(`https://localhost:5033/api/Huurverzoek/BeschikbareVoertuigen/${startdatum}/${einddatum}`);
                setAlleVoertuigen(response.data);  // Verondersteld dat de response een array is
                filterVoertuigen(filterType, response.data);
            } catch (error) {
                console.error("Het is niet gelukt om de voertuigen op te halen", error);
            }
        };
        fetchVoertuigen();
    }, []);

    const filterVoertuigen = (type, voertuigenArray) => {
        const filtered = voertuigenArray.filter(v => v.voertuigType.toLowerCase() === type.toLowerCase());
        setVoertuigen(filtered);
    };

    const handleChange = (event) => {
        setFilterType(event.target.value);
        filterVoertuigen(event.target.value, alleVoertuigen);
    };

    const handleCriteriaChange = (event) => {
        setFilterCriteria(event.target.value.toLowerCase());
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
        console.log(voertuig);
        const kenteken = voertuig.kenteken;
        const VoertuigID = voertuig.voertuigId;

        console.log(VoertuigID);

        const reservationData = {
            startDateTime,
            endDateTime,
            userRole,
            kenteken,
            VoertuigID
        };

        navigate(
            `/bevestigingHuur`, { state: reservationData });
    };

    return (
        <div className="container">
            <div className="input-container">
                <div className="input-field">Auto's</div>

                <input
                    type="text"
                    placeholder="Filter op kenmerken (bijv. kleur, aantal deuren)"
                    value={filterCriteria}
                    onChange={handleCriteriaChange}
                    className="input-field"
                />
                <button onClick={fetchVoertuigen} className="button">
                    Voer voertuigType in
                </button>
            </div>
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
                        <th>Beschikbaar</th>
                        <th>Beschikbaar tot</th>
                    </tr>
                </thead>
                <tbody>
                    {filteredVoertuigen.map((voertuig, index) => (
                        <tr key={index}>
                            <td
                                onClick={() => handleVoertuigClick(voertuig)}
                                style={{ cursor: "pointer", color: "blue", textDecoration: "underline" }}
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

export default ZakelijkAutoTonen;
