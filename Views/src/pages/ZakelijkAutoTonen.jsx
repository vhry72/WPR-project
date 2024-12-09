import React, { useState } from 'react';
import "../styles/styles.css";

const SearchPage = () => {
    const [searchQuery, setSearchQuery] = useState('');

    const handleSearchChange = (event) => {
        setSearchQuery(event.target.value);
    };

    return (
        <div>
            <h1>Zoekpagina</h1>
            <div>
                <input
                    type="text"
                    value={searchQuery}
                    onChange={handleSearchChange}
                    placeholder="Typ hier om te zoeken..."
                />
            </div>
            <ul>
                {/* Lege lijst */}
            </ul>
        </div>
    );
};

export default SearchPage;

