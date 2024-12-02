<>
  <meta charSet="UTF-8" />
  <meta name="viewport" content="width=device-width, initial-scale=1.0" />
  <title>Betaling</title>
  <link rel="stylesheet" href="styles.css" />
  <style
    dangerouslySetInnerHTML={{
      __html:
        "\n        body {\n            font-family: Arial, sans-serif;\n            background-color: #f4f4f4;\n            margin: 0;\n            padding: 0;\n            display: flex;\n            justify-content: center;\n            align-items: center;\n            height: 100vh;\n        }\n\n        .payment-container {\n            background: #ffffff;\n            padding: 20px 30px;\n            border-radius: 8px;\n            box-shadow: 0 4px 6px rgba(0, 0, 0, 0.1);\n            text-align: center;\n            width: 100%;\n            max-width: 400px;\n        }\n\n        h1 {\n            color: #333;\n            margin-bottom: 20px;\n        }\n\n        .payment-form {\n            display: flex;\n            flex-direction: column;\n            gap: 15px;\n            margin-top: 20px;\n        }\n\n        .payment-form label {\n            font-size: 14px;\n            color: #555;\n            text-align: left;\n        }\n\n        .payment-form input {\n            padding: 10px;\n            font-size: 16px;\n            border: 1px solid #ddd;\n            border-radius: 4px;\n        }\n\n        .payment-button {\n            background: linear-gradient(to right, #007bff, #00c6ff);\n            color: white;\n            padding: 15px 30px;\n            text-decoration: none;\n            border-radius: 4px;\n            display: inline-block;\n            font-size: 16px;\n            margin-top: 20px;\n            transition: background 0.3s ease;\n        }\n\n        .payment-button:hover {\n            background: linear-gradient(to right, #0056b3, #007bff);\n        }\n    "
    }}
  />
  <div className="payment-container">
    <h1>Vul je Betalingsgegevens in</h1>
    <p>
      Om je betaling af te ronden, vul de benodigde gegevens in en klik op
      'Betalen'.
    </p>
    <form action="zaakdashboard.jsx" method="post" className="payment-form">
      <div>
        <label htmlFor="iban">IBAN</label>
        <input
          type="text"
          id="iban"
          name="iban"
          required=""
          placeholder="Vul je IBAN in"
        />
      </div>
      <div>
        <label htmlFor="bank-name">Bank Naam</label>
        <input
          type="text"
          id="bank-name"
          name="bank-name"
          required=""
          placeholder="Vul de naam van je bank in"
        />
      </div>
      <div>
        <label htmlFor="account-holder">Rekeninghouder</label>
        <input
          type="text"
          id="account-holder"
          name="account-holder"
          required=""
          placeholder="Vul de naam van de rekeninghouder in"
        />
      </div>
      <button type="submit" className="payment-button">
        Betalen
      </button>
    </form>
  </div>
</>
