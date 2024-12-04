/* eslint-disable no-unused-vars */
import React, { useEffect } from "react";
import "./styles.css"; // Zorg ervoor dat dit pad juist is 

const Home = () => {
    useEffect(() => {
        console.log("Navigeert naar Home");
    }, []);

    return (
        <>
            {/* Header Section */}
            <header>
                <div className="container">
                    <a href="#" className="logo">
                        <img src="/path-to-logo.png" alt="Website Logo" />
                    </a>
                    
                </div>
            </header>

            {/* Hero Section */}
            <section className="hero" id="home">
                <div>
                    <h1>Welkom bij Onze Website</h1>
                    <p>Ontdek de beste diensten die we aanbieden!</p>
                    <a href="#services" className="cta">
                        Ontdek Meer
                    </a>
                </div>
            </section>

            {/* Services Section */}
            <section className="services" id="services">
                <h2>Onze Diensten</h2>
                <div className="service-list">
                    {[
                        {
                            id: 1,
                            imgSrc: "/path-to-service1.jpg",
                            title: "Service 1",
                            description: "Beschrijving van Service 1.",
                        },
                        {
                            id: 2,
                            imgSrc: "/path-to-service2.jpg",
                            title: "Service 2",
                            description: "Beschrijving van Service 2.",
                        },
                        {
                            id: 3,
                            imgSrc: "/path-to-service3.jpg",
                            title: "Service 3",
                            description: "Beschrijving van Service 3.",
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
                <p>&copy; 2024 Mijn Applicatie. Alle rechten voorbehouden.</p>
            </footer>
        </>
    );
};

export default Home;
