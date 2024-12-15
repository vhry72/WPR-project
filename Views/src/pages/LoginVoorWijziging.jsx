import React, { useState } from "react";
import { useNavigate } from "react-router-dom";
import "../styles/Login.css";
import particulierHuurdersRequestService from "../services/requests/ParticulierHuurderRequestService";
import zakelijkHuurdersRequestService from "../services/requests/ZakelijkeHuurderRequestService";

const Login = () => {
    const navigate = useNavigate();
    const [activeTab, setActiveTab] = useState("particulier"); // 'particulier' or 'zakelijk'
    const [particulierFormData, setParticulierFormData] = useState({
        particulierEmail: "",
        wachtwoord: "",
    });
    const [zakelijkFormData, setZakelijkFormData] = useState({
        zakelijkEmail: "",
        wachtwoord: "",
    });
    const [isLoading, setIsLoading] = useState(false);
    const [errorMessage, setErrorMessage] = useState("");

    const handleTabChange = (tab) => {
        setActiveTab(tab);
        setErrorMessage(""); // Reset error message when switching tabs
    };

    const handleChange = (event, formType) => {
        const { id, value } = event.target;
        if (formType === "particulier") {
            setParticulierFormData((prev) => ({ ...prev, [id]: value }));
        } else {
            setZakelijkFormData((prev) => ({ ...prev, [id]: value }));
        }
    };

    const handleLogin = async (event) => {
        event.preventDefault();
        setIsLoading(true);
        setErrorMessage("");

        try {
            let payload, response;

            if (activeTab === "particulier") {
                const { particulierEmail, wachtwoord } = particulierFormData;
                if (!particulierEmail || !wachtwoord) {
                    setErrorMessage("Vul alle verplichte velden in!");
                    setIsLoading(false);
                    return;
                }

                payload = {
                    email: particulierEmail,
                    wachtwoord: wachtwoord,
                };

                response = await particulierHuurdersRequestService.login(payload);

                if (response.data.isEmailVerified) {
                    navigate(`/particulierHuurderDashBoard?HuurderID=${response.data.id}`);
                } else {
                    setErrorMessage("Je moet eerst je e-mail bevestigen.");
                }
            } else if (activeTab === "zakelijk") {
                const { zakelijkEmail, wachtwoord } = zakelijkFormData;
                if (!zakelijkEmail || !wachtwoord) {
                    setErrorMessage("Vul alle verplichte velden in!");
                    setIsLoading(false);
                    return;
                }

                payload = {
                    email: zakelijkEmail,
                    wachtwoord: wachtwoord,
                };

                response = await zakelijkHuurdersRequestService.login(payload);

                
                navigate(`/zakelijkDashboard?id=${response.data.id}`);
            }
        } catch (error) {
            setErrorMessage("Login mislukt. Controleer je gegevens en probeer opnieuw.");
        } finally {
            setIsLoading(false);
        }
    };

    return (
        <div className="login-container-wrapper">
            <div className="login-container">
                <h1>Login</h1>

                {/* Tabs for switching between Particulier and Zakelijk login */}
                <div className="login-tabs">
                    <button
                        className={activeTab === "particulier" ? "active" : ""}
                        onClick={() => handleTabChange("particulier")}
                    >
                        Particulier
                    </button>
                    <button
                        className={activeTab === "zakelijk" ? "active" : ""}
                        onClick={() => handleTabChange("zakelijk")}
                    >
                        Zakelijk
                    </button>
                </div>

                {/* Form based on the active tab */}
                <form onSubmit={handleLogin} className="login-form">
                    {activeTab === "particulier" && (
                        <>
                            <div className="form-group">
                                <label htmlFor="particulierEmail">Email</label>
                                <input
                                    type="email"
                                    id="particulierEmail"
                                    value={particulierFormData.particulierEmail}
                                    onChange={(e) => handleChange(e, "particulier")}
                                    required
                                />
                            </div>

                            <div className="form-group">
                                <label htmlFor="wachtwoord">Wachtwoord</label>
                                <input
                                    type="password"
                                    id="wachtwoord"
                                    value={particulierFormData.wachtwoord}
                                    onChange={(e) => handleChange(e, "particulier")}
                                    required
                                />
                            </div>
                        </>
                    )}

                    {activeTab === "zakelijk" && (
                        <>
                            <div className="form-group">
                                <label htmlFor="zakelijkEmail">Email</label>
                                <input
                                    type="email"
                                    id="zakelijkEmail"
                                    value={zakelijkFormData.zakelijkEmail}
                                    onChange={(e) => handleChange(e, "zakelijk")}
                                    required
                                />
                            </div>

                            <div className="form-group">
                                <label htmlFor="wachtwoord">Wachtwoord</label>
                                <input
                                    type="password"
                                    id="wachtwoord"
                                    value={zakelijkFormData.wachtwoord}
                                    onChange={(e) => handleChange(e, "zakelijk")}
                                    required
                                />
                            </div>
                        </>
                    )}

                    {errorMessage && (
                        <p className="error-message">{errorMessage}</p>
                    )}

                    <button
                        type="submit"
                        disabled={isLoading}
                        className={isLoading ? "submit-button loading" : "submit-button"}
                    >
                        {isLoading ? "Bezig met inloggen..." : "Login"}
                    </button>
                </form>
            </div>
        </div>
    );
};

export default Login;
