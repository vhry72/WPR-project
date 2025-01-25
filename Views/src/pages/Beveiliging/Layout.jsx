import { Outlet, Link, useNavigate } from "react-router-dom";
import { useContext } from "react";
import { UserContext } from "../../context/UserContext";
import "../../styles/navigatieBalk.css";

import JwtService from "../../services/JwtService";


const Layout = () => {
    const { userRole, setUserRole } = useContext(UserContext)
    const navigate = useNavigate();

 
    const navLinks = {
        ParticuliereHuurder: [
            { to: "/accountWijzigingParticulier", label: "Wijzig Gegevens"},
            { to: "/huurVoertuig", label: "Voertuig huren" },
            { to: "/HuurverzoekInzien", label: "Huurverzoek inzien" }
        ],
        ZakelijkeHuurder: [
            { to: "WagenparkBeheerderForm", label: "Voeg een Wagenparkmedewerker toe" },
            { to: "/wagenparkWijzigPagina", label: "Wijzig Wagenparkbeheerder" },
            { to: "/accountwijzigingZakelijk", label: "Wijzig gegevens"}
        ],
        BedrijfsMedewerker: [
            { to: "/zakelijkHuurderDashBoard", label: "Dashboard" },
            { to: "/huurVoertuig", label: "Auto's huren" },
            { to: "/HuurverzoekInzien", label: "Huurverzoek inzien" }
        ],
        BackofficeMedewerker: [
            { to: "/BackOfficeVerhuuraanvragen", label: "Huur Verzoeken" },
            { to: "/SchadeMeldingen", label: "Schademeldingen" },
            { to: "/VerhuurGegevens", label: "Afgekeurde huurverzoeken" },
            { to: "/Schadeclaims", label: "Schade claims" },
            { to: "/VoertuigTonen", label: "Wagenpark beheren" },
            { to: "/PrivacyVerklaringWijziging", label: "privacyverklaring Wijzigen" },
            { to: "/FrontofficeTonen", label: "Frontoffice Beheer" },
            { to: "/accountWijzigingBackoffice", label: "Wijzig Gegevens"}

        ],
        FrontofficeMedewerker: [
            { to: "/Schadeclaims", label: "Schadeclaims" },
            { to: "/VoertuigDetails", label: "Voertuig Gegevens" },
            { to: "/VoertuigInenUitname", label: "Voertuigen Verhuur"}

        ],
        WagenparkBeheerder: [
            { to: "/wagendashboard", label: "Wagen Dashboard" },
            { to: "/wagenbeheer", label: "Medewerkersbeheer" },
            { to: "/abonnement", label: "Kies Abonnement" },
            { to: "/RegisterBedrijfsmedewerker", label: "Voeg medewerker toe" },
            { to: "/MedewerkersWijzigen", label: "Wijzig Medewerkers"},
            { to: "/medewerkerAbonnementDashboard", label: "Beheer Abonnement"},
            { to: "/VerhuurdeVoertuigen", label: "gehuurde voertuigen"}
        ],
    };

    const onLogoutClick = () => {
        JwtService.handleLogout(setUserRole, navigate);
    };


    return (
        <>
            <header>
                <nav className="navbar">
                    <Link to="/" className="nav-link">Home</Link>
                    {!userRole && <Link to="/register" className="nav-link">Aanmelden</Link>}
                    {!userRole && <Link to="/login" className="nav-link">Inloggen</Link>}
                    {!userRole  && <Link to="/Privacyverklaring" className="nav-link">Privacyverklaring</Link>}
                    { userRole && <Link to="/Privacyverklaring" className="nav-link">Privacyverklaring</Link>}
                    {userRole &&
                        navLinks[userRole]?.map((link, index) => (
                            <Link key={index} to={link.to} className="nav-link">
                                {link.label}
                            </Link>
                        ))}
                    {userRole && (
                        <button onClick={onLogoutClick} className="logout-button">
                            Uitloggen
                        </button>
                    )}
                </nav>
            </header>
            <main>
                <Outlet />
            </main>
        </>
    );
};

export default Layout;
