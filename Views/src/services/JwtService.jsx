
import axios from "axios";

const JwtService = {
    getUserInfo: async () => {
        try {
            const response = await fetch("https://localhost:5033/api/account/user-info", {
                method: "GET",
                credentials: "include",
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
            return data;
        } catch (error) {
            console.error("Fout bij de API-aanroep voor gebruikersinformatie:", error);
            return null;
        }
    },

    getUserId: async () => {
        const userInfo = await JwtService.getUserInfo();
        console.log(userInfo);
        return userInfo?.userId || null;
    },

    getUserRole: async () => {
        const userInfo = await JwtService.getUserInfo();
        return userInfo?.role || null;
    },

    handleLogout: async (setUserRole, navigate) => {
        try {
            await axios.post(`https://localhost:5033/api/Account/logout`, {}, { withCredentials: true });
            setUserRole(null);
            localStorage.removeItem("role");
            navigate("/");
        } catch (error) {
            console.error("Fout bij uitloggen:", error);
        }
    },
};

export default JwtService;
