import apiService from '../apiService';


const ZakelijkeHuurderRequestService = {
    getAll: async () => {
        try {
            const response = await apiService.get('/api/ZakelijkeHuurders');
            console.log('GET All:', response.data);
        } catch (error) {
            console.error('Error fetching all huurders:', error);
        }
    },

    getById: async (id) => {
        try {
            const response = await apiService.get(`/api/ZakelijkeHuurders/${id}`);
            console.log('GET by ID:', response.data);
        } catch (error) {
            console.error(`Error fetching huurder with ID ${id}:`, error);
        }
    },

    register: async (data) => {
        try {
            console.log({body: data})
            const response = await apiService.post('/ZakelijkeHuurder/registerDTO', { body: data });
            return response; // Zorg dat dit een geldig object is
        } catch (error) {
            console.error("API Error:", error);
            throw error; // Gooi de fout opnieuw als het misgaat
        }
    },


    verifyEmail: async (token) => {
        try {
            const response = await apiService.get(`/api/ZakelijkeHuurders/verify`, {
                params: { token },
            });
            console.log('GET Verify Email:', response.data);
        } catch (error) {
            console.error('Error verifying email:', error);
        }
    },

    update: async (id, updatedHuurder) => {
        try {
            await apiService.put(`/api/ZakelijkeHuurders/${id}`, updatedHuurder);
            console.log('PUT Update: Success');
        } catch (error) {
            console.error(`Error updating huurder with ID ${id}:`, error);
        }
    },

    delete: async (id) => {
        try {
            await apiService.delete(`/api/ZakelijkeHuurders/${id}`);
            console.log('DELETE: Success');
        } catch (error) {
            console.error(`Error deleting huurder with ID ${id}:`, error);
        }
    },

    isEmailVerified: async (id) => {
        try {
            const response = await apiService.get(`/api/ZakelijkeHuurders/${id}/isVerified`);
            console.log('GET Is Email Verified:', response.data);
        } catch (error) {
            console.error(`Error checking email verification for huurder with ID ${id}:`, error);
        }
    },
};

export default ZakelijkeHuurderRequestService