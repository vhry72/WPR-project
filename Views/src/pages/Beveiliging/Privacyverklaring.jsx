import React from 'react';
import '../../styles/privacyverklaring.css';

const PrivacyPolicy = () => {
    return (
        <div className="privacy-policy-container">
            <h1>Privacyverklaring</h1>
            <p>Laatst bijgewerkt: [DATUM INVOEGEN]</p>

            <section>
                <h2>1. Identiteit en contactgegevens</h2>
                <p>Naam bedrijf: [BEDRIJFSNAAM INVOEGEN]</p>
                <p>Adres: [ADRES INVOEGEN]</p>
                <p>Contact: [E-MAIL/TELEFOON INVOEGEN]</p>
            </section>

            <section>
                <h2>2. Functionaris voor Gegevensbescherming (FG)</h2>
                <p>Contactgegevens FG: [NAAM EN CONTACTGEGEVENS INVOEGEN]</p>
            </section>

            <section>
                <h2>3. Doeleinden en grondslag voor verwerking</h2>
                <p>De persoonsgegevens worden verwerkt voor de volgende doeleinden: [DOELEINDEN INVOEGEN]</p>
                <p>De grondslag voor deze verwerking is: [GRONDSLAG INVOEGEN]</p>
            </section>

            <section>
                <h2>4. Ontvangers van persoonsgegevens</h2>
                <p>De (categorieën van) ontvangers van de persoonsgegevens zijn: [ONTVANGERS INVOEGEN]</p>
            </section>

            <section>
                <h2>5. Doorgifte buiten de EU</h2>
                <p>[INFORMATIE OVER DOORGIFTE BUITEN DE EU INVOEGEN OF VERMELDEN DAT HET NIET VAN TOEPASSING IS]</p>
            </section>

            <section>
                <h2>6. Bewaartermijn</h2>
                <p>De persoonsgegevens worden bewaard voor: [BEWAARTERMIJN INVOEGEN]</p>
            </section>

            <section>
                <h2>7. Rechten van betrokkenen</h2>
                <p>U heeft recht op inzage, correctie, verwijdering, beperking van verwerking, overdraagbaarheid van gegevens en het recht om bezwaar te maken.</p>
            </section>

            <section>
                <h2>8. Klachten</h2>
                <p>U kunt een klacht indienen bij de Autoriteit Persoonsgegevens via: [WEBSITE LINK INVOEGEN]</p>
            </section>

            <section>
                <h2>9. Verplichting gegevensverstrekking</h2>
                <p>[INFORMATIE OVER OF GEGEVENS VERPLICHT ZIJN INVOEGEN]</p>
            </section>

            <section>
                <h2>10. Geautomatiseerde besluitvorming</h2>
                <p>[GEBRUIK VAN GEAUTOMATISEERDE BESLUITVORMING INVOEGEN OF VERMELDEN DAT HET NIET VAN TOEPASSING IS]</p>
            </section>

            <section>
                <h2>11. Bron van persoonsgegevens</h2>
                <p>[INFORMATIE OVER DE BRON VAN GEGEVENS INVOEGEN]</p>
            </section>

            <footer>
                <p>© [JAARTAL] [BEDRIJFSNAAM]. Alle rechten voorbehouden.</p>
            </footer>
        </div>
    );
};

export default PrivacyPolicy;
