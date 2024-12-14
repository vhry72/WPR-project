import { Link } from "react-router-dom";
import "../styles/styles.css";


const instellingen = () => {
    console.log("instellingen component wordt gerenderd");

    return (
        <>
            <div className="index-container">
                <div className="options">
                    <Link to="/accountwijzigingHuurders" className="btn">
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
