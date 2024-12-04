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
                </nav>
            </header>
            <main>
                <Outlet /> {/* Here the child route is rendered */}
            </main>
        </>
    );
};

export default Layout;
