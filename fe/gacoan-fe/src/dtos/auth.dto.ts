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
    Token: string | undefined;
    UserName: string | undefined;
    Email: string | undefined;
    Role: string | undefined;
}