import { Link } from "react-router-dom";
import "../styles/styles.css";


const instellingen = () => {
    console.log("instellingen component wordt gerenderd");

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

export default instellingen;
