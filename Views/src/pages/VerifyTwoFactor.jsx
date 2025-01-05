import React, { useState } from "react";
import apiService from "../services/apiService";

const VerifyTwoFactor = () => {
    const [code, setCode] = useState("");
    const [message, setMessage] = useState("");

    const handleVerify2FA = async () => {
        try {
            const response = await apiService.post("/api/Account/verify-2fa", { code });
            setMessage("Tweestapsverificatie succesvol geactiveerd!");
        } catch (error) {
            setMessage("Fout: Ongeldige code. Probeer opnieuw.");
        }
    };

    return (
        <div>
            <h2>Verifieer Tweestapsverificatie</h2>
            <input
                type="text"
                value={code}
                onChange={(e) => setCode(e.target.value)}
                placeholder="Voer de code in"
            />
            <button onClick={handleVerify2FA}>Verifieer</button>
            {message && <p>{message}</p>}
        </div>
    );
};

export default VerifyTwoFactor;
