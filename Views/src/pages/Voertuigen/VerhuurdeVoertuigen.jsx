import React, { useEffect, useState } from "react";
import axios from "axios";
import jsPDF from "jspdf";
import "jspdf-autotable";
import styles from "../../styles/VerhuurdeVoertuigen.module.css";
import { format, isWithinInterval, parseISO } from "date-fns";
import { nl } from "date-fns/locale";
import JwtService from "../../services/JwtService";

const VerhuurdeVoertuigen = () => {
    const [wagenparkbeheerderId, setWagenparkbeheerderId] = useState(null);
    const [voertuigen, setVoertuigen] = useState([]);
    const [filteredVoertuigen, setFilteredVoertuigen] = useState([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null);
    const [filterStartDate, setFilterStartDate] = useState("");
    const [filterEndDate, setFilterEndDate] = useState("");
    const [searchTerm, setSearchTerm] = useState("");

    useEffect(() => {
        const executeSteps = async () => {
            try {
                const userId = await fetchUserId();
                if (!userId) {
                    setError("Geen WagenparkbeheerderID gevonden.");
                    setLoading(false);
                    return;
                }

                setWagenparkbeheerderId(userId);

                await fetchVoertuigen(userId);
            } catch (error) {
                console.error("Er is een fout opgetreden:", error);
                setError("Er is een fout opgetreden tijdens het laden van de gegevens.");
            } finally {
                setLoading(false);
            }
        };

        executeSteps();

        const interval = setInterval(() => {
            if (wagenparkbeheerderId) {
                fetchVoertuigen(wagenparkbeheerderId);
            }
        }, 30000);

        return () => clearInterval(interval);
    }, [wagenparkbeheerderId]);

    const fetchUserId = async () => {
        try {
            const userId = await JwtService.getUserId();
            console.log("Ophalen van userId succesvol:", userId);
            return userId;
        } catch (error) {
            console.error("Fout bij het ophalen van de userId:", error);
            return null;
        }
    };

    const fetchVoertuigen = async (userId) => {
        try {
            console.log("Ophalen voertuigen voor wagenparkbeheerderId:", userId);

            const medewerkersResponse = await axios.get(
                `https://localhost:5033/api/WagenparkBeheerder/${userId}/medewerkers`, { withCredentials: true });
            const medewerkerIds = medewerkersResponse.data;

            if (!medewerkerIds.length) {
                setError("Geen medewerkers gevonden voor deze wagenparkbeheerder.");
                return;
            }

            const huurverzoekenPromises = medewerkerIds.map((medewerkerId) =>
                axios.get(
                    `https://localhost:5033/api/WagenparkBeheerder/verhuurdevoertuigen/${medewerkerId}`, { withCredentials: true })
            );
            const huurverzoekenResponses = await Promise.all(huurverzoekenPromises);
            const alleHuurverzoeken = huurverzoekenResponses.flatMap((res) => res.data);

            const voertuigenMetHuurdata = await Promise.all(
                alleHuurverzoeken.map(async (huur) => {
                    const huurderNaam = await fetchHuurderNaam(huur.huurderID);
                    const voertuigGegevens = await fetchVoertuigGegevens(huur.voertuigId);

                    return {
                        voertuigId: huur.voertuigId,
                        merk: voertuigGegevens?.merk || "Onbekend",
                        model: voertuigGegevens?.model || "Onbekend",
                        kleur: voertuigGegevens?.kleur || "Onbekend",
                        kenteken: voertuigGegevens?.kenteken || "Onbekend",
                        beginDate: huur.beginDate,
                        endDate: huur.endDate,
                        approved: huur.approved,
                        huurderNaam: huurderNaam,
                    };
                })
            );

            setVoertuigen(voertuigenMetHuurdata);
            setFilteredVoertuigen(voertuigenMetHuurdata);
        } catch (error) {
            console.error("Fout bij het ophalen van voertuigen:", error);
            setError("Er is een fout opgetreden bij het ophalen van de voertuigen.");
        }
    };

    const fetchHuurderNaam = async (wagenparkbeheerderId) => {
        try {
            const response = await axios.get(
                `https://localhost:5033/api/BedrijfsMedewerkers/${wagenparkbeheerderId}`, { withCredentials: true });
            return response.data.medewerkerNaam || "Onbekend";
        } catch (error) {
            console.error(`Fout bij ophalen van naam voor HuurderID: ${wagenparkbeheerderId}`, error);
            return "Onbekend";
        }
    };

    const fetchVoertuigGegevens = async (voertuigId) => {
        try {
            const response = await axios.get(`https://localhost:5033/api/Voertuig/${voertuigId}`);
            return response.data || null;
        } catch (error) {
            console.error(
                `Fout bij ophalen van voertuiggegevens voor VoertuigID: ${voertuigId}`,
                error
            );
            return null;
        }
    };

    const formatDate = (datetime) => {
        try {
            return format(new Date(datetime), "dd MMMM yyyy HH:mm", { locale: nl });
        } catch (error) {
            console.error("Fout bij formatteren van datetime:", datetime, error);
            return "Onbekende datum";
        }
    };

    const resetFilters = () => {
        setFilterStartDate("");
        setFilterEndDate("");
        setSearchTerm("");
    };

    const exportToCSV = () => {
        if (filteredVoertuigen.length === 0) {
            console.error("Geen gegevens beschikbaar om te exporteren.");
            return;
        }

        const header = ["Merk", "Model", "Kleur", "Kenteken", "Huurder Naam", "Huurperiode", "Goedkeuring"];
        const rows = filteredVoertuigen.map((voertuig) => [
            voertuig.merk,
            voertuig.model,
            voertuig.kleur,
            voertuig.kenteken,
            voertuig.huurderNaam,
            `${formatDate(voertuig.beginDate)} - ${formatDate(voertuig.endDate)}`,
            voertuig.approved ? "Ja" : "Nee",
        ]);

        const csvContent = [header.join(";"), ...rows.map((row) => row.join(";"))].join("\n");
        const blob = new Blob([csvContent], { type: "text/csv;charset=utf-8;" });
        const link = document.createElement("a");
        link.href = URL.createObjectURL(blob);
        link.setAttribute("download", "verhuurde_voertuigen.csv");
        document.body.appendChild(link);
        link.click();
        document.body.removeChild(link);
    };

    const exportToPDF = () => {
        if (filteredVoertuigen.length === 0) {
            console.error("Geen gegevens beschikbaar om te exporteren.");
            return;
        }

        const doc = new jsPDF();
        doc.text("Overzicht Verhuurde Voertuigen", 10, 10);

        const tableColumn = ["Merk", "Model", "Kleur", "Kenteken", "Huurder Naam", "Huurperiode", "Goedkeuring"];
        const tableRows = filteredVoertuigen.map((voertuig) => [
            voertuig.merk,
            voertuig.model,
            voertuig.kleur,
            voertuig.kenteken,
            voertuig.huurderNaam,
            `${formatDate(voertuig.beginDate)} - ${formatDate(voertuig.endDate)}`,
            voertuig.approved ? "Ja" : "Nee",
        ]);

        doc.autoTable({
            head: [tableColumn],
            body: tableRows,
            startY: 20,
        });

        doc.save("verhuurde_voertuigen.pdf");
    };

    useEffect(() => {
        let gefilterdeVoertuigen = voertuigen;

        if (filterStartDate && filterEndDate) {
            const start = parseISO(filterStartDate);
            const end = parseISO(filterEndDate);

            gefilterdeVoertuigen = gefilterdeVoertuigen.filter((voertuig) =>
                isWithinInterval(new Date(voertuig.beginDate), { start, end }) ||
                isWithinInterval(new Date(voertuig.endDate), { start, end })
            );
        }

        if (searchTerm) {
            gefilterdeVoertuigen = gefilterdeVoertuigen.filter((voertuig) =>
                voertuig.huurderNaam.toLowerCase().includes(searchTerm.toLowerCase())
            );
        }

        setFilteredVoertuigen(gefilterdeVoertuigen);
    }, [voertuigen, filterStartDate, filterEndDate, searchTerm]);

    if (loading) {
        return <p className={`${styles.message} ${styles.loading}`}>Gegevens worden geladen...</p>;
    }

    if (error) {
        return <p className={`${styles.message} ${styles.error}`}>{error}</p>;
    }

    return (
        <div className={styles.container}>
            <h1>Overzicht Gehuurde Voertuigen</h1>

            <div className={styles.filterContainer}>
                <input
                    type="date"
                    value={filterStartDate}
                    onChange={(e) => setFilterStartDate(e.target.value)}
                    className={styles.inputField}
                    aria-label="Selecteer begindatum van verhuurde wagens"
                />
                <input
                    type="date"
                    value={filterEndDate}
                    onChange={(e) => setFilterEndDate(e.target.value)}
                    className={styles.inputField}
                    aria-label="Selecteer einddatum van verhuurde wagens"
                />
                <input
                    type="text"
                    value={searchTerm}
                    onChange={(e) => setSearchTerm(e.target.value)}
                    placeholder="Zoek op naam"
                    className={styles.inputField}
                    aria-label= "Selecteer op medewerkernaam"
                />
                <button className={styles.filterButton} onClick={resetFilters}>
                    Reset Filters
                </button>
            </div>

            <table className={styles.styledTable}>
                <thead>
                    <tr>
                        <th>Merk</th>
                        <th>Model</th>
                        <th>Kleur</th>
                        <th>Kenteken</th>
                        <th>Huurder Naam</th>
                        <th>Huurperiode</th>
                        <th>Goedkeuring</th>
                    </tr>
                </thead>
                <tbody>
                    {filteredVoertuigen.map((voertuig) => (
                        <tr key={voertuig.voertuigId}>
                            <td>{voertuig.merk}</td>
                            <td>{voertuig.model}</td>
                            <td>{voertuig.kleur}</td>
                            <td>{voertuig.kenteken}</td>
                            <td>{voertuig.huurderNaam}</td>
                            <td>
                                {formatDate(voertuig.beginDate)} - {formatDate(voertuig.endDate)}
                            </td>
                            <td className={styles.status}>
                                <span
                                    className={`${styles.badge} ${voertuig.approved ? styles.approved : styles.notApproved}`}
                                >
                                    {voertuig.approved ? "Ja" : "Nee"}
                                </span>
                            </td>
                        </tr>
                    ))}
                </tbody>
            </table>

            <div className={styles.exportButtons}>
                <button className={styles.exportButton} onClick={exportToCSV}>
                    Exporteer naar CSV
                </button>
                <button className={styles.exportButton} onClick={exportToPDF}>
                    Exporteer naar PDF
                </button>
            </div>
        </div>
    );
};

export default VerhuurdeVoertuigen;