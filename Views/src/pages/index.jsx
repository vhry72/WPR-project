/* eslint-disable no-unused-vars */
import React, { useEffect, useState } from "react";
import CookieConsent from "react-cookie-consent"; // Importeren van de library
import "../styles/styles.css";
import "../styles/CookieConsent.css";
import "../styles/navigatieBalk.css";


// de home pagina waar je wordt gestuurd wanneer je de website opent
const Index = () => {
    const [showBanner, setShowBanner] = useState(false);

    function getCookie(name) {
        const value = `; ${document.cookie}`;
        const parts = value.split(`; ${name}=`);
        if (parts.length === 2) return parts.pop().split(';').shift();
    }

    useEffect(() => {
        // Controleer of de cookie al bestaat
        const consent = getCookie("cookie-consent");
        setShowBanner(!consent); // Alleen tonen als er geen consent cookie is
    }, []);

    return (
        <>
            {showBanner && (
                <div className="overlay">
                    <div className="cookieConsentModal">
                        <CookieConsent
                            location="bottom"
                            buttonText="OK"
                            cookieName="cookie-consent"
                            styleName="cookieConsentModal"
                            buttonStyleName="cookieConsentButton"
                            onAccept={() => {
                                // Cookie wordt automatisch gezet door CookieConsent component
                                setShowBanner(false);
                            }}
                            expires={365}
                        >
                            Wij gebruiken een noodzakelijk cookie voor authenticatie die essentieel is voor de werking van deze website. Dit cookie helpt ons jouw sessie te beheren en zorgt ervoor dat je niet opnieuw hoeft in te loggen bij elk bezoek. Door verder te gaan, accepteer je ons gebruik van dit cookie. Lees meer over ons <a href="/PrivacyVerklaring" className="privacyLink">privacybeleid</a>.
                        </CookieConsent>
                    </div>
                </div>
            )}
            

            <div className="hero-section" id="home">
                <div className="hero-text">
                    <h1>Welkom bij CarAndAll!</h1>
                    <p>Ontdek de beste diensten die we aanbieden!</p>
                </div>
            </div>

            {/* Services Section */}
            <section className="services-section" id="services">
                <h2 className="onze-diensten">Onze Diensten</h2>
                <div className="services-grid">
                    {[
                        {
                            id: 1,
                            imgSrc: "src/assets/mercedes-indesx.jpg",
                            title: "Auto's verhuur",
                            description: "De beste auto verhuurdiensten met opties voor een ieder geschikt!",
                        },
                        {
                            id: 2,
                            imgSrc: "src/assets/camper-index.png",
                            title: "Camper verhuur",
                            description: "Ontdek onze camperverhuurdiensten voor reizen.",
                        },
                        {
                            id: 3,
                            imgSrc: "src/assets/caravans-index.jpg",
                            title: "Caravan verhuur",
                            description: "Caravan verhuurdiensten voor vakantiegangers.",
                        },
                        {
                            id: 4,
                            imgSrc: "src/assets/z&p-index.png",
                            title: "Zakelijke & Persoonlijke verhuur",
                            description: "Verhuur voor zowel zakelijke als persoonlijke behoeften.",
                        },
                    ].map((service) => (
                        <div className="service-card" key={service.id}>
                            <img
                                src={service.imgSrc}
                                alt={`${service.title} - ${service.description}`}
                            />
                            <h3>{service.title}</h3>
                            <p>{service.description}</p>
                        </div>
                    ))}
                </div>
            </section>

            {/* Footer Section */}
            <footer className="footer">
                <p>&copy; 2024 CarAndAll. Alle rechten voorbehouden.</p>
            </footer>
        </>
    );
};

export default Index;
