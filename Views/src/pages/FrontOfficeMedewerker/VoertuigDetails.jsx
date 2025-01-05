import React, { useState } from "react";
import { useNavigate } from "react-router-dom";

const FrontOfficeDetails = () => {
    const [voertuigen, setVoertuigen] = useState([]);
    const navigate = useNavigate();
    const HuurderId = new URLSearchParams(location.search).get("HuurderID");

    return (
        <div className="container">
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
                    </tr>
                </thead>
                <tbody>
                    {voertuigen.map((voertuig, index) => (
                        <tr key={index}>
                            <td>{voertuig.model}</td>
                            <td>{voertuig.prijsPerDag}</td>
                            <td>{voertuig.voertuigType}</td>
                            <td>{voertuig.bouwjaar}</td>
                            <td>{voertuig.kenteken}</td>
                            <td>{voertuig.kleur}</td>
                            <td>{voertuig.voertuigBeschikbaar ? "Ja" : "Nee"}</td>
                        </tr>
                    ))}
                </tbody>
            </table>
        </div>
    );
};


export default FrontOfficeDetails;
