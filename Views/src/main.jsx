import { StrictMode } from 'react'
import { createRoot } from 'react-dom/client'
import './pages/styles.css'
import PageRoute from './PageRoute.jsx'

createRoot(document.getElementById('root')).render(
  <StrictMode>
    <PageRoute />
  </StrictMode>,
)
