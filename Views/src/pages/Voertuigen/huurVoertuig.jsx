import React, { useState, useEffect } from 'react';
import { useNavigate, useLocation } from 'react-router-dom';
import JwtService from "../../services/JwtService";
import { toast } from 'react-toastify';

const HuurVoertuig = () => {
    const navigate = useNavigate();
    const location = useLocation();
    const [startDateTime, setStartDateTime] = useState('');
    const [endDateTime, setEndDateTime] = useState('');
    const [errorMessage, setErrorMessage] = useState('');
    const [userRole, setUserRole] = useState('');
    const [loading, setLoading] = useState(true);

    useEffect(() => {
        const executeSteps = async () => {
            try {
                const role = await JwtService.getUserRole();
                const userId = await JwtService.getUserId();
                if (!role) {
                    setErrorMessage("Geen Huurder rol gevonden.");
                    setLoading(false);
                    return;
                }
                setUserRole(role);

                if (role === 'Bedrijfsmedewerker') {
                    const response = await fetch(`https://localhost:5033/api/BedrijfsMedewerkers/${userId}`, { withCredentials: true });
                    const data = await response.json();
                    console.log(response);

                    if (!response.ok || !data.abonnementId) {
                        toast.error("U maakt geen deel uit van een bedrijfsabonnement. Neem contact op met uw wagenparkbeheerder.");
                        navigate('/');
                        return;
                    }
                }
            } catch (error) {
                console.error("Er is een fout opgetreden:", error);
                setErrorMessage("Er is een fout opgetreden tijdens het laden van de gegevens.");
            } finally {
                setLoading(false);
            }
        };

        executeSteps();
    }, []);

    const now = new Date();
    const twoHoursLater = new Date(now.getTime() + 2 * 60 * 60 * 1000);
    const minSelectableTime = twoHoursLater.toISOString().slice(0, 16);

    const isTimeValid = (dateTime) => {
        const time = new Date(dateTime).getHours();
        return time >= 10 && time < 19;
    };

    const handleStartDateTimeChange = (event) => {
        const value = event.target.value;
        setStartDateTime(value);

        if (!isTimeValid(value)) {
            setErrorMessage('Starttijd moet tussen 10:00 en 19:00 liggen.');
        } else {
            setErrorMessage('');
        }
        setEndDateTime('');
    };

    const handleEndDateTimeChange = (event) => {
        const value = event.target.value;
        setEndDateTime(value);

        if (!isTimeValid(value)) {
            setErrorMessage('Eindtijd moet tussen 10:00 en 19:00 liggen.');
        } else {
            setErrorMessage('');
        }
    };

    const handleSubmit = (event) => {
        event.preventDefault();

        if (!isTimeValid(startDateTime) || !isTimeValid(endDateTime)) {
            setErrorMessage('Tijd moet tussen 10:00 en 19:00 liggen.');
            return;
        }

        const reservationData = {
            startDateTime,
            endDateTime,
            userRole
        };

        if (userRole === "ParticuliereHuurder") {
            navigate('/particulierVoertuigTonen', { state: reservationData });
        } else if (userRole === "Bedrijfsmedewerker") {
            navigate('/ZakelijkAutoTonen', { state: reservationData });
        }
    };

    return (
        <div className="container">
            <h1>Huur een Voertuig</h1>

            <form onSubmit={handleSubmit} className="rental-form">
                <div className="form-group">
                    <label htmlFor="start-datetime">Startdatum en tijd</label>
                    <input
                        type="datetime-local"
                        id="start-datetime"
                        value={startDateTime}
                        onChange={handleStartDateTimeChange}
                        required
                        min={minSelectableTime}
                    />
                </div>

                <div className="form-group">
                    <label htmlFor="end-datetime">Einddatum en tijd</label>
                    <input
                        type="datetime-local"
                        id="end-datetime"
                        value={endDateTime}
                        onChange={handleEndDateTimeChange}
                        required
                        min={startDateTime || minSelectableTime}
                    />
                </div>

                {errorMessage && <p className="error-message" style={{ color: 'red' }}>{errorMessage}</p>}

                <button
                    type="submit"
                    className="submit-button"
                    
                >
                    Bevestig datum
                </button>
            </form>
        </div>
    );
};

export default HuurVoertuig;
