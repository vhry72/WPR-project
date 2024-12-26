//import { Outlet, Link } from "react-router-dom";
//import { useContext } from "react";
//import { UserContext } from "../context/UserContext";
//import "../styles/navigatieBalk.css";

//const Layout = () => {
//    const { userRole } = useContext(UserContext);

//    // Definieer navigatie-opties per rol
//    const navLinks = {
//        particulier: [
//            { to: "/instellingen", label: "Instellingen" },
//            { to: "/particulierHuurderDashBoard", label: "Dashboard" },
//            { to: "/particulierVoertuigTonen", label: "Voertuigen Tonen" },
//        ],
//        zakelijk: [
//            { to: "/instellingen", label: "Instellingen" },
//            { to: "/zaakdashboard", label: "Zaak Dashboard" },
//            { to: "/zakelijkeautotonen", label: "Zakelijke Auto's" },
//        ],
//        medewerker: [
//            { to: "/instellingen", label: "Instellingen" },
//            { to: "/BackOfficeMedewerker", label: "BackOffice Medewerker" },
//            { to: "/SchadeMeldingen", label: "Schade Meldingen" },
//        ],
//        wagenparkbeheerder: [
//            { to: "/instellingen", label: "Instellingen" },
//            { to: "/wagendashboard", label: "Wagen Dashboard" },
//            { to: "/wagenbeheer", label: "Wagen Beheer" },
//        ],
//    };

//    return (
//        <>
//            <header>
//                <nav className="navbar">
//                    {/* Opties die altijd zichtbaar zijn */}
//                    <Link to="/Home" className="nav-link">Home</Link>
//                    <Link to="/" className="nav-link">Aanmelden</Link>

//                    {/* Dynamische opties op basis van de gebruikersrol */}
//                    {userRole &&
//                        navLinks[userRole]?.map((link, index) => (
//                            <Link key={index} to={link.to} className="nav-link">
//                                {link.label}
//                            </Link>
//                        ))}
//                </nav>
//            </header>
//            <main>
//                <Outlet /> {/* Render de child route */}
//            </main>
//        </>
//    );
//};

//export default Layout;
