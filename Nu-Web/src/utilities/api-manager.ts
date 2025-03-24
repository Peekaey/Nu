import {UserAuthRequestDTO} from "@/types/user-registration.ts";
import axios, {AxiosResponse} from "axios";


const baseURL = 'http://localhost:5000'; // For local development


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