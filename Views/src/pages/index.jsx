import { Link } from "react-router-dom";
import "./styles.css"; // Ensure this path is correct

const Index = () => {
    return (
        <div className="index-container">
            <h1>Welkom bij CarAndAll</h1>
            <div className="options">
                <Link to="/register" className="btn">
                    Registreren
                </Link>
                <Link to="/login" className="btn">
                    Inloggen
                </Link>
            </div>
        </div>
    );
};

export default Index;
