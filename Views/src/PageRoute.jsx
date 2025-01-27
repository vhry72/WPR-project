import { BrowserRouter, Routes, Route } from "react-router-dom";
import Layout from "./pages/Beveiliging/Layout";
import Register from "./pages/Accountbeheer/register";
import Index from "./pages/Index";
import Privacyverklaring from "./pages/Beveiliging/Privacyverklaring";
import Abonnement from "./pages/AbonnementBeheer/abonnement";
import BedrijfsAbonnement from "./pages/AbonnementBeheer/bedrijfsabonnement";
import ZakelijkAutoTonen from "./pages/Zakelijkhuurder/ZakelijkAutoTonen";
import AccountwijzigingParticulier from "./pages/Particulierhuurder/accountwijzigingParticulier";
import Wagenbeheer from "./pages/Wagenparkbeheerder/wagenbeheer";
import ParticulierVoertuigTonen from "./pages/Particulierhuurder/particulierVoertuigTonen";
import LoginVoorWijziging from "./pages/Beveiliging/LoginVoorWijziging";
import HuurVoertuig from "./pages/Voertuigen/huurVoertuig";
import BevestigingHuur from "./pages/Voertuigen/bevestigingHuur";
import BackOfficeVerhuurAanvragen from "./pages/BackOfficeMedewerker/BackOfficeVerhuurAanvragen";
import VerhuurGegevens from "./pages/BackOfficeMedewerker/VerhuurGegevens";
import SchadeMeldingen from "./pages/BackOfficeMedewerker/SchadeMeldingen";
import Schadeclaims from "./pages/BackOfficeMedewerker/Schadeclaims";
import VoertuigDetails from "./pages/FrontOfficeMedewerker/VoertuigDetails";
import VoertuigInEnUitname from "./pages/FrontOfficeMedewerker/VoertuigInenUitname";
import { UserProvider } from "./context/UserContext";
import EmailConfirmation from "./pages/Beveiliging/EmailConformation";
import PrivateRoute from "./pages/Beveiliging/PrivateRoute";
import VerhuurdeVoertuigen from "./pages/Voertuigen/VerhuurdeVoertuigen";
import SchadeClaimMaken from "./pages/BackOfficeMedewerker/SchadeClaimMaken";
import VoertuigTonen from "./pages/BackOfficeMedewerker/VoertuigTonen";
import VoertuigDetailsBackOffice from "./pages/BackOfficeMedewerker/VoertuigDetailsBackOffice";
import VoertuigNotitieTonen from "./pages/FrontOfficeMedewerker/VoertuigNotitieToevoegen";
import VoertuigToevoegen from "./pages/BackOfficeMedewerker/VoertuigToevoegen";
import MedewerkersWijzigen from "./pages/Wagenparkbeheerder/MedewerkersWijzigen"
import WijzigBedrijfsAbonnement from "./pages/AbonnementBeheer/WijzigBedrijfsAbonnement";
import WagenparkBeheerderForm from "./pages/Zakelijkhuurder/WagenparkBeheerderForm";
import HuurverzoekInzien from "./pages/Accountbeheer/HuurverzoekInzien";
import RegisterBedrijfsmedewerker from "./pages/Wagenparkbeheerder/RegisterBedrijfsmedewerker";
import PrivacyverklaringWijziging from "./pages/BackOfficeMedewerker/PrivacyVerklaringWijziging";
import WachtwoordReset from "./pages/Beveiliging/wachtwoordReset";
import WachtwoordVergeten from "./pages/Beveiliging/wachtwoordVergeten";
import TweefaReset from "./pages/Beveiliging/2faReset"; 
import FrontofficeTonen from "./pages/BackOfficeMedewerker/FrontofficeTonen";
import FrontofficeToevoegen from "./pages/BackOfficeMedewerker/FrontofficeToevoegen";
import FrontofficeDetails from "./pages/BackOfficeMedewerker/FrontofficeDetails";
import WagenparkWijzigPagina from "./pages/ZakelijkHuurder/wagenparkWijzigPagina";
import WachtwoordResetBeheerder from "./pages/Beveiliging/WachtwoordResetBeheerder"
import AccountwijzigingZakelijk from "./pages/Zakelijkhuurder/accountwijzigingZakelijk";
import AccountwijzigingBackOffice from "./pages/BackOfficeMedewerker/accountWijzigingBackoffice";
import AbonnementenKeuren from "./pages/BackOfficeMedewerker/AbonnementKeuren";

