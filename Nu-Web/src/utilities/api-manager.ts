import {UserRegistrationRequest} from "@/types/user-registration.ts";
import axios from "axios";


// Use environment variables for flexibility between environments
const baseURL = 'http://localhost:5000'; // For local development



export async function SendUserRegistrationRequest(request: UserRegistrationRequest) {
    const api = axios.create({ baseURL });
    console.log("Base URL: " + baseURL);
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