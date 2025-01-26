import React, { useEffect, useState } from "react";
import { useParams, useNavigate } from "react-router-dom";
import axios from "axios";

const VoertuigDetails = () => {
    const { voertuigId } = useParams();
    const navigate = useNavigate();
    const [voertuig, setVoertuig] = useState(null);
    const [formData, setFormData] = useState({
        merk: "",
        model: "",
        kleur: "",
        prijsPerDag: "",
        bouwjaar: "",
        kenteken: "",
        AantalDeuren: "",
        AantalSlaapplekken: ""
    });
    const [afbeelding, setAfbeelding] = useState(null);
    const [error, setError] = useState(null);

    useEffect(() => {
        const fetchVoertuigDetails = async () => {
            try {
                const response = await axios.get(`https://localhost:5033/api/Voertuig/${voertuigId}`);
                if (response.data) {
                    setVoertuig(response.data);
                    setFormData({
                        merk: response.data.merk || "",
                        model: response.data.model || "",
                        kleur: response.data.kleur || "",
                        prijsPerDag: response.data.prijsPerDag || "",
                        bouwjaar: response.data.bouwjaar || "",
                        kenteken: response.data.kenteken || "",
                        AantalDeuren: response.data.AantalDeuren || "",
                        AantalSlaapplekken: response.data.AantalSlaapplekken || ""
                    });
                }
            } catch (err) {
                console.error("Fout bij ophalen voertuigdetails:", err);
                setError(err);
            }
        };

        fetchVoertuigDetails();
    }, [voertuigId]);

    const handleInputChange = (e) => {
        const { name, value } = e.target;
        setFormData(prev => ({ ...prev, [name]: value }));
    };

    const handleFileChange = (e) => {
        setAfbeelding(e.target.files[0]);
    };

    const handleVoertuigChange = async (e) => {
        e.preventDefault();

        const uploadData = new FormData();
        Object.entries(formData).forEach(([key, value]) => {
            uploadData.append(key, value);
        });
        if (afbeelding) {
            uploadData.append("afbeelding", afbeelding);
        }

        try {
            await axios.put(`https://localhost:5033/api/Voertuig/veranderGegevens/${voertuigId}`, uploadData, { withCredentials: true });
            alert("Voertuiggegevens succesvol bijgewerkt!");
            navigate(`/VoertuigTonen`);
        } catch (err) {
            console.error("Fout bij het bijwerken van de gegevens:", err.response ? err.response.data : err);
            setError(err.response ? err.response.data : err);
        }
    };


    return (
        <div>
            <h1>Voertuig Details</h1>
            <form onSubmit={handleVoertuigChange}>
                <div>
                    <label>Merk:</label>
                    <input type="text" name="merk" value={formData.merk} onChange={handleInputChange} />
                </div>
                <div>
                    <label>Model:</label>
                    <input type="text" name="model" value={formData.model} onChange={handleInputChange} />
                </div>
                <div>
                    <label>Kleur:</label>
                    <input type="text" name="kleur" value={formData.kleur} onChange={handleInputChange} />
                </div>
                <div>
                    <label>Prijs Per Dag:</label>
                    <input type="number" name="prijsPerDag" value={formData.prijsPerDag} onChange={handleInputChange} />
                </div>
                <div>
                    <label>Bouwjaar:</label>
                    <input type="number" name="bouwjaar" value={formData.bouwjaar} onChange={handleInputChange} />
                </div>
                <div>
                    <label>Kenteken:</label>
                    <input type="text" name="kenteken" value={formData.kenteken} onChange={handleInputChange} />
                </div>
                <div>
                    <label>Aantal Deuren:</label>
                    <input type="number" name="AantalDeuren" value={formData.AantalDeuren} onChange={handleInputChange} />
                </div>
                <div>
                    <label>Aantal Slaapplekken:</label>
                    <input type="number" name="AantalSlaapplekken" value={formData.AantalSlaapplekken} onChange={handleInputChange} />
                </div>
                <div>
                    <label>Afbeelding:</label>
                    <input type="file" onChange={handleFileChange} />
                </div>
                <button type="submit">Verander Gegevens</button>
            </form>
            {error && <div>Er is een fout opgetreden: {error.message}</div>}
        </div>
    );
};

export default VoertuigDetails;
