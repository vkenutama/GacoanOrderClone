import type { LoginDto } from "../dtos/auth.dto";

const API_BASE_URL = import.meta.env.VITE_BACKEND_BASE_API_URL ?? "";
const ENDPOINT = API_BASE_URL + "/api/Auth/"

export const coba = () => {
    console.log(API_BASE_URL)
    // debugger;
}

export const Login = async (dto: LoginDto) => {
    
}