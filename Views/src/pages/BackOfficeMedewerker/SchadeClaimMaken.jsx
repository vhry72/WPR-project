import React, { useState } from "react";
import axios from "axios";
import { toast, ToastContainer } from "react-toastify";
import "react-toastify/dist/ReactToastify.css"; // Dit is de CSS voor de toast-notificaties

const CreateSchadeMelding = () => {
    const [formData, setFormData] = useState({
        beschrijving: "",
        datum: "",
        status: "",
        opmerkingen: "",
        voertuigId: "", // Het voertuig ID dat je wilt koppelen
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
            // Verstuur de schadeclaim naar de API
            const response = await axios.post(
                "https://localhost:5033/api/Schademelding/maak", // Verander dit naar de juiste URL voor jouw API
                formData
            );

            // Toon de succes-notificatie
            toast.success(response.data.Message || "Schademelding succesvol aangemaakt!", {
                position: "top-right",
                autoClose: 5000, // De toast verdwijnt na 5 seconden
                hideProgressBar: true,
                closeOnClick: true,
                pauseOnHover: true,
                draggable: true,
                progress: undefined,
            });
        } catch (error) {
            // Toon de fout-notificatie
            toast.error(
                error.response?.data?.Message || "Er is iets fout gegaan.",
                {
                    position: "top-right",
                    autoClose: 5000,
                    hideProgressBar: true,
                    closeOnClick: true,
                    pauseOnHover: true,
                    draggable: true,
                    progress: undefined,
                }
            );
        }
    };

    return (
        <div>
            <h2>Maak een nieuwe Schademelding</h2>
            <form onSubmit={handleSubmit}>
                <div>
                    <label>Beschrijving:</label>
                    <input
                        type="text"
                        name="beschrijving"
                        value={formData.beschrijving}
                        onChange={handleInputChange}
                        required
                    />
                </div>
                <div>
                    <label>Datum:</label>
                    <input
                        type="date"
                        name="datum"
                        value={formData.datum}
                        onChange={handleInputChange}
                        required
                    />
                </div>
                <div>
                    <label>Status:</label>
                    <input
                        type="text"
                        name="status"
                        value={formData.status}
                        onChange={handleInputChange}
                        required
                    />
                </div>
                <div>
                    <label>Opmerkingen:</label>
                    <textarea
                        name="opmerkingen"
                        value={formData.opmerkingen}
                        onChange={handleInputChange}
                    ></textarea>
                </div>
                <div>
                    <label>Voertuig ID:</label>
                    <input
                        type="text"
                        name="voertuigId"
                        value={formData.voertuigId}
                        onChange={handleInputChange}
                        required
                    />
                </div>
                <button type="submit">Verstuur</button>
            </form>

            {/* De container voor de toastmeldingen */}
            <ToastContainer />
        </div>
    );
};

export default CreateSchadeMelding;
