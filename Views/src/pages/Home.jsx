/* eslint-disable no-unused-vars */
import React, { useEffect } from "react";
import "../styles/styles.css";


const Home = () => {
    useEffect(() => {
        console.log("Navigeert naar Home");
    }, []);

    return (
        <>
            {/* Header*/}
            <header>
                <div className="container">
                    <a href="#" className="logo">
                    </a>

                </div>
            </header>

            <div className="hero" id="home">
                <div className="hero-text-container">
                    <h1>Welkom bij Onze Website</h1>
                    <p>Ontdek de beste diensten die we aanbieden!</p>
                </div>

                <div className="ontdek-btn">
                    <a href="#services" className="cta">
                        Ontdek Meer
                    </a>
                </div>
            </div>

            {/*Services*/}
            <section className="services" id="services">
                <h2>Onze Diensten</h2>
                <div className="service-list">
                    {[
                        {
                            id: 1, 
                            imgSrc: "src/assets/mercedes-indesx.jpg",
                            title: "Auto's",
                            description: "Een ruim aanbod aan auto voertuigen!",
                        },
                        {
                            id: 2, 
                            imgSrc: "src/assets/camper-index.png",
                            title: "Campers",
                            description: "Elk soort camper beschikbaar die je maar kunt wensen!",
                        },
                        {
                            id: 3, 
                            imgSrc: "src/assets/caravans-index.jpg",
                            title: "Caravans",
                            description: "De droom caravan speciaal naar wens geleverd!",
                        },
                        {
                            id: 4, 
                            imgSrc: "src/assets/z&p-index.png",
                            title: "Abonnement Opties",
                            description: "De Beste abonnement opties beschikbaar voor bedrijfen!",
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

            {/*Footer*/}
            <footer>
                <p>&copy; 2024 WPR Project. Alle rechten voorbehouden.</p>
            </footer>
        </>
    );
};

export default Home;
    