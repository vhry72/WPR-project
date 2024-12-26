import { Outlet, Link } from "react-router-dom";
import { useContext } from "react";
import { UserContext } from "../context/UserContext";
import "../styles/navigatieBalk.css";

const Layout = () => {
    const { userRole } = useContext(UserContext);

    // Definieer navigatie-opties per rol
    const navLinks = {
        particulier: [
            { to: "/", label: "Aanmelden" },
            { to: "/Home", label: "Info Pagina" },
            { to: "/instellingen", label: "Instellingen" },
            { to: "/particulierHuurderDashBoard", label: "Dashboard" },
            { to: "/particulierVoertuigTonen", label: "Voertuigen Tonen" },
        ],
        zakelijk: [
            { to: "/", label: "Aanmelden" },
            { to: "/Home", label: "Info Pagina" },
            { to: "/instellingen", label: "Instellingen" },
            { to: "/zaakdashboard", label: "Zaak Dashboard" },
            { to: "/zakelijkeautotonen", label: "Zakelijke Auto's" },
        ],
        medewerker: [
            { to: "/", label: "Aanmelden" },
            { to: "/Home", label: "Info Pagina" },
            { to: "/instellingen", label: "Instellingen" },
            { to: "/BackOfficeMedewerker", label: "BackOffice Medewerker" },
            { to: "/SchadeMeldingen", label: "Schade Meldingen" },
        ],
        wagenparkbeheerder: [
            { to: "/", label: "Aanmelden" },
            { to: "/Home", label: "Info Pagina" },
            { to: "/instellingen", label: "Instellingen" },
            { to: "/wagendashboard", label: "Wagen Dashboard" },
            { to: "/wagenbeheer", label: "Wagen Beheer" },
        ],
    };

    return (
        <>
            <header>
                <nav className="navbar">
                    {navLinks[userRole]?.map((link, index) => (
                        <Link key={index} to={link.to} className="nav-link">
                            {link.label}
                        </Link>
                    ))}
                </nav>
            </header>
            <main>
                <Outlet /> {/* Render de child route */}
            </main>
        </>
    );
};

export default Layout;
