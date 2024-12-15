import React, { useState } from 'react';

const BackOfficeVerhuurAanvraag = () => {
    // Dummy lijst van items met hun initiële goedkeuringsstatus en lege reden
    const initialList = [
        { id: 1, name: 'Item 1', approved: null, reason: '' },
        { id: 2, name: 'Item 2', approved: null, reason: '' },
        { id: 3, name: 'Item 3', approved: null, reason: '' },
        { id: 4, name: 'Item 4', approved: null, reason: '' },
    ];

    // Gebruik van useState om de lijst van items bij te houden
    const [items, setItems] = useState(initialList);

    // Functie om de goedkeuringsstatus van een item en de reden van goedkeuring/afkeuring te veranderen
    const handleApproval = (id, status) => {
        setItems(items.map(item =>
            item.id === id ? { ...item, approved: status, reason: '' } : item
        ));
    };

    // Functie om de reden van goedkeuring/afkeuring bij te werken
    const handleReasonChange = (id, value) => {
        setItems(items.map(item =>
            item.id === id ? { ...item, reason: value } : item
        ));
    };

    // Functie om een item te verwijderen als het goedgekeurd of afgekeurd is
    const handleRemoveItem = (id) => {
        setItems(items.filter(item => item.id !== id));
    };

    // Functie om te controleren of Enter is ingedrukt in het redeninvoerveld
    const handleKeyDown = (e, id) => {
        if (e.key === 'Enter') {
            handleRemoveItem(id); // Verwijder het item wanneer Enter wordt ingedrukt
        }
    };

    return (
        <div>
            <h1>Dummy Lijst</h1>
            <ul>
                {items.map((item) => (
                    <li key={item.id}>
                        <span>{item.name}</span>
                        <span style={{ marginLeft: '10px' }}>
                            {item.approved === null
                                ? 'Wachten op goedkeuring'
                                : item.approved ? 'Goedgekeurd' : 'Afgekeurd'}
                        </span>

                        {/* Goedkeurings- en afkeurknoppen */}
                        <button
                            onClick={() => handleApproval(item.id, true)}
                            style={{ marginLeft: '10px', backgroundColor: 'green', color: 'white' }}
                        >
                            Goedkeuren
                        </button>
                        <button
                            onClick={() => handleApproval(item.id, false)}
                            style={{ marginLeft: '10px', backgroundColor: 'red', color: 'white' }}
                        >
                            Afkeuren
                        </button>

                        {/* Het invulveld voor de reden */}
                        {item.approved !== null && (
                            <div>
                                <label htmlFor={`reason-${item.id}`} style={{ marginLeft: '10px' }}>
                                    Reden:
                                </label>
                                <input
                                    id={`reason-${item.id}`}
                                    type="text"
                                    value={item.reason}
                                    onChange={(e) => handleReasonChange(item.id, e.target.value)}
                                    onKeyDown={(e) => handleKeyDown(e, item.id)} // Luister naar de Enter-toets
                                    placeholder="Geef een reden"
                                    style={{ marginLeft: '10px', padding: '5px', width: '300px' }}
                                />
                            </div>
                        )}
                    </li>
                ))}
            </ul>
        </div>
    );
};

export default BackOfficeVerhuurAanvraag;

