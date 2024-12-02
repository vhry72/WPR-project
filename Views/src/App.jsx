import { BrowserRouter, Routes, Route } from "react-router-dom";
import Layout from "./pages/Layout";
import Home from "./pages/index";
import './pages/styles.css';

function App() {
    return (
        <>
            <BrowserRouter>
                <Routes>
                    <Route path="/" element={<Layout />}>
                        <Route index element={<index />} />
                    </Route>
                </Routes>
            </BrowserRouter>
        </>
    );
}

export default App;
