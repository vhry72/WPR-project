import React, { useState, useEffect } from "react";
import { Navigate } from "react-router-dom";
import { toast } from "react-toastify";

const PrivateRoute = ({ allowedRoles, children }) => {
    const [redirectPath, setRedirectPath] = useState(null);
    const userRole = localStorage.getItem("role"); 

    useEffect(() => {
        if (!userRole) {
            
            if (!redirectPath) {
                toast.warning("Je moet inloggen om toegang te krijgen.", { autoClose: 3000 });
                setTimeout(() => setRedirectPath("/login"), 3000); 
            }
        } else if (userRole && !allowedRoles.includes(userRole)) {
            
            if (!redirectPath) {
                toast.error("Je hebt geen toegang tot deze pagina.", { autoClose: 3000 });
                setTimeout(() => setRedirectPath("/Home"), 3000);
            }
        }
    }, [userRole, allowedRoles, redirectPath]);

    // Als een redirect nodig is, navigeer naar de juiste pagina
    if (redirectPath) {
        return <Navigate to={redirectPath} replace />;
    }

    // Controleer of de gebruiker de juiste rol heeft en render de kinderen
    if (userRole && allowedRoles.includes(userRole)) {
        return children;
    }

    // Geef niets weer terwijl de waarschuwing wordt getoond
    return null;
};

export default PrivateRoute;
