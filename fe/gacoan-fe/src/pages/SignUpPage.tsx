import { useState } from "react"
import { useNotification } from "../context/NotificationContext"
import type { RegisterDto } from "../dtos/auth.dto";
import { signUp } from "../api/auth.api";
import { HttpStatusCode } from "axios";
import { Link, useNavigate } from "react-router-dom";

export default function SignUpPage() {
    const NotificationTitle = "Sign Up";
    const NotificationDuration = 4000;

    const navigate = useNavigate();
    const { addNotification } = useNotification();

    const [username, setUsername] = useState('')
    const [password, setPassword] = useState('')
    const [fullName, setFullName] = useState('')
    const [email, setEmail] = useState('')
    const [role, setRole] = useState('Customer')
    const [repeatPassword, setRepeatPassword] = useState('')
    const [showPassword, setShowpassword] = useState(false)

    // Validasi form sebelum dikirim ke backend (sesuai aturan Backend .NET FluentValidation)
    const validateForm = (): boolean => {
        if (!fullName.trim()) {
            addNotification({
                title: NotificationTitle,
                desc: "Nama lengkap wajib diisi.",
                type: "error"
            });
            return false;
        }

        if (!username.trim()) {
            addNotification({
                title: NotificationTitle,
                desc: "Username wajib diisi.",
                type: "error"
            });
            return false;
        }

        if (username.trim().length < 3) {
            addNotification({
                title: NotificationTitle,
                desc: "Username minimal 3 karakter.",
                type: "error"
            });
            return false;
        }

        const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
        if (!email.trim()) {
            addNotification({
                title: NotificationTitle,
                desc: "Email wajib diisi.",
                type: "error"
            });
            return false;
        }

        if (!emailRegex.test(email.trim())) {
            addNotification({
                title: NotificationTitle,
                desc: "Format email tidak valid.",
                type: "error"
            });
            return false;
        }

        if (!password) {
            addNotification({
                title: NotificationTitle,
                desc: "Password wajib diisi.",
                type: "error"
            });
            return false;
        }

        if (password.length < 6) {
            addNotification({
                title: NotificationTitle,
                desc: "Password minimal 6 karakter.",
                type: "error"
            });
            return false;
        }

        if (password !== repeatPassword) {
            addNotification({
                title: NotificationTitle,
                desc: "Password dan Repeat Password tidak cocok.",
                type: "error"
            });
            return false;
        }

        if (role !== "Admin" && role !== "Customer") {
            addNotification({
                title: NotificationTitle,
                desc: "Role harus 'Admin' atau 'Customer'.",
                type: "error"
            });
            return false;
        }

        return true;
    };

    const handleSignUp = async (e?: React.FormEvent) => {
        if (e) e.preventDefault();

        // 1. Jalankan validator frontend
        if (!validateForm()) {
            return;
        }

        const user: RegisterDto = {
            userName: username.trim(),
            password: password,
            email: email.trim(),
            fullName: fullName.trim(),
            role: role,
        };

        try {
            const res = await signUp(user);

            if (res.statusCode === HttpStatusCode.Ok || res.statusCode === HttpStatusCode.Created) {
                addNotification(
                    {
                        title: NotificationTitle,
                        desc: `Registrasi berhasil! Silakan login.`,
                        type: "success"
                    },
                    NotificationDuration
                );

                setTimeout(() => {
                    navigate("/login");
                }, 1500);
            } else {
                addNotification(
                    {
                        title: NotificationTitle,
                        desc: res.message ?? "Registrasi gagal.",
                        type: "error"
                    },
                    NotificationDuration
                );
            }
        } catch (e: any) {
            const errorMsg = e.response?.data?.message || "Terdapat kesalahan saat registrasi.";
            addNotification(
                {
                    title: NotificationTitle,
                    desc: errorMsg,
                    type: "error"
                },
                NotificationDuration
            );
            console.error(e);
        }
    };

    const handleRoleChange = (e: React.ChangeEvent<HTMLSelectElement>) => {
        setRole(e.target.value);
    };

    return (
        <form
            onSubmit={handleSignUp}
            className="absolute inset-0 mt-10 mx-auto bg-white fadil flex h-fit w-3/4 md:w-1/2 px-6 rounded-2xl flex-col justify-center gap-8 py-16 font-nunito"
        >
            <div className="flex gap-1 flex-col items-center">
                <h1 className="font-medium text-2xl">Mendaftar ke Akun Baru</h1>
            </div>

            <div className="flex gap-6 flex-col">
                {/* Full Name */}
                <div className="flex flex-col gap-2">
                    <label htmlFor="full-name" className="nunito text-[1.05rem] text-[#202224] font-medium">Full Name</label>
                    <input
                        id="full-name"
                        type="text"
                        className="input-text"
                        value={fullName}
                        placeholder="Full Name"
                        onChange={(e) => setFullName(e.target.value)}
                    />
                </div>

                {/* Username */}
                <div className="flex flex-col gap-2">
                    <label htmlFor="username" className="nunito text-[1.05rem] text-[#202224] font-medium">Username</label>
                    <input
                        id="username"
                        type="text"
                        className="input-text"
                        value={username}
                        placeholder="Username"
                        onChange={(e) => setUsername(e.target.value)}
                    />
                </div>

                {/* Email */}
                <div className="flex flex-col gap-2">
                    <label htmlFor="email" className="nunito text-[1.05rem] text-[#202224] font-medium">Email</label>
                    <input
                        id="email"
                        type="email"
                        className="input-text"
                        value={email}
                        placeholder="Email"
                        onChange={(e) => setEmail(e.target.value)}
                    />
                </div>

                {/* Password */}
                <div className="flex flex-col gap-2">
                    <label htmlFor="password" className="nunito text-[1.05rem] text-[#202224] font-medium">Password</label>
                    <div className="flex flex-row items-center relative">
                        <input
                            id="password"
                            type={showPassword ? "text" : "password"}
                            className="input-text"
                            value={password}
                            placeholder="Password"
                            onChange={(e) => setPassword(e.target.value)}
                        />
                    </div>
                </div>

                {/* Repeat Password */}
                <div className="flex flex-col gap-2">
                    <label htmlFor="repeat-password" className="nunito text-[1.05rem] text-[#202224] font-medium">Repeat Password</label>
                    <div className="flex flex-row items-center relative">
                        <input
                            id="repeat-password"
                            type={showPassword ? "text" : "password"}
                            className="input-text"
                            value={repeatPassword}
                            placeholder="Repeat Password"
                            onChange={(e) => setRepeatPassword(e.target.value)}
                        />
                        <span
                            className="absolute right-4 uppercase text-[0.8rem] text-primary font-semibold hover:cursor-pointer select-none"
                            onClick={() => setShowpassword(!showPassword)}
                        >
                            {showPassword ? "Hide" : "Show"}
                        </span>
                    </div>
                </div>

                {/* Role */}
                <div className="flex flex-col gap-2">
                    <label htmlFor="role-selector" className="nunito text-[1.05rem] text-[#202224] font-medium">Role</label>
                    <div className="flex flex-row items-center fadil py-3 px-3 rounded-2xl bg-white">
                        <select
                            className="flex flex-1 outline-none bg-transparent cursor-pointer"
                            name="role-selector"
                            id="role-selector"
                            onChange={handleRoleChange}
                            value={role}
                        >
                            <option value="Customer">Customer</option>
                            <option value="Admin">Admin</option>
                        </select>
                    </div>
                </div>
            </div>

            <button
                type="submit"
                className="bg-primary rounded-lg text-white nunito py-3 flex justify-center items-center text-center hover:opacity-90 transition-opacity cursor-pointer"
            >
                Sign Up
            </button>

            <p className="self-center">
                Sudah memiliki akun?{" "}
                <Link to="/login" className="text-primary font-semibold underline">
                    Login
                </Link>
            </p>
        </form>
    );
}
