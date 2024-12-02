import { Outlet, Link } from "react-router-dom";

const Layout = () => {
    return (
        <>
            <header>
                <nav>
                   
                    
                </nav>
            </header>
            <main>
                <Outlet /> {/* This renders the child routes */}
            </main>
        </>
    );
};

export default Layout;
