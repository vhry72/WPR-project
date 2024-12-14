import { Link } from "react-router-dom";
import "../styles/styles.css";

const Index = () => {
    console.log("Index component wordt gerenderd");

    return (
        <>
            
            
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
