

const testRequestService = {
    getAll: async () => {
        try {
            const response = await apiService.get('/test/Test/');
            console.log('GET All:', response.data);
        } catch (error) {
            console.error('Error fetching all items:', error);
        }
    },

    getById: async (id) => {
        try {
            const response = await apiService.get(`/test/Test/${id}`);
            console.log('GET by ID:', response.data);
        } catch (error) {
            console.error(`Error fetching item with ID ${id}:`, error);
        }
    },

    create: async (newItem) => {
        try {
            const response = await apiService.post('/test/Test/', { body: newItem });
            console.log('POST Create:', response.data);
        } catch (error) {
            console.error('Error creating item:', error);
        }
    },

    update: async (id, updatedItem) => {
        try {
            await apiService.put(`/test/Test/${id}`, { body: updatedItem });
            console.log('PUT Update: Success');
        } catch (error) {
            console.error(`Error updating item with ID ${id}:`, error);
        }
    },

    delete: async (id) => {
        try {
            await apiService.delete(`/test/Test/${id}`);
            console.log('DELETE: Success');
        } catch (error) {
            console.error(`Error deleting item with ID ${id}:`, error);
        }
    },
};

export default testRequestService;