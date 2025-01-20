import { StrictMode } from 'react'
import { createRoot } from 'react-dom/client'
import '../src/styles/styles.css'
import PageRoute from './PageRoute.jsx'
import { ToastContainer } from 'react-toastify';
import 'react-toastify/dist/ReactToastify.css';

// de root van ons document om routing te gebruiken
createRoot(document.getElementById('root')).render(
    <StrictMode>
        <ToastContainer />
        <PageRoute />
    </StrictMode>,
);
