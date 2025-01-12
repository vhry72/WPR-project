import { BrowserRouter, Routes, Route } from "react-router-dom";
/*import Layout from "./pages/Beveiliging/LayoutTest";*/
import Layout from "./pages/Beveiliging/Layout";
import Register from "./pages/Accountbeheer/register";
/*import Login from "./pages/login";*/
import Index from "./pages/Index";
import Abonnement from "./pages/AbonnementBeheer/abonnement";
import BedrijfsAbonnement from "./pages/AbonnementBeheer/bedrijfsabonnement";
import AbonnementWijzigen from "./pages/AbonnementBeheer/WijzigAbonnement";
/*import MedewerkerAbonnementDashboard from "./pages/medewerkerAbonnementDashboard";*/
import Zaakdashboard from "./pages/Zakelijkhuurder/zaakdashboard";
/*import Wagenparklogin from "./pages/wagenparklogin";*/
import Wagendashboard from "./pages/Wagenparkbeheerder/wagendashboard";
import ZakelijkAutoTonen from "./pages/Zakelijkhuurder/ZakelijkAutoTonen";
import AccountwijzigingHuurders from "./pages/Accountbeheer/accountwijzigingHuurders";
import Instellingen from "./pages/Accountbeheer/instellingen";
import Wagenbeheer from "./pages/Wagenparkbeheerder/wagenbeheer";
import ParticulierVoertuigTonen from "./pages/Particulierhuurder/particulierVoertuigTonen";
import LoginVoorWijziging from "./pages/Beveiliging/LoginVoorWijziging";
import HuurVoertuig from "./pages/Voertuigen/huurVoertuig";
import ZakelijkHuurderDashBoard from "./pages/Zakelijkhuurder/zakelijkHuurderDashboard";
import ParticulierHuurderDashBoard from "./pages/Particulierhuurder/particulierHuurderDashBoard";
import BevestigingHuur from "./pages/Voertuigen/bevestigingHuur";
import BackOfficeMedewerker from "./pages/BackOfficeMedewerker/BackOfficeMedewerker";
import BackOfficeVerhuurAanvragen from "./pages/BackOfficeMedewerker/BackOfficeVerhuurAanvragen";
import VerhuurGegevens from "./pages/BackOfficeMedewerker/VerhuurGegevens";
import SchadeMeldingen from "./pages/BackOfficeMedewerker/SchadeMeldingen";
import Notification from "./pages/Zakelijkhuurder/NotificatieZakelijk";
import Schadeclaims from "./pages/BackOfficeMedewerker/Schadeclaims";
import FrontOfficeMedewerker from "./pages/FrontOfficeMedewerker/FrontOfficeMedewerker";
import VoertuigDetails from "./pages/FrontOfficeMedewerker/VoertuigDetails";
import VoertuigInEnUitname from "./pages/FrontOfficeMedewerker/VoertuigInenUitname";
import { UserProvider } from "./context/UserContext"; // Context import
import EmailConfirmation from "./pages/Beveiliging/EmailConformation";
import PrivateRoute from "./pages/Beveiliging/PrivateRoute";
import VerhuurdeVoertuigen from "./pages/Voertuigen/VerhuurdeVoertuigen";
import VerwijderAccount from "./pages/Accountbeheer/VerwijderAccount";
import SchadeClaimMaken from "./pages/BackOfficeMedewerker/SchadeClaimMaken";
import VoertuigTonen from "./pages/BackOfficeMedewerker/VoertuigTonen";
import VoertuigDetailsBackOffice from "./pages/BackOfficeMedewerker/VoertuigDetailsBackOffice";




