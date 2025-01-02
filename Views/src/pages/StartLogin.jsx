import { Link } from "react-router-dom";
import "../styles/navigatieBalk.css";
import "../styles/styles.css";

const StartLogin = () => {
    console.log("Index component wordt gerenderd");

    return (
        <>
            <div className="index-container">
                <div className="options">
                    <Link to="/register" className="btn">
                        Registreren
                    </Link>
                </div>
            </div>
        </>
    );
};

export default StartLogin;
