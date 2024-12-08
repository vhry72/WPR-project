import { Outlet, Link } from "react-router-dom";

const Layout = () => {
    return (
        <>
            <header>
                <nav className="navbar">
                    <Link to="/" className="nav-link">Startup</Link>&nbsp;
                    <Link to="/Home" className="nav-link">Info Pagina</Link>&nbsp;
                    <Link to="/register" className="nav-link">Registreren</Link>&nbsp;
                    <Link to="/login" className="nav-link">Inloggen</Link>
                    <Link to="/instellingen" className="nav-link">Instellingen</Link>
                    <Link to="/abonnement" className="nav-link">Abonnement</Link>
                    <Link to="/zaakdashboard" className="nav-link">Zaak Dashboard</Link>
                    <Link to="/wagenparklogin" className="nav-link">Wagenpark Login</Link>
                    <Link to="/wagendashboard" className="nav-link">Wagen Dashboard</Link>

                </nav>
            </header>
            <main>
                <Outlet /> {/* Here the child route is rendered */}
            </main>
        </>
    );
};

export default Layout;
