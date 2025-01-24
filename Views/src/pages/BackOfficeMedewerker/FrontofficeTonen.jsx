import React, { useState, useEffect } from "react";
import { useNavigate } from "react-router-dom";
import axios from "axios";
import "../../styles/FrontofficeBeheer.css";

const API_URL = "https://localhost:5033";

const FrontofficeTonen = () => {
    const [medewerkers, setMedewerkers] = useState([]);
    const navigate = useNavigate();

    useEffect(() => {
        const fetchMedewerkers = async () => {
            try {
                const response = await axios.get(`${API_URL}/api/FrontOfficeMedewerker/GetAll`);
                setMedewerkers(response.data);
            } catch (error) {
                console.error("Fout bij het ophalen van medewerkers:", error);
            }
        };

        fetchMedewerkers();
    }, []);

    const handleMedewerkerClick = (medewerkerId) => {
        const data = { medewerkerId };
        navigate(`/FrontofficeDetails`, { state: data });
    };

    const handleToevoegenClick = () => {
        navigate(`/FrontofficeToevoegen`);
    };

    return (
        <div className="container">
            <h2>Frontoffice Medewerkers</h2>
            <button onClick={handleToevoegenClick} className="button">
                Nieuwe Medewerker Toevoegen
            </button>
            <table className="styled-table">
                <thead>
                    <tr>
                        <th>Naam</th>
                        <th>E-mailadres</th>
                        <th>Acties</th>
                    </tr>
                </thead>
                <tbody>
                    {medewerkers.map((medewerker) => (
                        <tr key={medewerker.frontofficeMedewerkerId}>
                            <td>{medewerker.medewerkerNaam}</td>
                            <td>{medewerker.medewerkerEmail}</td>
                            <td>
                                <button
                                    onClick={() => handleMedewerkerClick(medewerker.frontofficeMedewerkerId)}
                                    className="button details"
                                >
                                    Details
                                </button>
                            </td>
                        </tr>
                    ))}
                </tbody>
            </table>
        </div>
    );
};

export default FrontofficeTonen;
