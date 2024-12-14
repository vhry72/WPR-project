import React, { useState } from 'react';
import { useParams, useNavigate } from 'react-router-dom';

const HuurZakelijkVoertuig = () => {
    const { kenteken } = useParams();
    const navigate = useNavigate();
    const [startDate, setStartDate] = useState('');
    const [endDate, setEndDate] = useState('');

    const handleStartDateChange = (event) => {
        setStartDate(event.target.value);
    };

    const handleEndDateChange = (event) => {
        setEndDate(event.target.value);
    };

    const handleSubmit = (event) => {
        event.preventDefault();
        // Implement logic to handle rental booking
        console.log("Reservering verzonden", { kenteken, startDate, endDate });
        // Redirect or give feedback to the user
        navigate('/bevestiging', { state: { kenteken, startDate, endDate } });
    };

    return (
        <div className="container">
            <h1>Huur een Zakelijk Voertuig</h1>
            <h2>Geselecteerd voertuig: {kenteken}</h2>

            <form onSubmit={handleSubmit} className="rental-form">
                <div className="form-group">
                    <label htmlFor="start-date">Startdatum</label>
                    <input
                        type="date"
                        id="start-date"
                        value={startDate}
                        onChange={handleStartDateChange}
                        required
                    />
                </div>

                <div className="form-group">
                    <label htmlFor="end-date">Einddatum</label>
                    <input
                        type="date"
                        id="end-date"
                        value={endDate}
                        onChange={handleEndDateChange}
                        required
                    />
                </div>

                <button type="submit" className="submit-button">Bevestig Reservering</button>
            </form>
        </div>
    );
};

export default HuurZakelijkVoertuig;
