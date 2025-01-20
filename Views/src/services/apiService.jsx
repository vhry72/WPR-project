import axios from 'axios';

// gebruikt axios voor het fetchen van Backend APIS wat het overzichtelijker maakt
const API_URL = 'https://localhost:5033/api'; 

const defaultHeaders = {
    'Content-Type': 'application/json',
};

const apiService = {
    get: (path, options = {}) => {
        return axios.get(`${API_URL}${path}`, {
            headers: { ...defaultHeaders, ...(options.headers || {}) },
            params: options.params || {},
            withCredentials: true,
        });
    },

    post: (path, options = {}) => {
        return axios.post(`${API_URL}${path}`, options.body || {}, {
            headers: { ...defaultHeaders, ...(options.headers || {}) },
            withCredentials: true,
        });
    },

    put: (path, options = {}) => {
        return axios.put(`${API_URL}${path}`, options.body || {}, {
            headers: { ...defaultHeaders, ...(options.headers || {}) },
            withCredentials: true,
        });
    },

    patch: (path, options = {}) => {
        return axios.patch(`${API_URL}${path}`, options.body || {}, {
            headers: { ...defaultHeaders, ...(options.headers || {}) },
            withCredentials: true,
        });
    },

    delete: (path, options = {}) => {
        return axios.delete(`${API_URL}${path}`, {
            headers: { ...defaultHeaders, ...(options.headers || {}) },
            withCredentials: true,
        });
    },
};

export default apiService;

