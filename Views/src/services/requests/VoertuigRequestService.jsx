import apiService from '../apiService';

const VoertuigRequestService = {
    // Haal alle voertuigen op
    getAll: async (filterParams = {}) => {
        try {
            const response = await apiService.get('/Voertuig/filter', { params: filterParams });
            console.log('GET All Voertuigen:', response.data);
            return response.data; // Zorg dat het data retourneert
        } catch (error) {
            console.error('Error fetching all voertuigen:', error);
            throw error; // Gooi fout opnieuw voor debugging
        }
    },

    // Haal details op van een specifiek voertuig op basis van ID
    getById: async (id) => {
        try {
            const response = await apiService.get(`/Voertuig/${id}`);
            console.log('GET Voertuig by ID:', response.data);
            return response.data; // Zorg dat het data retourneert
        } catch (error) {
            console.error(`Error fetching voertuig with ID ${id}:`, error);
            throw error; // Gooi fout opnieuw voor debugging
        }
    },

    // Maak een nieuw voertuig aan
    create: async (voertuigData) => {
        try {
            const response = await apiService.post('/Voertuig', { body: voertuigData });
            console.log('POST Create Voertuig:', response.data);
            return response.data;
        } catch (error) {
            console.error('Error creating voertuig:', error);
            throw error;
        }
    },

    // Update een bestaand voertuig
    update: async (id, updatedVoertuigData) => {
        try {
            const response = await apiService.put(`/Voertuig/${id}`, { body: updatedVoertuigData });
            console.log('PUT Update Voertuig:', response.data);
            return response.data;
        } catch (error) {
            console.error(`Error updating voertuig with ID ${id}:`, error);
            throw error;
        }
    },

    // Verwijder een voertuig
    delete: async (id) => {
        try {
            await apiService.delete(`/Voertuig/${id}`);
            console.log('DELETE Voertuig: Success');
        } catch (error) {
            console.error(`Error deleting voertuig with ID ${id}:`, error);
            throw error;
        }
    },
};

export default VoertuigRequestService;
