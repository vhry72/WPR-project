<>
  <meta charSet="UTF-8" />
  <meta name="viewport" content="width=device-width, initial-scale=1.0" />
  <title>Registreren</title>
  <link rel="stylesheet" href="styles.css" />
  <div className="register-container">
    <h1>Registreren</h1>
    <div className="tabs">
      <div className="tab active" data-tab="particulier">
        Particulier
      </div>
      <div className="tab" data-tab="zakelijk">
        Zakelijk
      </div>
    </div>
    {/* Particulier Formulier */}
    <form id="form-particulier" className="form">
      <div className="form-group">
        <label htmlFor="email">E-mail</label>
        <input type="email" id="email" name="email" required="" />
      </div>
      <div className="form-group">
        <label htmlFor="voornaam">Voornaam</label>
        <input type="text" id="voornaam" name="voornaam" required="" />
      </div>
      <div className="form-group">
        <label htmlFor="achternaam">Achternaam</label>
        <input type="text" id="achternaam" name="achternaam" required="" />
      </div>
      <div className="form-group">
        <label htmlFor="telefoon">Telefoonnummer</label>
        <input type="tel" id="telefoon" name="telefoon" required="" />
      </div>
      <div className="form-group">
        <label htmlFor="adres">Adres</label>
        <input type="text" id="adres" name="adres" required="" />
      </div>
      <div className="form-group">
        <label htmlFor="wachtwoord">Wachtwoord</label>
        <input type="password" id="wachtwoord" name="wachtwoord" required="" />
      </div>
      <button type="submit" className="register-button">
        Registreren
      </button>
    </form>
    {/* Zakelijk Formulier */}
    <form id="form-zakelijk" className="form" style={{ display: "none" }}>
      <div className="form-group">
        <label htmlFor="email-zakelijk">E-mail</label>
        <input
          type="email"
          id="email-zakelijk"
          name="email-zakelijk"
          required=""
        />
      </div>
      <div className="form-group">
        <label htmlFor="wachtwoord-zakelijk">Wachtwoord</label>
        <input
          type="password"
          id="wachtwoord-zakelijk"
          name="wachtwoord-zakelijk"
          required=""
        />
      </div>
      <div className="form-group">
        <label htmlFor="bedrijfsnaam">Bedrijfsnaam</label>
        <input type="text" id="bedrijfsnaam" name="bedrijfsnaam" required="" />
      </div>
      <div className="form-group">
        <label htmlFor="adres-zakelijk">Adres</label>
        <input
          type="text"
          id="adres-zakelijk"
          name="adres-zakelijk"
          required=""
        />
      </div>
      <div className="form-group">
        <label htmlFor="kvk-nummer">KVK-nummer</label>
        <input type="text" id="kvk-nummer" name="kvk-nummer" required="" />
      </div>
      <button type="submit" className="register-button">
        Registreren
      </button>
    </form>
  </div>
</>
