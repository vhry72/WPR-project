import React, { useState } from 'react';
import axios from 'axios';
import { toast } from 'react-toastify';
import { useNavigate } from 'react-router-dom';

const WachtwoordResetBeheerder = () => {
    const [email, setEmail] = useState('');
    const [beheerderEmail, setBeheerderEmail] = useState(''); 
    const [isLoading, setIsLoading] = useState(false);
    const navigate = useNavigate();

    const handleSubmit = async (event) => {
        event.preventDefault();
        setIsLoading(true);

        try {
            const response = await axios.post(
                `https://localhost:5033/api/Account/password-reset-beheerder/${beheerderEmail}`,
                JSON.stringify(email),
                { headers: { 'Content-Type': 'application/json' } }
            );
            console.log(response);
            toast.success("Controleer je email voor de reset link.");
            navigate('/');
        } catch (error) {
            toast.error(error.response?.data || "Er is iets misgegaan.");
        } finally {
            setIsLoading(false);
        }
    };

    return (
        <div className="form-container">
            <form onSubmit={handleSubmit}>
                <h2>Wachtwoord resetten medewerker</h2>
                <input
                    type="email"
                    placeholder="Vul de email van de medewerker in"
                    value={email}
                    onChange={(e) => setEmail(e.target.value)}
                    required
                />

                <input
                    type="email"
                    placeholder="Vul je beheerder e-mailadres in"
                    value={beheerderEmail}
                    onChange={(e) => setBeheerderEmail(e.target.value)}
                    required
                />

                <button type="submit" disabled={isLoading}>
                    {isLoading ? "Versturen..." : "Verstuur Reset Link"}
                </button>
            </form>
        </div>
    );
};

export default WachtwoordResetBeheerder;
