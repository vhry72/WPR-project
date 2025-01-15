/* eslint-disable no-unused-vars */
import React, { useEffect } from "react";
import "../styles/styles.css";
import "../styles/navigatieBalk.css";

const Index = () => {
    useEffect(() => {
        console.log("Navigeert naar Index pagina");
    }, []);

    return (
        <>
            <div className="hero-section" id="home">
                <div className="hero-text">
                    <h1>Welkom bij CarAndAll!</h1>
                    <p>Ontdek de beste diensten die we aanbieden!</p>
                </div>
            </div>

            {/* Services Section */}
            <section className="services-section" id="services">
                <h2 className = "onze-diensten">Onze Diensten</h2>
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