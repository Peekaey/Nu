import {UserAuthRequestDTO} from "@/types/user-registration.ts";
import axios, {AxiosResponse} from "axios";


// const baseURL = 'http://localhost:5000';
const baseURL = 'http://192.168.1.106:5000';


export async function SendUserRegistrationRequest(request: UserAuthRequestDTO): Promise<AxiosResponse> {
    const api = axios.create({ baseURL });
    try {
        const response = await api.post("/api/v1/Auth/register",
            JSON.stringify({
                Email: request.Email,
                Password: request.Password
            }),
            {
                headers: {
                    'Content-Type': 'application/json'
                }
            }
        );
        console.log(response);
        return response;
    } catch (error) {
        console.log(error);
        throw error;
    }
}

export async function SendUserAuthenticationRequest(request: UserAuthRequestDTO): Promise<AxiosResponse> {
    const api = axios.create({ baseURL, withCredentials: true });
    try {
        const response = await api.post("/api/v1/Auth/login",
            JSON.stringify({
                Email: request.Email,
                Password: request.Password
            }),
            {
                headers: {
                    'Content-Type': 'application/json'
                }
            }
        );
        console.log(response);
        return response;
    } catch (error) {
        console.log(error);
        throw error;
    }
}

export async function GetAllLibraryAlbums(): Promise<AxiosResponse> {
    const api = axios.create({baseURL});
    try {
        const response = await api.get("/api/v1/folder/all");
        console.log("Full Response:", response);
        console.log("Response Data:", response.data);
        return response;
    } catch (error) {
        console.error("Error fetching library albums:", error);
        throw error;
    }
}

export async function GetLibraryAlbumContents(id: string): Promise<AxiosResponse> {
    const api = axios.create({baseURL});
    try {
        const response = await api.get("/api/v1/folder/album/" + id);
        console.log("Full Response:", response);
        console.log("Response Data:", response.data);
        return response;
    } catch (error) {
        console.error("Error fetching library albums:", error);
        throw error;
    }
}

export async function GetRootLibraryContents(): Promise<AxiosResponse> {
    const api = axios.create({baseURL});
    try {
        const response = await api.get("/api/v1/folder/root");
        console.log("Full Response:", response);
        console.log("Response Data:", response.data);
        return response;
    } catch (error) {
        console.error("Error fetching library albums:", error);
        throw error;
    }
}

export async function GetLibraryFolderContents(id: string): Promise<AxiosResponse> {
    const api = axios.create({baseURL});
    try {
        const response = await api.get("/api/v1/folder/folder/" + id);
        console.log("Full Response:", response);
        console.log("Response Data:", response.data);
        return response;
    } catch (error) {
        console.error("Error fetching library albums:", error);
        throw error;
    }
}

export async function GetRootFolderPath(): Promise<AxiosResponse> {
    const api = axios.create({baseURL});
    try {
        const response = await api.get("/api/v1/settings/rootfolderpath");
        console.log("Full Response:", response);
        console.log("Response Data:", response.data);
        return response;
    } catch (error) {
        console.error("Error fetching library albums:", error);
        throw error;
    }
}
