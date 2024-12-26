import { BrowserRouter, Routes, Route } from "react-router-dom";
import Layout from "./pages/Layout";
import Register from "./pages/register";
import Login from "./pages/login";
import Home from "./pages/Home";
import Index from "./pages/Index";
import Abonnement from "./pages/abonnement";
import Payment from "./pages/payment";
import BedrijfsAbonnement from "./pages/bedrijfsabonnement";
import MedewerkerAbonnementDashboard from "./pages/medewerkerAbonnementDashboard";
import Zaakdashboard from "./pages/zaakdashboard";
import Wagenparklogin from "./pages/wagenparklogin";
import Wagendashboard from "./pages/wagendashboard";
import ZakelijkAutoTonen from "./pages/ZakelijkAutoTonen";
import AccountwijzigingHuurders from "./pages/accountwijzigingHuurders";
import Instellingen from "./pages/instellingen";
import Wagenbeheer from "./pages/wagenbeheer";
import ParticulierVoertuigTonen from "./pages/particulierVoertuigTonen";
import LoginVoorWijziging from "./pages/LoginVoorWijziging";
import TestPage from "./pages/testPage";
import HuurVoertuig from "./pages/huurVoertuig";
import ZakelijkHuurderDashBoard from "./pages/zakelijkHuurderDashboard";
import ParticulierHuurderDashBoard from "./pages/particulierHuurderDashboard";
import BevestigingHuur from "./pages/bevestigingHuur";
import BackOfficeMedewerker from "./pages/BackOfficeMedewerker/BackOfficeMedewerker";
import BackOfficeVerhuurAanvragen from "./pages/BackOfficeMedewerker/BackOfficeVerhuurAanvragen";
import VerhuurGegevens from "./pages/BackOfficeMedewerker/VerhuurGegevens";
import SchadeMeldingen from "./pages/BackOfficeMedewerker/SchadeMeldingen";
import { UserProvider } from "./context/UserContext"; // Context import

function PageRoute() {
    return (
        <UserProvider>
            <BrowserRouter>
                <Routes>
                    <Route path="/" element={<Layout />}>
                        <Route index element={<Index />} />
                        <Route path="Home" element={<Home />} />
                        <Route path="register" element={<Register />} />
                        <Route path="login" element={<Login />} />
                        <Route path="abonnement" element={<Abonnement />} />
                        <Route path="bedrijfsabonnement" element={<BedrijfsAbonnement />} />
                        <Route path="payment" element={<Payment />} />
                        <Route path="zaakdashboard" element={<Zaakdashboard />} />
                        <Route path="medewerkerAbonnementDashboard" element={<MedewerkerAbonnementDashboard />} />
                        <Route path="wagenparklogin" element={<Wagenparklogin />} />
                        <Route path="wagendashboard" element={<Wagendashboard />} />
                        <Route path="zakelijkeautotonen" element={<ZakelijkAutoTonen />} />
                        <Route path="wagenbeheer" element={<Wagenbeheer />} />
                        <Route path="testPage" element={<TestPage />} />
                        <Route path="instellingen" element={<Instellingen />} />
                        <Route path="accountwijzigingHuurders" element={<AccountwijzigingHuurders />} />
                        <Route path="particulierVoertuigTonen" element={<ParticulierVoertuigTonen />} />
                        <Route path="huurVoertuig" element={<HuurVoertuig />} />
                        <Route path="LoginVoorWijziging" element={<LoginVoorWijziging />} />
                        <Route path="zakelijkHuurderDashBoard" element={<ZakelijkHuurderDashBoard />} />
                        <Route path="particulierHuurderDashBoard" element={<ParticulierHuurderDashBoard />} />
                        <Route path="bevestigingHuur" element={<BevestigingHuur />} />
                        <Route path="BackOfficeMedewerker" element={<BackOfficeMedewerker />} />
                        <Route path="BackOfficeVerhuurAanvragen" element={<BackOfficeVerhuurAanvragen />} />
                        <Route path="VerhuurGegevens" element={<VerhuurGegevens />} />
                        <Route path="SchadeMeldingen" element={<SchadeMeldingen />} />
                    </Route>
                </Routes>
            </BrowserRouter>
        </UserProvider>
    );
}

export default PageRoute;
