import React, { useState } from 'react';
import axios from 'axios';
import { toast } from 'react-toastify';
import { useNavigate, useSearchParams } from 'react-router-dom';

const WachtwoordReset = () => {
    const [password, setPassword] = useState('');
    const [confirmPassword, setConfirmPassword] = useState('');
    const [isLoading, setIsLoading] = useState(false);
    const navigate = useNavigate();
    const [searchParams, setSearchParams] = useSearchParams();
    const userId = searchParams.get('userId');
    const code = searchParams.get('code');

    const handleSubmit = async (event) => {
        event.preventDefault();
        if (password !== confirmPassword) {
            toast.error("Wachtwoorden komen niet overeen");
            return;
        }
        setIsLoading(true);
        try {
            const payload = {
                userId: userId, 
                token: code,     
                password: password 
            };

            const response = await axios.post(
                'https://localhost:5033/api/Account/reset-password',
                JSON.stringify(payload),  // De data om te versturen als een JSON string
                {
                    headers: {
                        'Content-Type': 'application/json'  // Stel de header in om aan te geven dat je JSON verstuurt
                    }
                }
            );
            toast.success("Wachtwoord is gereset.");
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
                <h2>Reset Wachtwoord</h2>
                <input
                    type="password"
                    placeholder="Nieuw Wachtwoord"
                    value={password}
                    onChange={(e) => setPassword(e.target.value)}
                    required
                />
                <input
                    type="password"
                    placeholder="Bevestig Wachtwoord"
                    value={confirmPassword}
                    onChange={(e) => setConfirmPassword(e.target.value)}
                    required
                />
                <button type="submit" disabled={isLoading}>
                    {isLoading ? "Resetten..." : "Reset Wachtwoord"}
                </button>
            </form>
        </div>
    );
};

export default WachtwoordReset;
