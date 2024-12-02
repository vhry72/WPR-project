<>
  <meta charSet="UTF-8" />
  <meta name="viewport" content="width=device-width, initial-scale=1.0" />
  <title>Login</title>
  <link rel="stylesheet" href="styles.css" />
  <div className="login-container">
    <h1>Inloggen</h1>
    <div className="tabs">
      <div className="tab active" data-tab="particulier">
        Particulier
      </div>
      <div className="tab" data-tab="zakelijk">
        Zakelijk
      </div>
      <div className="tab" data-tab="medewerker">
        Medewerker
      </div>
    </div>
    {/* Formulier voor Particulier */}
    <form id="form-particulier" className="form">
      <label htmlFor="username-particulier">Username</label>
      <input
        type="text"
        id="username-particulier"
        name="username-particulier"
        required=""
      />
      <label htmlFor="password-particulier">Password</label>
      <input
        type="password"
        id="password-particulier"
        name="password-particulier"
        required=""
      />
      <button type="submit" className="login-button">
        Inloggen
      </button>
    </form>
    {/* Formulier voor Zakelijk */}
    <form id="form-zakelijk" className="form" style={{ display: "none" }}>
      <label htmlFor="username-zakelijk">Username</label>
      <input
        type="text"
        id="username-zakelijk"
        name="username-zakelijk"
        required=""
      />
      <label htmlFor="password-zakelijk">Password</label>
      <input
        type="password"
        id="password-zakelijk"
        name="password-zakelijk"
        required=""
      />
      <button type="submit" className="login-button">
        Inloggen
      </button>
    </form>
    {/* Formulier voor Medewerker */}
    <form id="form-mededewerker" className="form" style={{ display: "none" }}>
      <label htmlFor="username-medewerker">Username</label>
      <input
        type="text"
        id="username-medewerker"
        name="username-medewerker"
        required=""
      />
      <label htmlFor="password-medewerker">Password</label>
      <input
        type="password"
        id="password-medewerker"
        name="password-medewerker"
        required=""
      />
      <button type="submit" className="login-button">
        Inloggen
      </button>
    </form>
  </div>
</>
