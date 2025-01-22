import { BrowserRouter, Routes, Route } from "react-router-dom";
/*import Layout from "./pages/Beveiliging/LayoutTest";*/
import Layout from "./pages/Beveiliging/Layout";
import Register from "./pages/Accountbeheer/register";
/*import Login from "./pages/login";*/
import Index from "./pages/Index";
import Privacyverklaring from "./pages/Beveiliging/Privacyverklaring";
import Abonnement from "./pages/AbonnementBeheer/abonnement";
import BedrijfsAbonnement from "./pages/AbonnementBeheer/bedrijfsabonnement";
import MedewerkerAbonnementDashboard from "./pages/Wagenparkbeheerder/medewerkerAbonnementDashboard";
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
import { UserProvider } from "./context/UserContext";
import EmailConfirmation from "./pages/Beveiliging/EmailConformation";
import PrivateRoute from "./pages/Beveiliging/PrivateRoute";
import VerhuurdeVoertuigen from "./pages/Voertuigen/VerhuurdeVoertuigen";
import VerwijderAccount from "./pages/Accountbeheer/VerwijderAccount";
import SchadeClaimMaken from "./pages/BackOfficeMedewerker/SchadeClaimMaken";
import VoertuigTonen from "./pages/BackOfficeMedewerker/VoertuigTonen";
import VoertuigDetailsBackOffice from "./pages/BackOfficeMedewerker/VoertuigDetailsBackOffice";
import VoertuigNotitieTonen from "./pages/FrontOfficeMedewerker/VoertuigNotitieToevoegen";
import VoertuigToevoegen from "./pages/BackOfficeMedewerker/VoertuigToevoegen";
import WijzigBedrijfsAbonnement from "./pages/AbonnementBeheer/WijzigBedrijfsAbonnement";
import FrontofficeRegister from "./pages/BackOfficeMedewerker/FrontofficeRegister";
import WagenparkBeheerderForm from "./pages/Zakelijkhuurder/WagenparkBeheerderForm";
import WijzigingVoertuig from "./pages/FrontOfficeMedewerker/WijzigingenVoertuig";
import HuurverzoekInzien from "./pages/Accountbeheer/HuurverzoekInzien";
import RegisterBedrijfsmedewerker from "./pages/Wagenparkbeheerder/RegisterBedrijfsmedewerker";
import PrivacyverklaringWijziging from "./pages/BackOfficeMedewerker/PrivacyVerklaringWijziging";
import WachtwoordReset from "./pages/Beveiliging/wachtwoordReset";
import WachtwoordVergeten from "./pages/Beveiliging/wachtwoordVergeten";
import TweefaReset from "./pages/Beveiliging/2faReset"; 
import FrontofficeTonen from "./pages/BackOfficeMedewerker/FrontofficeTonen";
import FrontofficeToevoegen from "./pages/BackOfficeMedewerker/FrontofficeToevoegen";
import FrontofficeDetails from "./pages/BackOfficeMedewerker/FrontofficeDetails";



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
                        <Route path="privacyverklaring" element={<Privacyverklaring />} />
                        

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

                        <Route
                            path="HuurverzoekInzien"
                            element={
                                <PrivateRoute allowedRoles={["ParticuliereHuurder", "BedrijfsMedewerker"]}>
                                    <HuurverzoekInzien />
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
                            path="WagenparkBeheerderForm"
                            element={
                                <PrivateRoute allowedRoles={["ZakelijkeHuurder"]}>
                                    <WagenparkBeheerderForm />
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
                        <Route
                            path="zaakdashboard"
                            element={
                                <PrivateRoute allowedRoles={["BedrijfsMedewerker"]}>
                                    <Zaakdashboard />
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
                            path="PrivacyVerklaringWijziging"
                            element={
                                <PrivateRoute allowedRoles={["BackofficeMedewerker"]}>
                                    <PrivacyverklaringWijziging />
                                </PrivateRoute>
                            }
                        />
                        <Route
                            path="VoertuigTonen"
                            element={
                                <PrivateRoute allowedRoles={["BackofficeMedewerker"]}>
                                    <VoertuigTonen />
                                </PrivateRoute>
                            }
                        />
                        <Route
                            path="VoertuigToevoegen"
                            element={
                                <PrivateRoute allowedRoles={["BackofficeMedewerker"]}>
                                    <VoertuigToevoegen />
                                </PrivateRoute>
                            }
                        />
                        <Route
                            path="VoertuigDetailsBackOffice/:voertuigId"
                            element={
                                <PrivateRoute allowedRoles={["BackofficeMedewerker"]}>
                                    <VoertuigDetailsBackOffice />
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
                            path="WijzigingenVoertuig/:voertuigId"
                            element={
                                <PrivateRoute allowedRoles={["FrontofficeMedewerker"]}>
                                    <WijzigingVoertuig/>
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
                        <Route
                            path="VoertuigNotitieToevoegen/:voertuigId"
                            element={
                                <PrivateRoute allowedRoles={["FrontofficeMedewerker"]}>
                                    <VoertuigNotitieTonen />
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
                            path="medewerkerAbonnementDashboard"
                            element={
                                <PrivateRoute allowedRoles={["WagenparkBeheerder"]}>
                                <MedewerkerAbonnementDashboard/>
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
                            path="WijzigBedrijfsAbonnement"
                            element={
                                <PrivateRoute allowedRoles={["WagenparkBeheerder"]}>
                                    <WijzigBedrijfsAbonnement />
                                </PrivateRoute>
                            }
                        />
                        <Route
                            path="RegisterBedrijfsmedewerker"
                            element={
                                <PrivateRoute allowedRoles={["WagenparkBeheerder"]}>
                                    <RegisterBedrijfsmedewerker />
                                </PrivateRoute>
                            }
                                />

                        <Route
                            path="FrontofficeRegister"
                            element={
                                <PrivateRoute allowedRoles={["BackofficeMedewerker"]}>
                                    <FrontofficeRegister />
                                </PrivateRoute>
                            }
                        />

                        <Route
                            path="FrontofficeTonen"
                            element={
                                <PrivateRoute allowedRoles={["BackofficeMedewerker"]}>
                                    <FrontofficeTonen />
                                </PrivateRoute>
                            }
                        />
                        <Route
                            path="FrontofficeToevoegen"
                            element={
                                <PrivateRoute allowedRoles={["BackofficeMedewerker"]}>
                                    <FrontofficeToevoegen />
                                </PrivateRoute>
                            }
                        />
                        <Route
                            path="FrontofficeDetails/:medewerkerId"
                            element={
                                <PrivateRoute allowedRoles={["BackofficeMedewerker"]}>
                                    <FrontofficeDetails />
                                </PrivateRoute>
                            }
                        />


                        {/* Openbare pagina's */}
                        <Route path="wachtwoord-vergeten" element={<WachtwoordVergeten />} />
                        <Route path="wachtwoord-reset" element={<WachtwoordReset />} />
                        <Route path="2fa-reset" element={<TweefaReset />} />
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
                        <Route path="BackOfficeMedewerker/SchadeClaimMaken" element={<SchadeClaimMaken />} />
                       
                    </Route>
                </Routes>
            </BrowserRouter>
        </UserProvider>
    );
}

export default PageRoute;