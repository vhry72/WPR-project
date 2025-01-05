

const medewerkerService = {
    // Haal medewerkers opo
    async getMedewerkers() {
        try {
            const response = await apiService.get("/wagenparkbeheerder");
            return response.data;
        } catch (error) {
            console.error("Fout bij het ophalen van medewerkers:", error);
            throw new Error("Kon medewerkers niet ophalen.");
        }
    },

    // Voeg een nieuwe medewerker toe
    async voegMedewerkerToe(medewerker) {
        try {
            const response = await apiService.post("/wagenparkbeheerder/voegmedewerker", {
                body: medewerker,
            });
            return response.data;
        } catch (error) {
            console.error("Fout bij het toevoegen van medewerker:", error);
            throw new Error("Kon medewerker niet toevoegen.");
        }
    },

    // Verwijder een medewerker
    async verwijderMedewerker(email) {
        try {
            await apiService.delete(`/wagenparkbeheerder/verwijdermedewerker`, {
                body: { email },
            });
        } catch (error) {
            console.error("Fout bij het verwijderen van medewerker:", error);
            throw new Error("Kon medewerker niet verwijderen.");
        }
    },
};

export default medewerkerService;
