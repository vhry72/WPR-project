const API_URL = 'https://localhost:5033/api/ParticulierHuurder'; // Pas dit aan naar jouw API-base-URL
import apiService from '../apiService'; 

const handleResponse = async (response) => {
    if (!response.ok) {
        const errorData = await response.json();
        throw new Error(errorData.message || 'Something went wrong');
    }
    return response.json();
};

const ParticulierHuurdersRequestService = {
    getAll: async () => {
        try {
            const response = await fetch(`${API_URL}`);
            return await handleResponse(response);
        } catch (error) {
            console.error('Error fetching all huurders:', error);
            throw error;
        }
    },

    getById: async (id) => {
        try {
            const response = await fetch(`${API_URL}/${id}`);
            return await handleResponse(response);
        } catch (error) {
            console.error(`Error fetching huurder with ID ${id}:`, error);
            throw error;
        }
    },

    update: async (id, payload) => {
        try {
            const response = await fetch(`${API_URL}/${id}`, {
                method: 'PUT',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify(payload),
            });
            return await handleResponse(response);
        } catch (error) {
            console.error(`Error updating huurder with ID ${id}:`, error);
            throw error;
        }
    },

    login: async (credentials) => {
        try {
            const response = await fetch(`${API_URL}/login`, {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify(credentials),
            });
            return await handleResponse(response);
        } catch (error) {
            console.error('Error during login:', error);
            throw error;
        }
    },

    register: async (data) => {
        try {
            const response = await apiService.post('/Account/register-particulier', { body: data });
            return response; // Zorg dat dit een geldig object is
        } catch (error) {
            console.error("API Error:", error);
            throw error; // Gooi de fout opnieuw als het misgaat
        }
    },



    verifyEmail: async (token) => {
        try {
            const response = await fetch(`${API_URL}/verify?token=${encodeURIComponent(token)}`);
            return await handleResponse(response);
        } catch (error) {
            console.error('Error verifying email:', error);
            throw error;
        }
    },

    delete: async (id) => {
        try {
            const response = await fetch(`${API_URL}/${id}`, {
                method: 'DELETE',
            });
            return await handleResponse(response);
        } catch (error) {
            console.error(`Error deleting huurder with ID ${id}:`, error);
            throw error;
        }
    },

    isEmailVerified: async (id) => {
        try {
            const response = await fetch(`${API_URL}/${id}/isVerified`);
            return await handleResponse(response);
        } catch (error) {
            console.error(`Error checking email verification for huurder with ID ${id}:`, error);
            throw error;
        }
    },
};

export default ParticulierHuurdersRequestService;