// De routing van de paginas

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
                        <Route path="privacyverklaring" element={<Privacyverklaring />} />
                        <Route path="wachtwoord-vergeten" element={<WachtwoordVergeten />} />
                        <Route path="wachtwoord-reset" element={<WachtwoordReset />} />
                        <Route path="2fa-reset" element={<TweefaReset />} />
                        

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
                                <PrivateRoute allowedRoles={["ParticuliereHuurder", "Bedrijfsmedewerker"]}>
                                    <BevestigingHuur />
                                </PrivateRoute>
                            }
                        />

                        <Route
                            path="huurVoertuig"
                            element={
                                <PrivateRoute allowedRoles={["ParticuliereHuurder", "Bedrijfsmedewerker"]}>
                                    <HuurVoertuig />
                                </PrivateRoute>
                            }
                        />

                        <Route
                            path="accountwijzigingParticulier"
                            element={
                                <PrivateRoute allowedRoles={["ParticuliereHuurder"]}>
                                    <AccountwijzigingParticulier />
                                </PrivateRoute>
                            }
                        />

                        <Route
                            path="HuurverzoekInzien"
                            element={
                                <PrivateRoute allowedRoles={["ParticuliereHuurder", "Bedrijfsmedewerker"]}>
                                    <HuurverzoekInzien />
                                </PrivateRoute>
                            }
                        />

                        {/* Routes specifiek voor 'zakelijk' */}
                        <Route
                            path="abonnement"
                            element={
                                <PrivateRoute allowedRoles={["ZakelijkeHuurder", "Wagenparkbeheerder"]}>
                                    <Abonnement />
                                </PrivateRoute>
                            }
                        />

                        <Route
                            path="wagenparkWijzigPagina"
                            element={
                                <PrivateRoute allowedRoles={["ZakelijkeHuurder"]}>
                                    <WagenparkWijzigPagina />
                                </PrivateRoute>
                            }
                        />

                        <Route
                            path="wachtwoordResetBeheerder"
                            element={
                                <PrivateRoute allowedRoles={["ZakelijkeHuurder", "Wagenparkbeheerder", "Backofficemedewerker"]}>
                                    <WachtwoordResetBeheerder />
                                </PrivateRoute>
                            }
                        />

                        <Route
                            path="accountwijzigingZakelijk"
                            element={
                                <PrivateRoute allowedRoles={["ZakelijkeHuurder"]}>
                                    <AccountwijzigingZakelijk />
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

                        {/* Routes specifiek voor 'bedrijfsmedewerker' */}

                        <Route
                            path="ZakelijkAutoTonen"
                            element={
                                <PrivateRoute allowedRoles={["Bedrijfsmedewerker"]}>
                                    <ZakelijkAutoTonen />
                                </PrivateRoute>
                            }
                        />

                        <Route
                            path="accountwijzigingBedrijfsmedewerker"
                            element={
                                <PrivateRoute allowedRoles={["Bedrijfsmedewerker"]}>
                                    <AccountwijzigingParticulier />
                                </PrivateRoute>
                            }
                        />


                        {/* Routes specifiek voor 'BackofficeMedewerker' */}
                        
                        <Route
                            path="SchadeMeldingen"
                            element={
                                <PrivateRoute allowedRoles={["Backofficemedewerker", "Frontofficemedewerker"]}>
                                    <SchadeMeldingen />
                                </PrivateRoute>
                            }
                        />
                        <Route
                            path="AbonnementKeuren"
                            element={
                                <PrivateRoute allowedRoles={["Backofficemedewerker"]}>
                                    <AbonnementenKeuren />
                                </PrivateRoute>
                            }
                        />
                        <Route
                            path="PrivacyVerklaringWijziging"
                            element={
                                <PrivateRoute allowedRoles={["Backofficemedewerker"]}>
                                    <PrivacyverklaringWijziging />
                                </PrivateRoute>
                            }
                        />

                        <Route
                            path="accountWijzigingBackoffice"
                            element={
                                <PrivateRoute allowedRoles={["Backofficemedewerker"]}>
                                    <AccountwijzigingBackOffice />
                                </PrivateRoute>
                            }
                        />

                        <Route
                            path="VoertuigTonen"
                            element={
                                <PrivateRoute allowedRoles={["Backofficemedewerker"]}>
                                    <VoertuigTonen />
                                </PrivateRoute>
                            }
                        />
                        <Route
                            path="VoertuigToevoegen"
                            element={
                                <PrivateRoute allowedRoles={["Backofficemedewerker"]}>
                                    <VoertuigToevoegen />
                                </PrivateRoute>
                            }
                        />
                        <Route
                            path="VoertuigDetailsBackOffice/:voertuigId"
                            element={
                                <PrivateRoute allowedRoles={["Backofficemedewerker"]}>
                                    <VoertuigDetailsBackOffice />
                                </PrivateRoute>
                            }
                        />
                        <Route
                            path="BackOfficeVerhuurAanvragen"
                            element={
                                <PrivateRoute allowedRoles={["Backofficemedewerker"]}>
                                    <BackOfficeVerhuurAanvragen />
                                </PrivateRoute>
                            }
                        />
                        <Route
                            path="VerhuurGegevens"
                            element={
                                <PrivateRoute allowedRoles={["Backofficemedewerker"]}>
                                    <VerhuurGegevens />
                                </PrivateRoute>
                            }
                        />
                        <Route
                            path="SchadeClaimMaken"
                            element={
                                <PrivateRoute allowedRoles={["Backofficemedewerker", "Frontofficemedewerker"]}>
                                    <SchadeClaimMaken />
                                </PrivateRoute>
                            }
                        />
                        {/* Routes specifiek voor 'FrontofficeMedewerker' */}
                        
                        <Route
                            path="VoertuigDetails"
                            element={
                                <PrivateRoute allowedRoles={["Frontofficemedewerker"]}>
                                    <VoertuigDetails />
                                </PrivateRoute>
                            }
                        />
                        <Route
                            path="VoeruigInenUitname"
                            element={
                                <PrivateRoute allowedRoles={["Frontofficemedewerker"]}>
                                    <VoertuigInEnUitname />
                                </PrivateRoute>
                            }
                        />
                        <Route
                            path="VoertuigNotitieToevoegen/:voertuigId"
                            element={
                                <PrivateRoute allowedRoles={["Frontofficemedewerker"]}>
                                    <VoertuigNotitieTonen />
                                </PrivateRoute>
                            }
                        />

                        <Route
                            path="VoertuigInenUitname"
                            element={
                                <PrivateRoute allowedRoles={["Frontofficemedewerker"]}>
                                    <VoertuigInEnUitname />
                                </PrivateRoute>
                            }
                        />
                        {/* Routes specifiek voor 'wagenparkbeheerder' */}
                    
                        <Route
                            path="wagenbeheer"
                            element={
                                <PrivateRoute allowedRoles={["Wagenparkbeheerder"]}>
                                    <Wagenbeheer />
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
                            path="WijzigBedrijfsAbonnement"
                            element={
                                <PrivateRoute allowedRoles={["Wagenparkbeheerder"]}>
                                    <WijzigBedrijfsAbonnement />
                                </PrivateRoute>
                            }
                        />
                        <Route
                            path="RegisterBedrijfsmedewerker"
                            element={
                                <PrivateRoute allowedRoles={["Wagenparkbeheerder"]}>
                                    <RegisterBedrijfsmedewerker />
                                </PrivateRoute>
                            }
                        />
                        <Route
                            path="MedewerkersWijzigen"
                            element={
                                <PrivateRoute allowedRoles={["Wagenparkbeheerder"]}>
                                    <MedewerkersWijzigen />
                                </PrivateRoute>
                            }
                        />

                        <Route
                            path="FrontofficeTonen"
                            element={
                                <PrivateRoute allowedRoles={["Backofficemedewerker"]}>
                                    <FrontofficeTonen />
                                </PrivateRoute>
                            }
                        />
                        <Route
                            path="FrontofficeToevoegen"
                            element={
                                <PrivateRoute allowedRoles={["Backofficemedewerker"]}>
                                    <FrontofficeToevoegen />
                                </PrivateRoute>
                            }
                        />
                        <Route
                            path="FrontofficeDetails"
                            element={
                                <PrivateRoute allowedRoles={["Backofficemedewerker"]}>
                                    <FrontofficeDetails />
                                </PrivateRoute>
                            }
                        />

                        <Route
                            path="Schadeclaims"
                            element={
                                <PrivateRoute allowedRoles={["Backofficemedewerker", "Frontofficemedewerker"]}>
                                    <Schadeclaims />
                                </PrivateRoute>
                            }
                        />
                    </Route>
                </Routes>
            </BrowserRouter>
        </UserProvider>
    );
}

export default PageRoute;