<>
  <meta charSet="UTF-8" />
  <meta name="viewport" content="width=device-width, initial-scale=1.0" />
  <title>Kies Abonnement</title>
  <link rel="stylesheet" href="styles.css" />
  <style
    dangerouslySetInnerHTML={{
      __html:
        "\n        body {\n            font-family: Arial, sans-serif;\n            background-color: #f4f4f4;\n            margin: 0;\n            padding: 0;\n            display: flex;\n            justify-content: center;\n            align-items: center;\n            height: 100vh;\n        }\n\n        .subscription-container {\n            background: #ffffff;\n            padding: 20px 30px;\n            border-radius: 8px;\n            box-shadow: 0 4px 6px rgba(0, 0, 0, 0.1);\n            text-align: center;\n            width: 100%;\n            max-width: 400px;\n            margin: 0 auto;\n            transition: background-color 1s ease, color 1s ease; /* Smooth transition */\n        }\n\n        h1 {\n            color: #333;\n            margin-bottom: 30px;\n        }\n\n        .subscription-options {\n            display: flex;\n            justify-content: space-around;\n            margin-top: 20px;\n            font-size: 18px;\n            font-weight: bold;\n        }\n\n        .option {\n            cursor: pointer;\n            padding: 10px 20px;\n            border-radius: 5px;\n            transition: transform 0.3s ease, background-color 0.3s ease; /* Smooth transition */\n        }\n\n        .option:hover {\n            background-color: #e0f7fa;\n        }\n\n        .selected {\n            transform: scale(1.2);\n            background-color: #007bff;\n            color: white;\n        }\n\n        .subscription-button {\n            background: linear-gradient(to right, #007bff, #00c6ff);\n            color: white;\n            padding: 10px 20px;\n            text-decoration: none;\n            border-radius: 4px;\n            display: inline-block;\n            font-size: 16px;\n            margin-top: 30px;\n            transition: background 0.3s ease;\n            cursor: pointer;\n        }\n\n        .subscription-button:hover {\n            background: linear-gradient(to right, #0056b3, #007bff);\n        }\n\n        .labels {\n            display: flex;\n            justify-content: space-between;\n            color: #555;\n            font-size: 14px;\n            font-weight: bold;\n            margin-top: 10px;\n        }\n    "
    }}
  />
  <div className="subscription-container">
    <h1>Kies je Abonnement</h1>
    <div className="subscription-options">
      <div className="option" id="monthly" onclick="selectOption(0)">
        Maandelijks
      </div>
      <div className="option" id="quarterly" onclick="selectOption(1)">
        Per kwartaal
      </div>
      <div className="option" id="yearly" onclick="selectOption(2)">
        Per jaar
      </div>
    </div>
    {/* Bevestigingsknop die naar de betalingen leidt */}
    <a href="payment.jsx" className="subscription-button" id="confirm-btn">
      €10 / maand - Bevestigen
    </a>
  </div>
</>
