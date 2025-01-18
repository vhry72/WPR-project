import apiService from '../apiService'; 

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

    getObject: async (id) => {
        try {
            const response = await apiService.get(`/WagenparkBeheerder/${id}/medewerker-object`);
            return response.data;
        } catch (error) {
            console.error(`Error fetching medewerker with ID ${id}:`, error);
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