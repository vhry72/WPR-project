import React, { useState } from 'react';

const BackOfficeVerhuurAanvraag = () => {
    // Dummy lijst van items met hun initiële goedkeuringsstatus en lege reden
    const initialList = [
        { id: 1, name: 'Ford Focus van 12-11 tot 13-11', approved: null, reason: '' },
        { id: 2, name: 'Tyota Aygo van 19-12 tot 01-01', approved: null, reason: '' },
        { id: 3, name: 'Nissan Note van 01-02 tot 04-05', approved: null, reason: '' },
        { id: 4, name: 'Mercedes Benz van 12-12 tot 14-12', approved: null, reason: '' },
    ];

    // Gebruik van useState om de lijst van items en afgekeurde verzoeken bij te houden
    const [items, setItems] = useState(initialList);
    const [rejectedItems, setRejectedItems] = useState([]); // Voor afgekeurde verzoeken

    // Functie om de goedkeuringsstatus van een item te veranderen
    const handleApproval = (id, status) => {
        const updatedItem = items.find(item => item.id === id);

        if (status) {
            // Goedkeuren: Verwijderen uit de lijst
            setItems(items.filter(item => item.id !== id));
        } else {
            // Afkeuren: Verplaatsen naar de afgekeurde lijst
            setRejectedItems([...rejectedItems, { ...updatedItem, approved: false }]);
            setItems(items.filter(item => item.id !== id));
        }
    };

    // Functie om de reden bij te werken (voor goedkeuren of afkeuren)
    const handleReasonChange = (id, value) => {
        setItems(items.map(item =>
            item.id === id ? { ...item, reason: value } : item
        ));
    };

    return (
        <div>
            <h1>Verhuur Aanvragen</h1>

            {/* Lijst van actieve items */}
            <ul>
                {items.map((item) => (
                    <li key={item.id}>
                        <span>{item.name}</span>
                        <span style={{ marginLeft: '10px' }}>
                            {item.approved === null
                                ? 'Wachten op goedkeuring'
                                : item.approved ? 'Goedgekeurd' : 'Afgekeurd'}
                        </span>

                        {/* Het invulveld voor de reden */}
                        <div>
                            <label htmlFor={`reason-${item.id}`} style={{ marginLeft: '10px' }}>
                                Reden:
                            </label>
                            <input
                                id={`reason-${item.id}`}
                                type="text"
                                value={item.reason}
                                onChange={(e) => handleReasonChange(item.id, e.target.value)}
                                placeholder="Geef een reden"
                                style={{ marginLeft: '10px', padding: '5px', width: '300px' }}
                            />
                        </div>

                        {/* Goedkeurings- en afkeurknoppen */}
                        <button
                            onClick={() => handleApproval(item.id, true)}
                            style={{ marginLeft: '10px', backgroundColor: 'green', color: 'white' }}
                            disabled={!item.reason.trim()} // Voorkom goedkeuren zonder reden
                        >
                            Goedkeuren
                        </button>
                        <button
                            onClick={() => handleApproval(item.id, false)}
                            style={{ marginLeft: '10px', backgroundColor: 'red', color: 'white' }}
                            disabled={!item.reason.trim()} // Voorkom afkeuren zonder reden
                        >
                            Afkeuren
                        </button>
                    </li>
                ))}
            </ul>

            {/* Lijst van afgekeurde items */}
            {rejectedItems.length > 0 && (
                <div>
                    <h2>Afgekeurde Verzoeken</h2>
                    <ul>
                        {rejectedItems.map((item) => (
                            <li key={item.id}>
                                <span>{item.name}</span>
                                <span style={{ marginLeft: '10px', color: 'red' }}>
                                    Afgekeurd
                                </span>
                                <p style={{ marginLeft: '10px' }}>
                                    Reden: {item.reason || 'Geen reden opgegeven'}
                                </p>
                            </li>
                        ))}
                    </ul>
                </div>
            )}
        </div>
    );
};

export default BackOfficeVerhuurAanvraag;
