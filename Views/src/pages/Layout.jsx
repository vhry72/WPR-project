import { Outlet, Link } from "react-router-dom";

const Layout = () => {
    return (
        <>
            <header>
                <nav>
                    <Link to="/">Home</Link>&nbsp;
                    <Link to="/register">Registreren</Link>&nbsp;
                    <Link to="/login">Inloggen</Link>
                </nav>
            </header>
            <main>
                <Outlet /> {/* This renders the child routes */}
            </main>
        </>
    );
};

export default Layout;
