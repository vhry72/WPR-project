/* eslint-disable no-unused-vars */
import React, { useEffect } from "react";
import "../styles/styles.css";
import "../styles/navigatieBalk.css";


const Index = () => {
    useEffect(() => {
        console.log("Navigeert naar Home");
    }, []);

    return (
        <>
           

            <div className="hero" id="home">
                <div className="hero-text-container">
                    <h1>Welkom bij CarAndAll!</h1>
                    <p>Ontdek de beste diensten die we aanbieden!</p>
                </div>

                <div className="ontdek-btn">
                    <a href="#services" className="cta">
                        Ontdek Meer
                    </a>
                </div>
            </div>

            {/* Services Section */}
            <section className="services" id="services">
                <h2>Onze Diensten</h2>
                <div className="service-list">
                    {[
                        {
                            id: 1, // Toegevoegd
                            imgSrc: "src/assets/mercedes-indesx.jpg",
                            title: "Service 1",
                            description: "Beschrijving van Service 1.",
                        },
                        {
                            id: 2, // Toegevoegd
                            imgSrc: "src/assets/camper-index.png",
                            title: "Service 2",
                            description: "Beschrijving van Service 2.",
                        },
                        {
                            id: 3, // Toegevoegd
                            imgSrc: "src/assets/caravans-index.jpg",
                            title: "Service 3",
                            description: "Beschrijving van Service 3.",
                        },
                        {
                            id: 4, // Toegevoegd
                            imgSrc: "src/assets/z&p-index.png",
                            title: "Service 4",
                            description: "Beschrijving van Service 4.",
                        },
                    ].map((service) => (
                        <div className="service" key={service.id}>
                            <img src={service.imgSrc} alt={service.title} />
                            <h3>{service.title}</h3>
                            <p>{service.description}</p>
                        </div>
                    ))}
                </div>
            </section>

            {/* Footer Section */}
            <footer>
                <p>&copy; 2024 CarAndAll. Alle rechten voorbehouden.</p>
            </footer>
        </>
    );
};

export default Index;
