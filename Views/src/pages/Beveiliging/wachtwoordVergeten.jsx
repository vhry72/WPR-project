import React, { useState } from 'react';
import axios from 'axios';
import { toast } from 'react-toastify';
import { useNavigate } from 'react-router-dom';

const WachtwoordVergeten = () => {
    const [email, setEmail] = useState('');
    const [isLoading, setIsLoading] = useState(false);
    const navigate = useNavigate();




    const handleSubmit = async (event) => {
        event.preventDefault();
        setIsLoading(true);


        try {
            const response = await axios.post(
                'https://localhost:5033/api/Account/forgot-password',
                JSON.stringify(email),
                { headers: { 'Content-Type': 'application/json' } }
            );
            toast.success("Controleer je email voor de reset link.");
            navigate('/login');
        } catch (error) {
            toast.error(error.response?.data || "Er is iets misgegaan.");
        } finally {
            setIsLoading(false);
        }
    };

    return (
        <div className="form-container">
            <form onSubmit={handleSubmit}>
                <h2>Wachtwoord Vergeten</h2>
                <input
                    type="email"
                    placeholder="Vul je e-mailadres in"
                    value={email}
                    onChange={(e) => setEmail(e.target.value)}
                    required
                />
                <button type="submit" disabled={isLoading}>
                    {isLoading ? "Versturen..." : "Verstuur Reset Link"}
                </button>
            </form>
        </div>
    );
};

export default WachtwoordVergeten;
