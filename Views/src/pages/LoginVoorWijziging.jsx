import React, { useState } from "react";
import { useNavigate } from "react-router-dom";
import "../styles/Login.css";
import particulierHuurdersRequestService from "../services/requests/ParticulierHuurderRequestService";

const Login = () => {
    const navigate = useNavigate();
    const [formData, setFormData] = useState({
        particulierEmail: "",
        wachtwoord: "",
    });
    const [isLoading, setIsLoading] = useState(false);
    const [errorMessage, setErrorMessage] = useState("");

    const handleChange = (event) => {
        const { id, value } = event.target;
        setFormData((prev) => ({ ...prev, [id]: value }));
    };

    const handleLogin = async (event) => {
        event.preventDefault();
        setIsLoading(true);
        setErrorMessage("");

        try {
            if (!formData.particulierEmail || !formData.wachtwoord) {
                setErrorMessage("Vul alle verplichte velden in!");
                setIsLoading(false);
                return;
            }

            const payload = {
                email: formData.particulierEmail,
                wachtwoord: formData.wachtwoord,
            };

            const response = await particulierHuurdersRequestService.login(payload);

            if (response.data.isEmailVerified) {
                navigate(`/accountwijzigingHuurders?id=${response.data.id}`);
            } else {
                setErrorMessage("Je moet eerst je e-mail bevestigen.");
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

                <form onSubmit={handleLogin} className="login-form">
                    <div className="form-group">
                        <label htmlFor="particulierEmail">Email</label>
                        <input
                            type="email"
                            id="particulierEmail"
                            value={formData.particulierEmail}
                            onChange={handleChange}
                            required
                        />
                    </div>

                    <div className="form-group">
                        <label htmlFor="wachtwoord">Wachtwoord</label>
                        <input
                            type="password"
                            id="wachtwoord"
                            value={formData.wachtwoord}
                            onChange={handleChange}
                            required
                        />
                    </div>

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
