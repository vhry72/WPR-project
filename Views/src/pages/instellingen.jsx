import { Link } from "react-router-dom";
import "../styles/styles.css";


const instellingen = () => {
    console.log("instellingen component wordt gerenderd");

    return (
            <div className="index-container">
                <div className="options">
                <Link to="/accountwijzigingHuurders" className="btn" style={{ color: '#FFFFFF', backgroundColor: '#0053ba' }}>
                    Beheer Account
                </Link>
                <Link to="/verwijder" className="btn" style={{ color: '#FFFFFF', backgroundColor: '#0053ba' }}>
                    Verwijder account
                </Link>
                </div>
            </div>
    );
};

export default instellingen;
