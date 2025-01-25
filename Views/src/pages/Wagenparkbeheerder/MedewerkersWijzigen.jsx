import React, { useState, useEffect } from 'react';
import axios from 'axios';
import { useNavigate } from 'react-router-dom';
import { toast } from 'react-toastify';
import JwtService from "../../services/JwtService";

const MedewerkerWijzigPagina = () => {
    const [medewerkerGegevens, setMedewerkerGegevens] = useState({ medewerkerNaam: '', medewerkerEmail: '' });
    const [medewerkerId, setMedewerkerId] = useState('');
    const [medewerkers, setMedewerkers] = useState([]); // Changed this to hold full employee objects
    const [isLoading, setIsLoading] = useState(false);
    const navigate = useNavigate();
    const [beheerderId, setBeheerderId] = useState('');

    useEffect(() => {
        const loadBeheerderId = async () => {
            const id = await JwtService.getUserId(); // JWtservice gebruikt voor vinden Id
            setBeheerderId(id);
        };

        loadBeheerderId();
    }, []);

    useEffect(() => {
        const fetchMedewerkers = async () => {
            if (!beheerderId) return;
            try {
                const response = await axios.get(`https://localhost:5033/api/WagenparkBeheerder/${beheerderId}/medewerker-object`);
                const actieveMedewerkers = response.data.filter(medewerker => medewerker.isActive);
                if (actieveMedewerkers.length > 0) {
                    setMedewerkers(actieveMedewerkers);
                } else {
                    toast.error("Geen actieve medewerkers gevonden.");
                }
            } catch (error) {
                toast.error("Fout bij het laden van medewerker IDs.");
            }
        };

        fetchMedewerkers();
    }, [beheerderId]);

    useEffect(() => {
        const fetchMedewerkerDetails = async () => {
            if (!medewerkerId) return;
            setIsLoading(true);
            try {
                const response = await axios.get(`https://localhost:5033/api/BedrijfsMedewerkers/${medewerkerId}/gegevens`);
                if (response.data) {
                    setMedewerkerGegevens({
                        medewerkerNaam: response.data.medewerkerNaam,
                        medewerkerEmail: response.data.medewerkerEmail
                    });
                }
                setIsLoading(false);
            } catch (error) {
                toast.error("Fout bij het laden van medewerker gegevens.");
                setIsLoading(false);
            }
        };

        fetchMedewerkerDetails();
    }, [medewerkerId]);

    const handleSelectMedewerker = (event) => {
        setMedewerkerId(event.target.value);
    };

    const handleChange = (event) => {
        const { name, value } = event.target;
        setMedewerkerGegevens(prev => ({ ...prev, [name]: value }));
    };

    const ToPasswordReset = () => {
        navigate("/WachtwoordResetBeheerder");
    };

    const handleSubmit = async (event) => {
        event.preventDefault();
        setIsLoading(true);

        try {
            console.log(medewerkerId);
            console.log(medewerkerGegevens);
           const response = await axios.put(`https://localhost:5033/api/BedrijfsMedewerkers/${medewerkerId}`, medewerkerGegevens, {
                headers: {
                'Content-Type': 'application/json'
            }
           });
            console.log(response);
            toast.success("De gegevens zijn succesvol aangepast!");
            navigate('/');
        } catch (error) {
            toast.error("Er is een fout opgetreden bij het bijwerken van de gegevens.");
            setIsLoading(false);
        }
    };

    const handleDeleteMedewerker = async () => {
        if (!medewerkerId) {
            toast.error("Selecteer eerst een medewerker om te verwijderen.");
            return;
        }

        const confirmDelete = window.confirm("Weet je zeker dat je deze medewerker wilt verwijderen?");
        if (confirmDelete) {
            setIsLoading(true);
            try {
                const response = await axios.delete(`https://localhost:5033/api/BedrijfsMedewerkers/${medewerkerId}`);
                toast.success("Medewerker is succesvol verwijderd!");
                setMedewerkers(medewerkers.filter(medewerker => medewerker.bedrijfsMedewerkerId !== medewerkerId));
                setMedewerkerGegevens({ medewerkerNaam: '', medewerkerEmail: '' });
                setMedewerkerId('');
                navigate('/');
            } catch (error) {
                toast.error("Er is een fout opgetreden bij het verwijderen van de medewerker.");
            }
            setIsLoading(false);
        }
    };

    return (
        <div className="medewerker-wijzig-container">
            <h1>Wijzig Gegevens van Medewerkers</h1>
            <select onChange={handleSelectMedewerker} value={medewerkerId}>
                <option value="">Selecteer een Medewerker</option>
                {medewerkers.map(medewerker => (
                    <option key={medewerker.bedrijfsMedewerkerId} value={medewerker.bedrijfsMedewerkerId}>
                        {medewerker.medewerkerNaam}
                    </option>
                ))}
            </select>
            <form onSubmit={handleSubmit}>
                <label htmlFor="medewerkerNaam">Medewerker Naam</label>
                <input
                    type="text"
                    id="medewerkerNaam"
                    name="medewerkerNaam"
                    value={medewerkerGegevens.medewerkerNaam}
                    onChange={handleChange}
                    required
                />

                <label htmlFor="medewerkerEmail">Medewerker Email</label>
                <input
                    type="email"
                    id="medewerkerEmail"
                    name="medewerkerEmail"
                    value={medewerkerGegevens.medewerkerEmail}
                    onChange={handleChange}
                    required
                />

                <button type="submit" disabled={isLoading || !medewerkerId}>
                    {isLoading ? 'Updating' : 'Update Gegevens'}
                </button>

                <button onClick={handleDeleteMedewerker} disabled={isLoading || !medewerkerId}>
                    {isLoading ? 'Verwijderen...' : 'Verwijder Medewerker'}
                </button>

                <button onClick={ToPasswordReset}>Reset Medewerker Wachtwoord</button>
            </form>
        </div>
    );
};

export default MedewerkerWijzigPagina; 
