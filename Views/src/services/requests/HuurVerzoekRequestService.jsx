


const HuurVerzoekRequestService = {

    register: async (data) => {
        try {
            const response = await apiService.post('/Huurverzoek/verzoek', { body: data });
            return response; // Zorg dat dit een geldig object is
            console.log(data);
        } catch (error) {
            console.error("API Error:", error);
            throw error; // Gooi de fout opnieuw als het misgaat
        }
    },

    checkActive: async (huurderId) => {
        try {
            const response = await apiService.get(`/Huurverzoek/check-active/${huurderId}`);
            return response;
        } catch (error) {
            console.error("API Error:", error);
            throw error;
        }
    },

};

export default HuurVerzoekRequestService;