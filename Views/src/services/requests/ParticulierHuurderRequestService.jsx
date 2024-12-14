import apiService from '../apiService'; 


const ParticulierHuurdersRequestService = {
    getAll: async () => {
        try {
            const response = await apiService.get('/ParticulierHuurders');
            console.log('GET All:', response.data);
        } catch (error) {
            console.error('Error fetching all huurders:', error);
        }
    },

    getById: async (id) => {
        try {
            const response = await apiService.get(`/ParticulierHuurder/${id}`);
            return response; // Response wordt teruggegeven aan de aanroepende functie
        } catch (error) {
            console.error(`Error fetching huurder with ID ${id}:`, error);
            throw error; // Error opnieuw gooien om af te handelen in de frontend
        }
    },

    update: async (id, payload) => {
        try {
            const response = await apiService.put(`/ParticulierHuurder/${id}`, { body: payload });
            console.log(payload)
            return response; // Response wordt teruggegeven voor succescontrole
        } catch (error) {
            console.error(`Error updating huurder with ID ${id}:`, error);
            throw error; // Error opnieuw gooien om af te handelen in de frontend
        }
    },

    login: async (credentials) => {
        try {
            const response = await apiService.post('/ParticulierHuurder/login', { body: credentials });
            return response; // Zorg dat dit een geldig object is
        } catch (error) {
            console.error("API Error:", error);
            throw error; // Gooi de fout opnieuw als het misgaat
        }
    },

    register: async (data) => {
        try {
            const response = await apiService.post('/ParticulierHuurder/register', { body: data });
            return response; // Zorg dat dit een geldig object is
        } catch (error) {
            console.error("API Error:", error);
            throw error; // Gooi de fout opnieuw als het misgaat
        }
    },


    verifyEmail: async (token) => {
        try {
            const response = await apiService.get(`/ParticulierHuurders/verify`, {
                params: { token },
            });
            console.log('GET Verify Email:', response.data);
        } catch (error) {
            console.error('Error verifying email:', error);
        }
    },

    delete: async (id) => {
        try {
            await apiService.delete(`/ParticulierHuurders/${id}`);
            console.log('DELETE: Success');
        } catch (error) {
            console.error(`Error deleting huurder with ID ${id}:`, error);
        }
    },

    isEmailVerified: async (id) => {
        try {
            const response = await apiService.get(`/ParticulierHuurders/${id}/isVerified`);
            console.log('GET Is Email Verified:', response.data);
        } catch (error) {
            console.error(`Error checking email verification for huurder with ID ${id}:`, error);
        }
    },
};

export default ParticulierHuurdersRequestService