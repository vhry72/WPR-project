//import { Outlet, Link, useNavigate } from "react-router-dom";
//import { useContext } from "react";
//import { UserContext } from "../context/UserContext";
//import "../styles/navigatieBalk.css";
//import axios from "axios";

//const Layout = () => {
//    const { userRole, setUserRole } = useContext(UserContext);
//    const navigate = useNavigate();


//    const navLinks = {
//        ParticuliereHuurder: [
//            { to: "/instellingen", label: "Instellingen" },
//            { to: "/particulierHuurderDashBoard", label: "Dashboard" },
//            { to: "/particulierVoertuigTonen", label: "Voertuigen Tonen" },
//        ],
//        ZakelijkeHuurder: [
//            { to: "/instellingen", label: "Instellingen" },
//            { to: "/zaakdashboard", label: "Zaak Dashboard" },
//            { to: "/zakelijkeautotonen", label: "Zakelijke Auto's" },
//        ],
//        BedrijfsMedewerker: [
//            { to: "/zakelijkHuurderDashBoard", label: "Dashboard" },
//            { to: "/ZakelijkAutoTonen", label: "Zakelijke Auto's" },
//        ],
//        BackofficeMedewerker: [
//            { to: "/instellingen", label: "Instellingen" },
//            { to: "/BackOfficeVerhuuraanvragen", label: "Huur Verzoeken" },
//            { to: "/SchadeMeldingen", label: "Schademeldingen" },
//            { to: "/VerhuurGegevens", label: "Afgekeurde huurverzoeken"}
//        ],
//        FrontofficeMedewerker: [
//            { to: "/instellingen", label: "Instellingen" },
//            { to: "/FrontOfficeMedewerker", label: "Frontoffice Medewerker" },
//            { to: "/Scadeclaims", label: "Schadeclaims" },
//            { to: "/VoertuigDetails", label: "Voertuigen" },
//            { to: "/VoertuigInenUitname", label: "Voertuigen Verhuur"}

//        ],
//        WagenparkBeheerder: [
//            { to: "/instellingen", label: "Instellingen" },
//            { to: "/wagendashboard", label: "Wagen Dashboard" },
//            { to: "/wagenbeheer", label: "Wagen Beheer" },
//            { to: "/medewerkerAbonnementDashboard", label: "Beheer abbonoment"},
//            { to: "/VerhuurdeVoertuigen", label: "gehuurder voertuigen"}
//        ],
//    };

//    const handleLogout = async () => {
//        try {
//            await axios.post("https://localhost:5033/api/Account/logout", {}, { withCredentials: true });
//            setUserRole(null); // Reset de rol
//            localStorage.removeItem("role"); // Verwijder de rol uit localStorage
//            navigate("/"); // Navigeer naar de inlogpagina
//        } catch (error) {
//            console.error("Fout bij uitloggen:", error);
//        }
//    };

//    return (
//        <>
//            <header>
//                <nav className="navbar">
//                    <Link to="/" className="nav-link">Home</Link>
//                    {!userRole && <Link to="/register" className="nav-link">Aanmelden</Link>}
//                    {!userRole && <Link to="/login" className="nav-link">Inloggen</Link>}
//                    {userRole &&
//                        navLinks[userRole]?.map((link, index) => (
//                            <Link key={index} to={link.to} className="nav-link">
//                                {link.label}
//                            </Link>
//                        ))}
//                    {userRole && (
//                        <button onClick={handleLogout} className="logout-button">
//                            Uitloggen
//                        </button>
//                    )}
//                </nav>
//            </header>
//            <main>
//                <Outlet />
//            </main>
//        </>
//    );
//};

//export default Layout;
