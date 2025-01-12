import React, { useState } from "react";
import axios from "axios";
import { toast, ToastContainer } from "react-toastify";
import "react-toastify/dist/ReactToastify.css";

const CreateVoertuig = () => {
    const [formData, setFormData] = useState({
        merk: "",
        model: "",
        bouwjaar: "",
        kleur: "",
        kenteken: "",
        voertuigType: "",
    });

    // Functie voor formulierinvoer
    const handleInputChange = (e) => {
        const { name, value } = e.target;
        setFormData((prevFormData) => ({
            ...prevFormData,
            [name]: value,
        }));
    };

    // Functie voor het indienen van het formulier
    const handleSubmit = async (e) => {
        e.preventDefault();

        try {
            const response = await axios.post(
                "https://localhost:5033/api/Voertuig/maakVoertuig", // Pas aan naar jouw API-url
                formData
            );

            // Succesmelding
            toast.success(response.data.Message || "Voertuig succesvol aangemaakt!", {
                position: "top-right",
                autoClose: 5000,
                hideProgressBar: true,
                closeOnClick: true,
                pauseOnHover: true,
                draggable: true,
            });

            // Reset het formulier
            setFormData({
                merk: "",
                model: "",
                bouwjaar: "",
                kleur: "",
                kenteken: "",
                voertuigType: "",
            });
        } catch (error) {
            // Foutmelding
            toast.error(
                error.response?.data?.Message || "Er is iets fout gegaan.",
                {
                    position: "top-right",
                    autoClose: 5000,
                    hideProgressBar: true,
                    closeOnClick: true,
                    pauseOnHover: true,
                    draggable: true,
                }
            );
        }
    };

    return (
        <div>
            <h2>Voeg een nieuw voertuig toe</h2>
            <form onSubmit={handleSubmit}>
                <div>
                    <label>Merk:</label>
                    <input
                        type="text"
                        name="merk"
                        value={formData.merk}
                        onChange={handleInputChange}
                        required
                    />
                </div>
                <div>
                    <label>Model:</label>
                    <input
                        type="text"
                        name="model"
                        value={formData.model}
                        onChange={handleInputChange}
                        required
                    />
                </div>
                <div>
                    <label>Bouwjaar:</label>
                    <input
                        type="number"
                        name="bouwjaar"
                        value={formData.bouwjaar}
                        onChange={handleInputChange}
                        required
                    />
                </div>
                <div>
                    <label>Kleur:</label>
                    <input
                        type="text"
                        name="kleur"
                        value={formData.kleur}
                        onChange={handleInputChange}
                        required
                    />
                </div>
                <div>
                    <label>Kenteken:</label>
                    <input
                        type="text"
                        name="kenteken"
                        value={formData.kenteken}
                        onChange={handleInputChange}
                        required
                    />
                </div>
                <div>
                    <label>Voertuig type:</label>
                    <select
                        name="voertuigType"
                        value={formData.voertuigType}
                        onChange={handleInputChange}
                        required
                    >
                        <option value="" disabled>
                            Selecteer een voertuigtype
                        </option>
                        <option value="auto">Auto</option>
                        <option value="camper">Camper</option>
                        <option value="caravan">Caravan</option>
                    </select>
                </div>
                <button type="submit">Verstuur</button>
            </form>

            {/* Toast-container */}
            <ToastContainer />
        </div>
    );
};

export default CreateVoertuig;
