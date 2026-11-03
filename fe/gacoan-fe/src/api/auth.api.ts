import type { LoginDto, AuthResponseDto, RegisterDto, UserDetailDto } from "../dtos/auth.dto";
import api from './axiosInstance';
import type { ServiceResult } from '../dtos/service_result';

const ENDPOINT = "api/Auth/";

export const login = async (dto: LoginDto): Promise<ServiceResult<AuthResponseDto>> => {
    const res: ServiceResult<AuthResponseDto> = (await api.post(ENDPOINT + 'login', dto)).data;
    return res;
};

export const me = async (): Promise<ServiceResult<UserDetailDto>> => {
    const res: ServiceResult<UserDetailDto> = (await api.get(ENDPOINT + 'me')).data;
    return res;
};

export const signUp = async (dto: RegisterDto): Promise<ServiceResult<AuthResponseDto>> => {
    const res: ServiceResult<AuthResponseDto> = (await api.post(ENDPOINT + 'register', dto)).data;
    return res;
};
