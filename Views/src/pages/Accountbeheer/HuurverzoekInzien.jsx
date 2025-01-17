import React, { useEffect, useState } from "react";
import axios from "axios";
import jsPDF from "jspdf";
import "jspdf-autotable";
import { format, isWithinInterval, parseISO } from "date-fns";
import { nl } from "date-fns/locale";
import JwtService from "../../services/JwtService";
import "../../styles/huurverzoekinzien.css";  // Zorg ervoor dat je het CSS-bestand juist koppelt

const HuurverzoekInzien = () => {
    const [huurverzoeken, setHuurverzoeken] = useState([]);
    const [filteredHuurverzoeken, setFilteredHuurverzoeken] = useState([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null);
    const [filterStartDate, setFilterStartDate] = useState("");
    const [filterEndDate, setFilterEndDate] = useState("");

    useEffect(() => {
        const fetchHuurverzoeken = async () => {
            try {
                setLoading(true);
                const userId = await JwtService.getUserId();
                const response = await axios.get(`https://localhost:5033/api/HuurVerzoek/GetByHuurderID/${userId}`);
                const voertuigenData = await Promise.all(response.data.map(async (huur) => {
                    const voertuigGegevens = await fetchVoertuigGegevens(huur.voertuigId);
                    return {
                        ...huur,
                        ...voertuigGegevens
                    };
                }));
                setHuurverzoeken(voertuigenData);
                setFilteredHuurverzoeken(voertuigenData);
            } catch (error) {
                console.error("Fout bij het ophalen van huurverzoeken:", error);
                setError("Er is een fout opgetreden bij het laden van de huurverzoeken.");
            } finally {
                setLoading(false);
            }
        };

        fetchHuurverzoeken();
    }, []);

    const fetchVoertuigGegevens = async (voertuigId) => {
        try {
            const response = await axios.get(`https://localhost:5033/api/Voertuig/${voertuigId}`);
            return response.data;
        } catch (error) {
            console.error(`Fout bij het ophalen van voertuiggegevens voor VoertuigID: ${voertuigId}`, error);
            return {};
        }
    };

    useEffect(() => {
        let results = huurverzoeken;
        if (filterStartDate && filterEndDate) {
            const start = parseISO(filterStartDate);
            const end = parseISO(filterEndDate);
            results = results.filter(voertuig => isWithinInterval(new Date(voertuig.beginDate), { start, end }));
        }
        setFilteredHuurverzoeken(results);
    }, [filterStartDate, filterEndDate, huurverzoeken]);

    const exportToCSV = () => {
        const headers = ["Merk", "Model", "Begin Datum", "Eind Datum", "Goedgekeurd"];
        const data = filteredHuurverzoeken.map(hv => [
            hv.merk, hv.model, format(new Date(hv.beginDate), "yyyy-MM-dd"), format(new Date(hv.endDate), "yyyy-MM-dd"), hv.approved ? "Ja" : "Nee"
        ]);
        const csvContent = [headers.join(";"), ...data.map(e => e.join(";"))].join("\n");
        const blob = new Blob([csvContent], { type: "text/csv;charset=utf-8;" });
        const url = window.URL.createObjectURL(blob);
        const a = document.createElement("a");
        a.href = url;
        a.download = "huurverzoeken.csv";
        document.body.appendChild(a);
        a.click();
        document.body.removeChild(a);
    };

    const exportToPDF = () => {
        const doc = new jsPDF();
        const tableColumn = ["Merk", "Model", "Begin Datum", "Eind Datum", "Goedgekeurd"];
        const tableRows = filteredHuurverzoeken.map(hv => [
            hv.merk, hv.model, format(new Date(hv.beginDate), "yyyy-MM-dd"), format(new Date(hv.endDate), "yyyy-MM-dd"), hv.approved ? "Ja" : "Nee"
        ]);
        doc.autoTable(tableColumn, tableRows);
        doc.save("huurverzoeken.pdf");
    };

    if (loading) return <div className="huurverzoekinzien-message">Laden...</div>;
    if (error) return <div className="huurverzoekinzien-message">{error}</div>;

    return (
        <div className="huurverzoekinzien-container">
            <h1 className="huurverzoekinzien-title">Huurverzoeken Inzien</h1>
            <div className="huurverzoekinzien-filterContainer">
                <input
                    type="date"
                    value={filterStartDate}
                    onChange={e => setFilterStartDate(e.target.value)}
                    className="huurverzoekinzien-inputField"
                />
                <input
                    type="date"
                    value={filterEndDate}
                    onChange={e => setFilterEndDate(e.target.value)}
                    className="huurverzoekinzien-inputField"
                />
                <button onClick={() => { setFilterStartDate(''); setFilterEndDate(''); }} className="huurverzoekinzien-filterButton">
                    Reset Filters
                </button>
                <button onClick={exportToCSV} className="huurverzoekinzien-exportButton">Exporteer naar CSV</button>
                <button onClick={exportToPDF} className="huurverzoekinzien-exportButton">Exporteer naar PDF</button>
            </div>
            <table className="huurverzoekinzien-styledTable">
                <thead>
                    <tr>
                        <th>Merk</th>
                        <th>Model</th>
                        <th>Begin Datum</th>
                        <th>Eind Datum</th>
                        <th>Goedgekeurd</th>
                    </tr>
                </thead>
                <tbody>
                    {filteredHuurverzoeken.map((hv, index) => (
                        <tr key={index}>
                            <td>{hv.merk}</td>
                            <td>{hv.model}</td>
                            <td>{format(new Date(hv.beginDate), "dd-MM-yyyy")}</td>
                            <td>{format(new Date(hv.endDate), "dd-MM-yyyy")}</td>
                            <td>{hv.approved ? "Ja" : "Nee"}</td>
                        </tr>
                    ))}
                </tbody>
            </table>
        </div>
    );
};

export default HuurverzoekInzien;
