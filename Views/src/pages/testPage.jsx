import React, { useState } from 'react';
import testRequestService from '../services/requests/testRequestService';

const TestPage = () => {
    const [id, setId] = useState(''); // State for entering an ID
    const [newItemName, setNewItemName] = useState(''); // State for entering a new item's name
    const [updateName, setUpdateName] = useState(''); // State for updating an item's name

    const handleGetAll = async () => {
        console.log('Fetching all items...');
        await testRequestService.getAll();
    };

    const handleGetById = async () => {
        if (!id) {
            console.error('Please enter a valid ID.');
            return;
        }
        console.log(`Fetching item with ID: ${id}`);
        await testRequestService.getById(id);
    };

    const handlePost = async () => {
        if (!newItemName) {
            console.error('Please enter a name for the new item.');
            return;
        }
        const newItem = { name: newItemName };
        console.log('Creating a new item...');
        await testRequestService.create(newItem);
    };

    const handlePut = async () => {
        if (!id || !updateName) {
            console.error('Please enter both an ID and a new name for updating the item.');
            return;
        }
        const updatedItem = { name: updateName };
        console.log(`Updating item with ID: ${id}`);
        await testRequestService.update(id, updatedItem);
    };

    const handleDelete = async () => {
        if (!id) {
            console.error('Please enter a valid ID.');
            return;
        }
        console.log(`Deleting item with ID: ${id}`);
        await testRequestService.delete(id);
    };

    return (
        <div>
            <h1>Test API Requests</h1>

            <div style={{ marginBottom: '20px' }}>
                <button onClick={handleGetAll}>Get All Items</button>
            </div>

            <div style={{ marginBottom: '20px' }}>
                <input
                    type="text"
                    placeholder="Enter ID"
                    value={id}
                    onChange={(e) => setId(e.target.value)}
                />
                <button onClick={handleGetById}>Get Item By ID</button>
            </div>

            <div style={{ marginBottom: '20px' }}>
                <input
                    type="text"
                    placeholder="Enter new item name"
                    value={newItemName}
                    onChange={(e) => setNewItemName(e.target.value)}
                />
                <button onClick={handlePost}>Create New Item</button>
            </div>

            <div style={{ marginBottom: '20px' }}>
                <input
                    type="text"
                    placeholder="Enter ID to update"
                    value={id}
                    onChange={(e) => setId(e.target.value)}
                />
                <input
                    type="text"
                    placeholder="Enter new name"
                    value={updateName}
                    onChange={(e) => setUpdateName(e.target.value)}
                />
                <button onClick={handlePut}>Update Item</button>
            </div>

            <div style={{ marginBottom: '20px' }}>
                <input
                    type="text"
                    placeholder="Enter ID to delete"
                    value={id}
                    onChange={(e) => setId(e.target.value)}
                />
                <button onClick={handleDelete}>Delete Item</button>
            </div>
        </div>
    );
};

export default TestPage;
