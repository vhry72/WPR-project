import { Link } from "react-router-dom";
import "./styles.css"; // Zorg ervoor dat dit pad correct is

const PartDashboard = () => {
    console.log("PartDashboard component wordt gerenderd");

    return (
        <>
            <header>
                <h1 className="H1Tekst-Index"></h1>
            </header>
            <div className="index-container">
                <div className="options">
                    <Link to="/accountbeheer" className="btn">
                        Beheer Account
                    </Link>
                    <Link to="/verwijder" className="btn">
                        Verwijder account
                    </Link>
                </div>
            </div>
        </>
    );
};

export default PartDashboard;
