export interface RegisterDto{
    userName: string;
    email: string;
    fullName: string;
    password: string;
    role: string;
}

export interface LoginDto{
    userName: string;
    password: string;
}

export interface AuthResponseDto{
    token: string | undefined;
    userName: string | undefined;
    email: string | undefined;
    role: string | undefined;
}

export interface UserDetailDto {
    userId: string;
    userName: string;
    email: string;
    fullName: string;
    role: string;
    isAdmin: boolean;
}
