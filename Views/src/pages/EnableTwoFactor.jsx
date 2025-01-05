import React, { useState } from "react";
import QRCode from "qrcode.react";
import apiService from "../services/apiService";

const EnableTwoFactor = () => {
    const [qrCodeUri, setQrCodeUri] = useState("");
    const [loading, setLoading] = useState(false);

    const handleEnable2FA = async () => {
        setLoading(true);
        try {
            const response = await apiService.post("/api/Account/enable-2fa");
            setQrCodeUri(response.data.QrCodeUri); // Ontvang de QR-code URI van de API
        } catch (error) {
            console.error("Fout bij het inschakelen van 2FA:", error);
        } finally {
            setLoading(false);
        }
    };

    return (
        <div>
            <h2>Activeer Tweestapsverificatie</h2>
            <button onClick={handleEnable2FA} disabled={loading}>
                {loading ? "Bezig met laden..." : "Genereer QR-code"}
            </button>
            {qrCodeUri && (
                <div>
                    <p>Scan de onderstaande QR-code in je authenticator-app:</p>
                    <QRCode value={qrCodeUri} size={256} />
                </div>
            )}
        </div>
    );
};

export default EnableTwoFactor;
