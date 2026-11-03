import { useState } from "react"
import { useNotification } from "../context/NotificationContext"
import type { LoginDto } from "../dtos/auth.dto";
import { login } from "../api/auth.api";
import { HttpStatusCode } from "axios";
import { Link, useNavigate } from "react-router-dom";

export default function LoginPage() {
    const { addNotification } = useNotification();
    const navigate = useNavigate()

    const NotificationDuration = 4000;

    const [username, setUsername] = useState('')
    const [password, setPassword] = useState('')
    const [showPassword, setShowpassword] = useState(false)

    const handleOnChange = (target: string, value: string) => {
        const selector: Record<string, React.Dispatch<React.SetStateAction<string>>> = {
            'username': setUsername,
            'password': setPassword
        }

        selector[target](value);
    }

    const handleLogin = async () => {
        if (username.length == 0) {
            addNotification({ title: "Login", desc: 'Username harus diisi', type: "error" }, NotificationDuration)
            return; 
        }
        if(password.length < 6){
            addNotification({ title: "Login", desc: 'Password tidak boleh kosong', type: "error" }, NotificationDuration)
            return; 
        }

        const user: LoginDto = {
            userName: username,
            password: password
        }

        try {
            const res = await login(user)
            if (res.statusCode == HttpStatusCode.Ok) {
                addNotification({ title: "Login", desc: `Login berhasil, selamat datang ${res.data?.userName}`, type: "info" }, NotificationDuration)

                localStorage.setItem('token', res.data?.token as string)
                localStorage.setItem('username', res.data?.userName as string)

                navigate(res.data?.role?.toLocaleLowerCase() == 'admin' ? "/admin/home" : "/home")
            }
            else {
                addNotification({ title: "Login", desc: res.message ?? "", type: "error" }, NotificationDuration)
            }
        } catch (e) {
            addNotification({ title: "Login", desc: e instanceof Error ? e.message : String(e), type: "error" }, NotificationDuration)
        }

    }

    return (
        <div
            className="absolute inset-0 m-auto bg-white fadil flex max-h-131.25 w-1/2 px-6 rounded-2xl flex-col justify-center gap-8 py-16 font-nunito">
            <div className="flex gap-1 flex-col items-center">
                <h1 className="font-medium text-2xl">Login to Account</h1>
                <p className="text-secondary">Masuk menggunakan akun yang pernah ada :(</p>
            </div>

            <div className="flex gap-6 flex-col">
                <div className="flex flex-col gap-2">
                    <label htmlFor="username" className="nunito text-[1.05rem] text-[#202224] font-medium">Username</label>
                    <input
                        id="username"
                        type="text"
                        className="input-text"
                        value={username}
                        placeholder="Username"
                        onChange={(e) => handleOnChange('username', e.target.value)}
                    />
                </div>

                <div className="flex flex-col gap-2">
                    <label htmlFor="password" className="nunito text-[1.05rem] text-[#202224] font-medium">Password</label>
                    <div className="flex flex-row items-center">
                        <input
                            id="password"
                            type={(showPassword ? "text" : "password")}
                            className="input-text"
                            value={password}
                            placeholder="Password"
                            onChange={(e) => handleOnChange('password', e.target.value)}
                        />
                        <span
                            className="absolute right-10 uppercase text-[0.8rem] text-primary font-semibold hover:cursor-pointer"
                            onClick={() => setShowpassword(!showPassword)}>{(showPassword) ? "Hide" : "Show"}</span>
                    </div>

                </div>
            </div>

            <button
                className="bg-primary rounded-lg text-white nunito py-3 flex justify-center items-center text-center
                hover:cursor-pointer"
                onClick={handleLogin}>Login</button>

            <p className="self-center">Don’t have an account? <Link to={'/signup'} className="text-primary font-semibold underline">Create Account</Link></p>
        </div>
    )
}