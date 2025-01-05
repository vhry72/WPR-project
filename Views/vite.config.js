import { defineConfig } from 'vite';
import react from '@vitejs/plugin-react';
import fs from 'fs'; // Nodig voor het lezen van certificaten

export default defineConfig({
    plugins: [react()],
    build: {
        outDir: 'dist',
        commonjsOptions: {
            include: [/node_modules/],
        },
        rollupOptions: {
            output: {
                manualChunks: {
                    jspdf: ['jspdf', 'jspdf-autotable'], // Opsplitsen van jsPDF-gerelateerde modules
                },
            },
        },
        chunkSizeWarningLimit: 1000, // Waarschuwing pas vanaf 1000 KB
    },
    server: {
        https: {
            key: fs.readFileSync('./certificates/localhost-key.pem'),
            cert: fs.readFileSync('./certificates/localhost.pem'),
        },
        host: 'localhost',
        port: 5173,
    },
});

