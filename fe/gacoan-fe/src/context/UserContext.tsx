import { createContext, useContext, useState, useEffect, type ReactNode } from "react";
import type { UserDetailDto } from "../dtos/auth.dto";
import { me } from "../api/auth.api";

interface UserContextType {
    user: UserDetailDto | null;
    setUser: (user: UserDetailDto | null) => void;
    fetchUser: () => Promise<void>;
    logout: () => void;
}

const UserContext = createContext<UserContextType | undefined>(undefined);

export function UserProvider({ children }: { children: ReactNode }) {
    const [user, setUser] = useState<UserDetailDto | null>(() => {
        const stored = localStorage.getItem("user");
        if (stored) {
            try {
                return JSON.parse(stored) as UserDetailDto;
            } catch {
                return null;
            }
        }
        return null;
    });

    const fetchUser = async () => {
        try {
            const res = await me();
            if (res.statusCode === 200 && res.data) {
                setUser(res.data);
                localStorage.setItem("user", JSON.stringify(res.data));
            }
        } catch {
            setUser(null);
            localStorage.removeItem("user");
        }
    };

    const logout = () => {
        localStorage.removeItem("token");
        localStorage.removeItem("user");
        setUser(null);
    };

    useEffect(() => {
        const token = localStorage.getItem("token");
        if (token && !user) {
            fetchUser();
        }
    }, []);

    return (
        <UserContext.Provider value={{ user, setUser, fetchUser, logout }}>
            {children}
        </UserContext.Provider>
    );
}

export function useUser() {
    const context = useContext(UserContext);
    if (!context) {
        throw new Error("useUser must be used within a UserProvider");
    }
    return context;
}
