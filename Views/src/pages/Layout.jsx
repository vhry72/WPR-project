import { Outlet, Link } from "react-router-dom";

const Layout = () => {

    return (
        <>
            <p>Mijn spelletjes:</p>
            <nav>
                <Link to="/">Home</Link>&nbsp;
                <Link to="/dobbelsteen">Dobbelen</Link>&nbsp;
                <Link to="/lingo">Lingo</Link>
            </nav>
            <hr />
            <div>
                <Outlet />
                <p></p>
            </div>
        </>
    )
};

export default Layout;