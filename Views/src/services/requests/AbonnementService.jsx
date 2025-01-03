import apiService from '../apiService';

const AbonnementService = {
    getAll: async () => {
        try {
            const response = await apiService.get('/Abonnement');
            return response;
        } catch (error) {
            console.error('Error fetching abonnementen:', error);
            throw error;
        }
    },
    getBijnaVerlopen: async () => {
        try {
            const response = await apiService.get('/Abonnement/bijna-verlopen');
            return response
        }
        catch (error) {
            console.error('Error fetching bijna verlopen abonnementen: ', error);
            throw error;
        }
    },
    

    getById: async (zakelijkeId) => {
        try {
            const response = await apiService.get(`/Abonnement/${zakelijkeId}`);
            return response;
        } catch (error) {
            console.error(`Error fetching abonnement with ID ${zakelijkeId}:`, error);
            throw error;
        }
    },

    createBusinessSubscription: async (zakelijkeId, abonnementData) => {
        try {
            const response = await apiService.post(`/Abonnement/${zakelijkeId}/abonnement/maken`, abonnementData);
            return response;
        } catch (error) {
            console.error(`Error creating business subscription for ID ${zakelijkeId}:`, error);
            throw error;
        }
    },

    topUpBalance: async (zakelijkeId, bedrag) => {
        try {
            const response = await apiService.post(`/Abonnement/${zakelijkeId}/saldo/opwaarderen`, { bedrag });
            return response;
        } catch (error) {
            console.error(`Error topping up balance for ID ${zakelijkeId}:`, error);
            throw error;
        }
    },

    processOneTimePayment: async (zakelijkeId, bedrag) => {
        try {
            const response = await apiService.post(`/Abonnement/${zakelijkeId}/eenmalig/betalen`, { bedrag });
            return response;
        } catch (error) {
            console.error(`Error processing one-time payment for ID ${zakelijkeId}:`, error);
            throw error;
        }
    },

    addEmployee: async (zakelijkeId, medewerkerData) => {
        try {
            const response = await apiService.post(`/Abonnement/${zakelijkeId}/medewerker/toevoegen`, medewerkerData);
            return response;
        } catch (error) {
            console.error(`Error adding employee for ID ${zakelijkeId}:`, error);
            throw error;
        }
    },

    changeSubscription: async (beheerderId, abonnementData) => {
        try {
            const response = await apiService.post(`/Abonnement/${beheerderId}/abonnement/wijzig`, abonnementData);
            return response;
        } catch (error) {
            console.error(`Error changing subscription for ID ${beheerderId}:`, error);
            throw error;
        }
    },

    delete: async (id) => {
        try {
            await apiService.delete(`/Abonnement/${id}`);
            console.log('DELETE: Success');
        } catch (error) {
            console.error(`Error deleting abonnement with ID ${id}:`, error);
            throw error;
        }
    }
};

export default AbonnementService;
