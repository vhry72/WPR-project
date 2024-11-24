
const Home = () => {
    return (
        <>
            {/* Header Section */}
            <header>
                <div className="container">
                    <a href="#" className="logo">
                        <img src="/path-to-logo.png" alt="Logo" />
                    </a>
                    <nav>
                        <ul>
                            <li>
                                <a href="#home">Home</a>
                            </li>
                            <li>
                                <a href="#services">Services</a>
                            </li>
                            <li>
                                <a href="#about">About</a>
                            </li>
                            <li>
                                <a href="#contact">Contact</a>
                            </li>
                        </ul>
                    </nav>
                </div>
            </header>

            {/* Hero Section */}
            <section className="hero">
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
                    <div className="service">
                        <img src="/path-to-service1.jpg" alt="Service 1" />
                        <h3>Service 1</h3>
                        <p>Beschrijving van Service 1.</p>
                    </div>
                    <div className="service">
                        <img src="/path-to-service2.jpg" alt="Service 2" />
                        <h3>Service 2</h3>
                        <p>Beschrijving van Service 2.</p>
                    </div>
                    <div className="service">
                        <img src="/path-to-service3.jpg" alt="Service 3" />
                        <h3>Service 3</h3>
                        <p>Beschrijving van Service 3.</p>
                    </div>
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
