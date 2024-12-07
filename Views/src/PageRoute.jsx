import { BrowserRouter, Routes, Route } from "react-router-dom";
import Layout from "./pages/Layout";
import Register from "./pages/register";
import Login from "./pages/login";
import Home from "./pages/Home";
import Index from "./pages/Index";
import Abonnement from "./pages/abonnement"; // Nieuwe import
import Payment from "./pages/payment"; // Nieuwe import
import Zaakdashboard from "./pages/zaakdashboard"; // Nieuwe import
import wagenparklogin from "./pages/wagenparklogin"; // Nieuwe import
import zakelijkverwijder from "./pages/zakelijkverwijder"; // Nieuwe import
import wagendashboard from "./pages/wagendashboard"; // Nieuwe import

import "./pages/styles.css";

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
                    <Route path="wagenparklogin" element={<wagenparklogin />} /> {/* Nieuwe route */}
                    <Route path="zakelijkverwijder" element={<zakelijkverwijder />} /> {/* Nieuwe route */}
                    <Route path="wagendashboard" element={<wagendashboard />} /> {/* Nieuwe route */}

                </Route>
            </Routes>
        </BrowserRouter>
    );
}

export default PageRoute;
