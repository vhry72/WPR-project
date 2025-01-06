

const JwtService = {
    getUserInfo: async () => {
        try {
            const response = await fetch("https://localhost:5033/api/account/user-info", {
                method: "GET",
                credentials: "include", // Zorg ervoor dat cookies worden meegezonden
                headers: {
                    "Content-Type": "application/json",
                },
            });

            if (!response.ok) {
                console.error(
                    `Fout bij het ophalen van gebruikersinformatie: ${response.statusText}`
                );
                return null;
            }

            console.log(response.data);
            const data = await response.json();
            return data; // Retourneert een object met `UserId` en `Role`
        } catch (error) {
            console.error("Fout bij de API-aanroep voor gebruikersinformatie:", error);
            return null;
        }
    },

    // Haal alleen de gebruikers-ID op via de API
    getUserId: async () => {
        const userInfo = await JwtService.getUserInfo();
        console.log(userInfo);
        return userInfo?.userId || null;
    },

    // Haal alleen de gebruikersrol op via de API
    getUserRole: async () => {
        const userInfo = await JwtService.getUserInfo();
        return userInfo?.Role || null;
    },
};

export default JwtService;
