

const BedrijfsMedewerkerRequestService = {
    getAll: async () => {
        try {
            const response = await apiService.get('/BedrijfsMedewerkers');
            console.log('GET All:', response.data);
            return response;
        } catch (error) {
            console.error('Error fetching all medewerkers:', error);
            throw error;
        }
    },

    getById: async (id) => {
        try {
            const response = await apiService.get(`/BedrijfsMedewerkers/${id}`);
            return response;
        } catch (error) {
            console.error(`Error fetching medewerker with ID ${id}:`, error);
            throw error;
        }
    },

    register: async (data) => {
        try {
            const response = await apiService.post('/BedrijfsMedewerkers/register', { body: data });
            return response;
        } catch (error) {
            console.error('Error registering medewerker:', error);
            throw error;
        }
    },

    login: async (credentials) => {
        try {
            const response = await apiService.post('/BedrijfsMedewerkers/login', { body: credentials });
            return response;
        } catch (error) {
            console.error('Login failed:', error);
            throw error;
        }
    },

    update: async (id, payload) => {
        try {
            const response = await apiService.put(`/BedrijfsMedewerkers/${id}`, { body: payload });
            return response;
        } catch (error) {
            console.error(`Error updating medewerker with ID ${id}:`, error);
            throw error;
        }
    },

    delete: async (id) => {
        try {
            await apiService.delete(`/BedrijfsMedewerkers/${id}`);
            console.log('DELETE: Success');
        } catch (error) {
            console.error(`Error deleting medewerker with ID ${id}:`, error);
            throw error;
        }
    }
};

export default BedrijfsMedewerkerRequestService;
