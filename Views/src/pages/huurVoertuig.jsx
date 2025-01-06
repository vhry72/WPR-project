import React, { useState } from 'react';
import { useNavigate, useLocation } from 'react-router-dom';

const HuurVoertuig = () => {
    const navigate = useNavigate();
    const location = useLocation();

    const [startDateTime, setStartDateTime] = useState('');
    const [endDateTime, setEndDateTime] = useState('');
    const [errorMessage, setErrorMessage] = useState('');
 

    // URL-parameters ophalen
    const kenteken = new URLSearchParams(location.search).get("kenteken");
    const VoertuigId = new URLSearchParams(location.search).get("VoertuigID");
    const soortHuurder = new URLSearchParams(location.search).get("SoortHuurder");

    // Bereken "nu + 2 uur"
    const now = new Date();
    const twoHoursLater = new Date(now.getTime() + 2 * 60 * 60 * 1000);
    const minSelectableTime = twoHoursLater.toISOString().slice(0, 16);

    // Controleer verboden tijden (19:00 tot 10:00)
    const isTimeValid = (dateTime) => {
        const time = new Date(dateTime).getHours();
        return time >= 10 && time < 19; // Alleen tijden tussen 10:00 en 19:00 zijn toegestaan
    };

    const handleStartDateTimeChange = (event) => {
        const value = event.target.value;
        setStartDateTime(value);

        // Controleer de geselecteerde tijd
        if (!isTimeValid(value)) {
            setErrorMessage('Starttijd moet tussen 10:00 en 19:00 liggen.');
        } else {
            setErrorMessage('');
        }

        setEndDateTime(''); // Reset eindtijd bij wijziging van starttijd
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

        console.log("Reservering verzonden", { kenteken, startDateTime, endDateTime });
        navigate(`/bevestigingHuur?kenteken=${kenteken}&startdatum=${startDateTime}&einddatum=${endDateTime}&VoertuigID=${VoertuigId}&SoortHuurder=${soortHuurder}`);
    };

    return (
        <div className="container">
            <h1>Huur een Voertuig</h1>
            <h2>Geselecteerd voertuig: {kenteken}</h2>

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
                    disabled={!!errorMessage} // Knop uitschakelen bij fout
                >
                    Bevestig Reservering
                </button>
            </form>
        </div>
    );
};

export default HuurVoertuig;