function PageRoute() {
    return (
        <UserProvider>
            <BrowserRouter>
                <Routes>
                    <Route path="/" element={<Layout />}>
                        {/* Openbare routes */}
                        <Route index element={<Index />} />
                        <Route path="register" element={<Register />} />
                        <Route path="email-confirmation" element={<EmailConfirmation />} />
                        <Route path="VerhuurdeVoertuigen" element={<VerhuurdeVoertuigen/> } />
                        <Route path="login" element={<LoginVoorWijziging />} />
                        <Route path="VerwijderAccount" element={<VerwijderAccount />} />
                        

                        {/* Routes specifiek voor 'particulier' */}
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
                                <PrivateRoute allowedRoles={["ParticuliereHuurder", "BedrijfsMedewerker"]}>
                                    <BevestigingHuur />
                                </PrivateRoute>
                            }
                        />

                        <Route
                            path="huurVoertuig"
                            element={
                                <PrivateRoute allowedRoles={["ParticuliereHuurder", "BedrijfsMedewerker"]}>
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
                            path="abonnement"
                            element={
                                <PrivateRoute allowedRoles={["ZakelijkeHuurder", "WagenparkBeheerder"]}>
                                    <Abonnement />
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

                        <Route
                            path="accountwijzigingHuurders"
                            element={
                                <PrivateRoute allowedRoles={["ParticuliereHuurder"]}>
                                    <AccountwijzigingHuurders />
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

                        <Route
                            path="accountwijzigingHuurders"
                            element={
                                <PrivateRoute allowedRoles={["ParticuliereHuurder"]}>
                                    <AccountwijzigingHuurders />
                                </PrivateRoute>
                            }
                        />

                        {/* Routes specifiek voor 'bedrijfsmedewerker' */}

                        <Route
                            path="ZakelijkAutoTonen"
                            element={
                                <PrivateRoute allowedRoles={["BedrijfsMedewerker"]}>
                                    <ZakelijkAutoTonen />
                                </PrivateRoute>
                            }
                        />

                        <Route
                            path="accountwijzigingHuurders"
                            element={
                                <PrivateRoute allowedRoles={["BedrijfsMedewerker"]}>
                                    <AccountwijzigingHuurders />
                                </PrivateRoute>
                            }
                        />


                        {/* Routes specifiek voor 'BackofficeMedewerker' */}
                        
                        <Route
                            path="SchadeMeldingen"
                            element={
                                <PrivateRoute allowedRoles={["BackofficeMedewerker", "FrontofficeMedewerker"]}>
                                    <SchadeMeldingen />
                                </PrivateRoute>
                            }
                        />
                        <Route
                            path="BackOfficeVerhuurAanvragen"
                            element={
                                <PrivateRoute allowedRoles={["BackofficeMedewerker"]}>
                                    <BackOfficeVerhuurAanvragen />
                                </PrivateRoute>
                            }
                        />
                        <Route
                            path="VerhuurGegevens"
                            element={
                                <PrivateRoute allowedRoles={["BackofficeMedewerker"]}>
                                    <VerhuurGegevens />
                                </PrivateRoute>
                            }
                        />
                        <Route
                            path="SchadeClaimMaken"
                            element={
                                <PrivateRoute allowedRoles={["BackofficeMedewerker"]}>
                                    <SchadeClaimMaken />
                                </PrivateRoute>
                            }
                        />
                        {/* Routes specifiek voor 'FrontofficeMedewerker' */}
                        
                        <Route
                            path="VoertuigDetails"
                            element={
                                <PrivateRoute allowedRoles={["FrontofficeMedewerker"]}>
                                    <VoertuigDetails />
                                </PrivateRoute>
                            }
                        />
                        <Route
                            path="VoeruigInenUitname"
                            element={
                                <PrivateRoute allowedRoles={["FrontofficeMedewerker"]}>
                                    <VoertuigInEnUitname />
                                </PrivateRoute>
                            }
                        />

                        {/* Routes specifiek voor 'wagenparkbeheerder' */}
                        <Route
                            path="wagendashboard"
                            element={
                                <PrivateRoute allowedRoles={["WagenparkBeheerder"]}>
                                    <Wagendashboard />
                                </PrivateRoute>
                            }
                        />
                        <Route
                            path="wagenbeheer"
                            element={
                                <PrivateRoute allowedRoles={["WagenparkBeheerder"]}>
                                    <Wagenbeheer />
                                </PrivateRoute>
                            }
                        />
                        <Route
                            path="bedrijfsabonnement"
                            element={
                                <PrivateRoute allowedRoles={["WagenparkBeheerder"]}>
                                    <BedrijfsAbonnement />
                                </PrivateRoute>
                            }
                        />
                        <Route 
                            path="WijzigAbonnement"
                            element={
                                <PrivateRoute allowedRoles={["WagenparkBeheerder"]}>
                                    <AbonnementWijzigen />
                                </PrivateRoute>
                                }
                        />
                       



                        {/* Openbare pagina's */}
                        
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
                        <Route path="VoertuigInenUitname" element={<VoertuigInEnUitname />} />
                        <Route path="BackOfficeMedewerker/SchadeClaimMaken" element={<SchadeClaimMaken/> } />
                        <Route path="VoertuigTonen" element={<VoertuigTonen />} />
                        <Route path="VoertuigDetailsBackOffice/:voertuigId" element={<VoertuigDetailsBackOffice />} />
                    </Route>
                </Routes>
            </BrowserRouter>
        </UserProvider>
    );
}

export default PageRoute;