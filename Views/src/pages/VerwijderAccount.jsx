import React, { useState, useEffect } from "react";
import axios from "axios";
import { useNavigate } from "react-router-dom";
import JwtService from "../services/JwtService"; // Zorg ervoor dat de service correct is geïmporteerd

const VerwijderAccount = () => {
    const [huurderId, setHuurderId] = useState(null);
    const navigate = useNavigate();

    useEffect(() => {
        const fetchUserId = async () => {
            try {
                const userId = await JwtService.getUserId();
                if (userId ) {
                    setHuurderId(userId);
                    console.log(huurderId);
                    
                } else {
                    console.error("Huurder ID kon niet worden opgehaald via de API.");
                }
            } catch (error) {
                console.error("Fout bij het ophalen van de huurder ID:", error);
            }
        };

        fetchUserId();
    }, []);

    const handleAccountVerwijderen = async () => {
        if (window.confirm("Weet je zeker dat je jouw zakelijke account wilt verwijderen?")) {
            try {
                await axios.delete(`https://localhost:5033//api/ZakelijkeHuurder/${huurderId}`);
                alert("Account succesvol verwijderd.");
                navigate("/");
            } catch (error) {
                console.error("Fout bij het verwijderen van het account:", error);
                alert("Er is een fout opgetreden bij het verwijderen van het account.");
            }
        }
    };

    return (
        <div>
            <h2>Verwijder Zakelijke Account</h2>
            <p>Weet je zeker dat je jouw zakelijke account permanent wilt verwijderen? Deze actie kan niet ongedaan worden gemaakt.</p>
            <button onClick={handleAccountVerwijderen} style={{ color: "#FFFFFF", backgroundColor: "#D32F2F" }}>
                Verwijder Account
            </button>
        </div>
    );
};

export default VerwijderAccount;
