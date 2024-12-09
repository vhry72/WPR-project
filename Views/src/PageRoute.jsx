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
import ZakelijkAutoTonen from "./pages/ZakelijkAutoTonen";//nieuwe import
import accountwijzigingHuurders from "./pages/accountwijzigingHuurders"; //nieuwe import
import Instellingen from "./pages/instellingen"; // nieuwe import
import Wagenbeheer from "./pages/wagenbeheer"; // Nieuwe import
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
                    <Route path="payment" element={<Payment />} /> 
                    <Route path="zaakdashboard" element={<Zaakdashboard />} /> 
                    <Route path="wagenparklogin" element={<Wagenparklogin />} />           
                    <Route path="wagendashboard" element={<Wagendashboard />} /> 
                    <Route path="zakelijkeautotonen" element={<ZakelijkAutoTonen />} /> 
                    <Route path="wagenbeheer" element={<Wagenbeheer />} /> 
                    <Route path="payment" element={<Payment />} />
                    <Route path="testPage" element={<TestPage />} /> 
                    <Route path="instellingen" element={<Instellingen />} />
                    <Route path="accountwijzigingHuurders" element={<accountwijzigingHuurders />} />

                </Route>
            </Routes>
        </BrowserRouter>
    );
}

export default PageRoute;
