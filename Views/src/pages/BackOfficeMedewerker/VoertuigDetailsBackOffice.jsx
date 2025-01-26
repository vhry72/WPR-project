import React, { useEffect, useState } from "react";
import { useParams, useNavigate } from "react-router-dom";
import axios from "axios";


const VoertuigDetails = () => {
    const { voertuigId } = useParams();
    const navigate = useNavigate();
    const [voertuig, setVoertuig] = useState(null);
    const [formData, setFormData] = useState({ prijsPerDag: "", kleur: "", kenteken: "" });
    const [error, setError] = useState(null);

    useEffect(() => {
        const fetchVoertuigDetails = async () => {
            try {
                const response = await axios.get(
                    `https://localhost:5033/api/Voertuig/${voertuigId}`);
                    console.log(response)
                setVoertuig(response.data);
                setFormData({
                    prijsPerDag: response.prijsPerDag,
                    kleur: response.kleur,
                    kenteken: response.kenteken,
                });
            } catch (err) {
                console.error("Fout bij ophalen voertuigdetails:", err);
                setError(err);
            }
        };

        fetchVoertuigDetails();
    }, [voertuigId]);

    const handleInputChange = (e) => {
        const { name, value } = e.target;
        setFormData((prev) => ({ ...prev, [name]: value }));
    };

    const handleVoertuigChange = async (e) => {
        e.preventDefault();
        try {
            const response = await axios.put(
                `https://localhost:5033/api/Voertuig/veranderGegevens/${voertuigId}`,
                formData
            );
            if (response.status === 200) {
                alert("Gegevens succesvol bijgewerkt!");
                setVoertuig((prev) => ({ ...prev, ...formData }));
            }
        } catch (err) {
            console.error("Fout bij het bijwerken van de gegevens:", err);
            setError(err);
        }
    };

   
    const handleDelete = async () => {
        const bevestigen = window.confirm("Weet je zeker dat je dit voertuig wilt verwijderen?");
        if (bevestigen) {
            try {
                const response = await axios.delete(
                    `https://localhost:5033/api/Voertuig/verwijderVoertuig/${voertuigId}`
                );

                // Toon een normale alert bij succes
                alert(response.data.Message || "Voertuig succesvol verwijderd!");

                // Navigeer na succesvolle verwijdering
                navigate(`/VoertuigTonen`);
            } catch (err) {
                console.error("Fout bij verwijderen voertuig:", err);

                // Toon een alert bij een fout
                alert("Er is een fout opgetreden bij het verwijderen van het voertuig.");
            }
        }
    };




    if (error) {
        return <div>Er is een fout opgetreden: {error.message}</div>;
    }

    if (!voertuig) {
        return <div>Gegevens laden...</div>;
    }

    return (
        <div>
            <h1>Voertuig Details</h1>
            <p>Merk: {voertuig.merk}</p>
            <p>Model: {voertuig.model}</p>
            <p>Prijs Per Dag: {voertuig.prijsPerDag}</p>
            <p>Kleur: {voertuig.kleur}</p>
            <p>Bouwjaar: {voertuig.bouwjaar}</p>
            <form onSubmit={handleVoertuigChange}>
                <div>
                    <label>Prijs Per Dag:</label>
                    <input
                        type="number"
                        name="prijsPerDag"
                        value={formData.prijsPerDag}
                        onChange={handleInputChange}
                    />
                </div>
                <div>
                    <label>Kleur:</label>
                    <input
                        type="text"
                        name="kleur"
                        value={formData.kleur}
                        onChange={handleInputChange}
                    />
                </div>
                <div>
                    <label>Kenteken:</label>
                    <input
                        type="text"
                        name="kenteken"
                        value={formData.kenteken}
                        onChange={handleInputChange}
                    />
                </div>
                <button type="submit">Verander Gegevens</button>
            </form>
            <button
                onClick={handleDelete}
                style={{
                    backgroundColor: "red",
                    color: "white",
                    padding: "10px",
                    border: "none",
                    cursor: "pointer",
                }}
            >
                Verwijder Voertuig
            </button>
            <button onClick={() => navigate(`/VoertuigTonen`)} className="button">
                Terug naar Overzicht
            </button>
            
        </div>

    );
};

export default VoertuigDetails;
