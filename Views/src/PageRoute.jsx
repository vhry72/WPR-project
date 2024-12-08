import { BrowserRouter, Routes, Route } from "react-router-dom";
import Layout from "./pages/Layout";
import Register from "./pages/register";
import Login from "./pages/login";
import Home from "./pages/Home";
import Index from "./pages/Index";
import Abonnement from "./pages/abonnement"; // Nieuwe import
import Payment from "./pages/payment"; // Nieuwe import
import Zaakdashboard from "./pages/zaakdashboard"; // Nieuwe import
import Wagenparklogin from "./pages/wagenparklogin"; // Nieuwe import
import Wagendashboard from "./pages/wagendashboard"; // Nieuwe import

import Wagenbeheer from "./pages/wagenbeheer"; // Nieuwe import
import Toevoegen from "./pages/toevoegen"; // Nieuwe import
import Verwijderen from "./pages/verwijder"; // Nieuwe import
import "../src/styles/styles.css";
import TestPage from "./pages/testPage";



function PageRoute() {
    return (
        <BrowserRouter>
            <Routes>
                <Route path="/" element={<Layout />}>
                    <Route index element={<Index />} />
                    <Route path="Home" element={<Home />} />
                    <Route path="register" element={<Register />} />
                    <Route path="login" element={<Login />} />
                    <Route path="abonnement" element={<Abonnement />} />
                    <Route path="payment" element={<Payment />} /> {/* Nieuwe route */}
                    <Route path="zaakdashboard" element={<Zaakdashboard />} /> {/* Nieuwe route */}
                    <Route path="wagenparklogin" element={<Wagenparklogin />} /> {/* Nieuwe route */}                
                    <Route path="wagendashboard" element={<Wagendashboard />} /> {/* Nieuwe route */}

                    <Route path="wagenbeheer" element={<Wagenbeheer />} /> {/* Nieuwe route */}
                    <Route path="toevoegen" element={<Toevoegen />} /> {/* Nieuwe route */}
                    <Route path="verwijderen" element={<Verwijderen />} /> {/* Nieuwe route */}
                    <Route path="payment" element={<Payment />} />
                    <Route path="testPage" element={<TestPage />} /> 
                </Route>
            </Routes>
        </BrowserRouter>
    );
}

export default PageRoute;
