import apiService from '../apiService'; 


const ParticulierHuurdersRequestService = {
    getAll: async () => {
        try {
            const response = await apiService.get('/api/ParticulierHuurders');
            console.log('GET All:', response.data);
        } catch (error) {
            console.error('Error fetching all huurders:', error);
        }
    },

    getById: async (id) => {
        try {
            const response = await apiService.get(`/api/ParticulierHuurders/${id}`);
            console.log('GET by ID:', response.data);
        } catch (error) {
            console.error(`Error fetching huurder with ID ${id}:`, error);
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
            const response = await apiService.get(`/api/ParticulierHuurders/verify`, {
                params: { token },
            });
            console.log('GET Verify Email:', response.data);
        } catch (error) {
            console.error('Error verifying email:', error);
        }
    },

    update: async (id, updatedHuurder) => {
        try {
            await apiService.put(`/api/ParticulierHuurders/${id}`, updatedHuurder);
            console.log('PUT Update: Success');
        } catch (error) {
            console.error(`Error updating huurder with ID ${id}:`, error);
        }
    },

    delete: async (id) => {
        try {
            await apiService.delete(`/api/ParticulierHuurders/${id}`);
            console.log('DELETE: Success');
        } catch (error) {
            console.error(`Error deleting huurder with ID ${id}:`, error);
        }
    },

    isEmailVerified: async (id) => {
        try {
            const response = await apiService.get(`/api/ParticulierHuurders/${id}/isVerified`);
            console.log('GET Is Email Verified:', response.data);
        } catch (error) {
            console.error(`Error checking email verification for huurder with ID ${id}:`, error);
        }
    },
};

export default ParticulierHuurdersRequestService