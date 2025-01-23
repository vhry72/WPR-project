import React, { useState } from 'react';
import axios from 'axios';
import { toast } from 'react-toastify';
import { useNavigate } from 'react-router-dom';

const ResetTwoFactor = () => {
    const [email, setEmail] = useState('');
    const [password, setPassword] = useState('');
    const [isLoading, setIsLoading] = useState(false);
    const navigate = useNavigate();

    const handleSubmit = async (event) => {
        event.preventDefault();
        setIsLoading(true);
        try {
            const payload = { email, password };
            await axios.post(`https://localhost:5033/api/Account/reset-2fa`, payload);
            toast.success("Tweefactorauthenticatie is gereset.");
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
                <h2>Reset 2FA</h2>
                <input
                    type="email"
                    placeholder="E-mailadres"
                    value={email}
                    onChange={(e) => setEmail(e.target.value)}
                    required
                />
                <input
                    type="password"
                    placeholder="Wachtwoord"
                    value={password}
                    onChange={(e) => setPassword(e.target.value)}
                    required
                />
                <button type="submit" disabled={isLoading}>
                    {isLoading ? "Resetten..." : "Reset 2FA"}
                </button>
            </form>
        </div>
    );
};

export default ResetTwoFactor;
