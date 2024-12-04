import { Link } from "react-router-dom";
import "./styles.css"; // Ensure this path is correct

const Index = () => {
    console.log("Index component wordt gerenderd");

    return (
        <>
            <header>
                <div className="container">
                    <a href="#" className="logo">
                        {/* Add logo image here if needed */}
                    </a>
                    {/*<nav>*/}
                    {/*    <ul>*/}
                    {/*        <li><Link to="/">Home</Link></li>*/}
                    {/*        <li><Link to="/verhuren">Verhuur Opties</Link></li>*/}
                    {/*        <li><Link to="/faq">FAQ</Link></li>*/}
                    {/*        <li><Link to="/contact">Contact</Link></li>*/}
                    {/*    </ul>*/}
                    {/*</nav>*/}
                </div>
            </header>
            <h1 className="H1Tekst-Index">Welkom bij CarAndAll</h1>
            <div className="index-container">
                <div className="options">
                    <Link to="/register" className="btn">
                        Registreren
                    </Link>
                    <Link to="/login" className="btn">
                        Inloggen
                    </Link>
                </div>
            </div>
        </>
    );
};

export default Index;
