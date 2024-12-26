import React, { useEffect, useState } from 'react';
import ParticulierHuurdersRequestService from './services/requests/ParticulierHuurdersRequestService';

const HuurdersList = ({ onNavigate }) => {
    const [huurders, setHuurders] = useState([]);
    const [error, setError] = useState(null);

    useEffect(() => {
        const fetchHuurders = async () => {
            try {
                const data = await ParticulierHuurdersRequestService.getAll();
                setHuurders(data);
            } catch (err) {
                setError(err.message);
            }
        };

        fetchHuurders();
    }, []);

    if (error) {
        return <p>Error: {error}</p>;
    }

    return (
        <div>
            <h1>Particuliere Huurders</h1>
            <ul>
                {huurders.map((huurder) => (
                    <li key={huurder.id}>
                        <a href={`#details/${huurder.id}`} onClick={() => onNavigate('details', huurder.id)}>
                            {huurder.name}
                        </a>
                    </li>
                ))}
            </ul>
            <a href="#register" onClick={() => onNavigate('register')}>
                Registreer Nieuwe Huurder
            </a>
        </div>
    );
};

const HuurderDetails = ({ id, onNavigate }) => {
    const [huurder, setHuurder] = useState(null);
    const [error, setError] = useState(null);

    useEffect(() => {
        const fetchHuurder = async () => {
            try {
                const data = await ParticulierHuurdersRequestService.getById(id);
                setHuurder(data);
            } catch (err) {
                setError(err.message);
            }
        };

        fetchHuurder();
    }, [id]);

    if (error) return <p>Error: {error}</p>;
    if (!huurder) return <p>Loading...</p>;

    return (
        <div>
            <h1>Details van {huurder.name}</h1>
            <p>Email: {huurder.email}</p>
            <p>Geregistreerd op: {huurder.registrationDate}</p>
            <a href="#" onClick={() => onNavigate('list')}>Terug naar overzicht</a>
        </div>
    );
};

const RegisterHuurder = ({ onNavigate }) => {
    const [formData, setFormData] = useState({
        name: '',
        email: '',
        password: '',
    });
    const [message, setMessage] = useState('');
    const [error, setError] = useState(null);

    const handleInputChange = (e) => {
        const { name, value } = e.target;
        setFormData({ ...formData, [name]: value });
    };

    const handleSubmit = async (e) => {
        e.preventDefault();
        try {
            const response = await ParticulierHuurdersRequestService.register(formData);
            setMessage(response.message);
        } catch (err) {
            setError(err.message);
        }
    };

    return (
        <div>
            <h1>Registreer Huurder</h1>
            <form onSubmit={handleSubmit}>
                <label>
                    Naam:
                    <input type="text" name="name" value={formData.name} onChange={handleInputChange} />
                </label>
                <br />
                <label>
                    Email:
                    <input type="email" name="email" value={formData.email} onChange={handleInputChange} />
                </label>
                <br />
                <label>
                    Wachtwoord:
                    <input type="password" name="password" value={formData.password} onChange={handleInputChange} />
                </label>
                <br />
                <button type="submit">Registreer</button>
            </form>
            {message && <p>{message}</p>}
            {error && <p style={{ color: 'red' }}>Error: {error}</p>}
            <a href="#" onClick={() => onNavigate('list')}>Terug naar overzicht</a>
        </div>
    );
};

const ParticulierHuurderComponent = () => {
    const [view, setView] = useState('list');
    const [selectedId, setSelectedId] = useState(null);

    const handleNavigate = (newView, id = null) => {
        setView(newView);
        setSelectedId(id);
    };

    return (
        <div>
            {view === 'list' && <HuurdersList onNavigate={handleNavigate} />}
            {view === 'details' && selectedId && <HuurderDetails id={selectedId} onNavigate={handleNavigate} />}
            {view === 'register' && <RegisterHuurder onNavigate={handleNavigate} />}
        </div>
    );
};

export default ParticulierHuurderComponent;
