
import React from "react";
import { BrowserRouter as Router, Routes, Route } from "react-router-dom";
import Home from "./Home";
import Login from "./login";
import Register from "./register";
import Abonnement from "./abonnement";
import Payment from "./payment";
import Layout from "./Layout";

function App() {
  return (
    <Router>
      <Routes>
        <Route path="/" element={<Home />} />
        <Route path="/login" element={<Login />} />
        <Route path="/register" element={<Register />} />
        <Route path="/abonnement" element={<Abonnement />} />
        <Route path="/payment" element={<Payment />} />
        <Route path="/layout" element={<Layout />} />
      </Routes>
    </Router>
  );
}

export default App;
