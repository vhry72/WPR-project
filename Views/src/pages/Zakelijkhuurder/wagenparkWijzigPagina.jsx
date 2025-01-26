import React, { useState, useEffect } from 'react';
import axios from 'axios';
import { useNavigate } from 'react-router-dom';
import { toast } from 'react-toastify';
import JwtService from "../../services/JwtService";

const WagenparkWijzigPagina = () => {
    const [medewerkerGegevens, setMedewerkerGegevens] = useState({ beheerderNaam: '', bedrijfsEmail: '' });
    const [medewerkerId, setMedewerkerId] = useState('');
    const [medewerkers, setMedewerkers] = useState([]); // Changed this to hold full employee objects
    const [isLoading, setIsLoading] = useState(false);
    const navigate = useNavigate();
    const [beheerderId, setBeheerderId] = useState('');
    const [error, setError] = useState('');
    const toastIdGeenActieve = "geen-actieve-toast";
    const toastIdError = "error-toast";

    useEffect(() => {
        const loadBeheerderId = async () => {
            const id = await JwtService.getUserId(); 
            console.log("Loaded ID:", id);
            setBeheerderId(id);
        };

        loadBeheerderId();
    }, []);

    useEffect(() => {
        const fetchMedewerkers = async () => {

            if (!beheerderId) {
                console.log("beheerderId is undefined or not set yet.");
                return;
            }

            try {
                const response = await axios.get(
                    `https://localhost:5033/api/ZakelijkeHuurder/${beheerderId}/WagenparkGegevens`, { withCredentials: true });


                const actieveMedewerkers = response.data.filter(
                    (medewerker) => medewerker.isActive
                );

                if (actieveMedewerkers.length > 0) {
                    setMedewerkers(actieveMedewerkers);
                } else {
                    if (!toast.isActive(toastIdGeenActieve)) {
                        toast.error("Geen actieve medewerkers gevonden.", {
                            toastId: toastIdGeenActieve,
                        });
                    }
                }
            } catch (error) {
                if (!toast.isActive(toastIdError)) {
                    toast.error("Fout bij het laden van medewerker gegevens.", {
                        toastId: toastIdError,
                    });
                }
            }
        };

        fetchMedewerkers();
    }, [beheerderId]); 


    useEffect(() => {
        const fetchMedewerkerDetails = async () => {
            if (!medewerkerId) return;
            setIsLoading(true);
            try {
                const response = await axios.get(`https://localhost:5033/api/WagenparkBeheerder/${medewerkerId}/gegevens`, { withCredentials: true });
                if (response.data) {
                    setMedewerkerGegevens({
                        beheerderNaam: response.data.beheerderNaam,
                        bedrijfsEmail: response.data.bedrijfsEmail
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

    const handleDeleteMedewerker = async () => {
        if (!medewerkerId) {
            toast.error("Selecteer eerst een medewerker om te verwijderen.");
            return;
        }

        const confirmDelete = window.confirm("Weet je zeker dat je deze medewerker wilt verwijderen?");
        if (confirmDelete) {
            setIsLoading(true);
            try {
                const response = await axios.delete(`https://localhost:5033/api/WagenparkBeheerder/${medewerkerId}`, { withCredentials: true });
                toast.success("Medewerker is succesvol verwijderd!");
                setMedewerkers(medewerkers.filter(medewerker => medewerker.beheerderId !== medewerkerId));
                setMedewerkerGegevens({ beheerderNaam: '', bedrijfsEmail: '' });
                setMedewerkerId('');
                navigate('/');
            } catch (error) {
                toast.error("Er is een fout opgetreden bij het verwijderen van de medewerker.");
                setError(error.message);
                
            }
            setIsLoading(false);
        }
    };

    const handleSubmit = async (event) => {
        event.preventDefault();
        setIsLoading(true);

        try {
            console.log(medewerkerId);
            console.log(medewerkerGegevens);
            const response = await axios.put(`https://localhost:5033/api/WagenparkBeheerder/${medewerkerId}/updateGegevens`, medewerkerGegevens, {withCredentials: true ,
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

    return (
        <div className="medewerker-wijzig-container">
            <h1>Wijzig Gegevens van Medewerkers</h1>
            <select onChange={handleSelectMedewerker} value={medewerkerId}>
                <option value="">Selecteer een Medewerker</option>
                {medewerkers.map(medewerker => (
                    <option key={medewerker.beheerderId} value={medewerker.beheerderId}>
                        {medewerker.beheerderNaam}
                    </option>
                ))}
            </select>
            <form onSubmit={handleSubmit}>
                <label htmlFor="beheerderNaam">Medewerker Naam</label>
                <input
                    type="text"
                    id="beheerderNaam"
                    name="beheerderNaam"
                    value={medewerkerGegevens.beheerderNaam}
                    onChange={handleChange}
                    required
                />

                <label htmlFor="bedrijfsEmail">Medewerker Email</label>
                <input
                    type="email"
                    id="bedrijfsEmail"
                    name="bedrijfsEmail"
                    value={medewerkerGegevens.bedrijfsEmail}
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

                {error && <p className="error-message">{error}</p>}
            </form>
        </div>
    );
};

export default WagenparkWijzigPagina; 
