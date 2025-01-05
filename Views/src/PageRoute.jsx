import { BrowserRouter, Routes, Route } from "react-router-dom";
/*import Layout from "./pages/LayoutTest";*/
import Layout from "./pages/Layout";
import Register from "./pages/register";
import Login from "./pages/login";
import Index from "./pages/Index";
import StartLogin from "./pages/StartLogin";
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
import Notification from "./pages/NotificatieZakelijk";
import Schadeclaims from "./pages/BackOfficeMedewerker/Schadeclaims";
import FrontOfficeMedewerker from "./pages/FrontOfficeMedewerker/FrontOfficeMedewerker";
import VoertuigDetails from "./pages/FrontOfficeMedewerker/VoertuigDetails";
import VoertuigInEnUitname from "./pages/FrontOfficeMedewerker/VoertuigInenUitname";
import { UserProvider } from "./context/UserContext"; // Context import
import EmailConfirmation from "./pages/EmailConformation";
import PrivateRoute from "./pages/PrivateRoute";
import VerhuurdeVoertuigen from "./pages/VerhuurdeVoertuigen";


function PageRoute() {
    return (
        <UserProvider>
            <BrowserRouter>
                <Routes>
                    <Route path="/" element={<Layout />}>
                        {/* Openbare routes */}
                        <Route index element={<Index />} />
                        <Route path="startLogin" element={<StartLogin />} />
                        <Route path="register" element={<Register />} />
                        <Route path="email-confirmation" element={<EmailConfirmation />} />
                        <Route path="VerhuurdeVoertuigen" element={<VerhuurdeVoertuigen/> } />
                        <Route path="login" element={<LoginVoorWijziging />} />

                        {/* Routes specifiek voor 'particulier' */}
                        <Route
                            path="particulierHuurderDashBoard"
                            element={
                                <PrivateRoute allowedRoles={["ParticuliereHuurder"]}>
                                    <ParticulierHuurderDashBoard />
                                </PrivateRoute>
                            }
                        />
                        <Route
                            path="particulierVoertuigTonen"
                            element={
                                <PrivateRoute allowedRoles={["ParticuliereHuurder"]}>
                                    <ParticulierVoertuigTonen />
                                </PrivateRoute>
                            }
                        />
                        <Route
                            path="bevestigingHuur"
                            element={
                                <PrivateRoute allowedRoles={["ParticuliereHuurder"]}>
                                    <BevestigingHuur />
                                </PrivateRoute>
                            }
                        />

                        <Route
                            path="huurVoertuig"
                            element={
                                <PrivateRoute allowedRoles={["ParticuliereHuurder"]}>
                                    <HuurVoertuig />
                                </PrivateRoute>
                            }
                        />

                        <Route
                            path="accountwijzigingHuurders"
                            element={
                                <PrivateRoute allowedRoles={["ParticuliereHuurder"]}>
                                    <AccountwijzigingHuurders />
                                </PrivateRoute>
                            }
                        />

                        {/* Routes specifiek voor 'zakelijk' */}
                        <Route
                            path="zakelijkHuurderDashBoard"
                            element={
                                <PrivateRoute allowedRoles={["ZakelijkeHuurder"]}>
                                    <ZakelijkHuurderDashBoard />
                                </PrivateRoute>
                            }
                        />
                        <Route
                            path="zaakdashboard"
                            element={
                                <PrivateRoute allowedRoles={["ZakelijkeHuurder"]}>
                                    <Zaakdashboard />
                                </PrivateRoute>
                            }
                        />

                        <Route
                            path="abonnement"
                            element={
                                <PrivateRoute allowedRoles={["ZakelijkeHuurder"]}>
                                    <Abonnement />
                                </PrivateRoute>
                            }
                        />
                        <Route
                            path="bedrijfsabonnement"
                            element={
                                <PrivateRoute allowedRoles={["ZakelijkeHuurder"]}>
                                    <BedrijfsAbonnement />
                                </PrivateRoute>
                            }
                        />
                        <Route
                            path="payment"
                            element={
                                <PrivateRoute allowedRoles={["ZakelijkeHuurder"]}>
                                    <Payment />
                                </PrivateRoute>
                            }
                        />

                        {/* Routes specifiek voor 'bedrijfsmedewerker' */}

                        <Route
                            path="zakelijkeautotonen"
                            element={
                                <PrivateRoute allowedRoles={["Bedrijfsmedewerker"]}>
                                    <ZakelijkAutoTonen />
                                </PrivateRoute>
                            }
                        />

                        <Route
                            path="huurVoertuig"
                            element={
                                <PrivateRoute allowedRoles={["Bedrijfsmedewerker"]}>
                                    <HuurVoertuig />
                                </PrivateRoute>
                            }
                        />

                        <Route
                            path="accountwijzigingHuurders"
                            element={
                                <PrivateRoute allowedRoles={["Bedrijfsmedewerker"]}>
                                    <AccountwijzigingHuurders />
                                </PrivateRoute>
                            }
                        />


                        {/* Routes specifiek voor 'medewerker' */}
                        <Route
                            path="BackOfficeMedewerker"
                            element={
                                <PrivateRoute allowedRoles={["medewerker"]}>
                                    <BackOfficeMedewerker />
                                </PrivateRoute>
                            }
                        />
                        <Route
                            path="SchadeMeldingen"
                            element={
                                <PrivateRoute allowedRoles={["medewerker"]}>
                                    <SchadeMeldingen />
                                </PrivateRoute>
                            }
                        />
                        <Route
                            path="BackOfficeVerhuurAanvragen"
                            element={
                                <PrivateRoute allowedRoles={["medewerker"]}>
                                    <BackOfficeVerhuurAanvragen />
                                </PrivateRoute>
                            }
                        />
                        <Route
                            path="VerhuurGegevens"
                            element={
                                <PrivateRoute allowedRoles={["medewerker"]}>
                                    <VerhuurGegevens />
                                </PrivateRoute>
                            }
                        />

                        {/* Routes specifiek voor 'wagenparkbeheerder' */}
                        <Route
                            path="wagendashboard"
                            element={
                                <PrivateRoute allowedRoles={["Wagenparkbeheerder"]}>
                                    <Wagendashboard />
                                </PrivateRoute>
                            }
                        />
                        <Route
                            path="wagenbeheer"
                            element={
                                <PrivateRoute allowedRoles={["Wagenparkbeheerder"]}>
                                    <Wagenbeheer />
                                </PrivateRoute>
                            }
                        />
                        <Route
                            path="abonnement"
                            element={
                                <PrivateRoute allowedRoles={["Wagenparkbeheerder"]}>
                                    <Abonnement />
                                </PrivateRoute>
                            }
                        />
                        <Route
                            path="bedrijfsabonnement"
                            element={
                                <PrivateRoute allowedRoles={["Wagenparkbeheerder"]}>
                                    <BedrijfsAbonnement />
                                </PrivateRoute>
                            }
                        />
                        <Route
                            path="payment"
                            element={
                                <PrivateRoute allowedRoles={["Wagenparkbeheerder"]}>
                                    <Payment />
                                </PrivateRoute>
                            }
                        />



                        {/* Openbare pagina's */}
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
                        <Route path="NotificatieZakelijk" element={<Notification />} />
                        <Route path="Schadeclaims" element={<Schadeclaims />} />
                        <Route path="FrontOfficeMedewerker" element={<FrontOfficeMedewerker />} />
                        <Route path="VoertuigDetails" element={<VoertuigDetails />} />
                        <Route path="VoertuigInenUitname" element={<VoertuigInEnUitname /> } />

                    </Route>
                </Routes>
            </BrowserRouter>
        </UserProvider>
    );
}

export default PageRoute;