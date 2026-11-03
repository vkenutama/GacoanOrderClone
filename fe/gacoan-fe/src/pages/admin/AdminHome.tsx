import { useEffect, useState } from "react"
import { useNavigate } from "react-router-dom"
import type { UserDetailDto } from "../../dtos/auth.dto"
import { UserProvider, useUser } from "../../context/UserContext"

export default function AdminHome() {
    const navigate = useNavigate()

    const [user, setUser] = useState<UserDetailDto | null>(null)

    const userContext = useUser();

    const handleLogout = () => {
        localStorage.removeItem('token')
        localStorage.removeItem('user')
        navigate('/login')
    }

    useEffect(() => {
        const userStr = localStorage.getItem('user')
        if (userStr) {
            try {
                const parsed: UserDetailDto = JSON.parse(userStr)
                setUser(parsed)
            } catch (e) {
                console.error("Gagal parse data user:", e)
            }
        }
    }, [])


    return (
        <div className="w-full min-h-screen bg-gray-50 flex flex-col items-center">
            {/* Navbar */}
            <div className="fixed top-4 w-[90%] flex flex-row items-center justify-between bg-white fadil p-3 px-6 rounded-2xl h-fit z-40">
                <div className="font-bold text-lg text-primary">

                </div>

                <div className="flex items-center gap-4 ml-auto">
                    {user && (
                        <span className="text-sm font-normal text-gray-700">
                            <span className="font-semibold">{user.fullName || user.userName}</span>
                        </span>
                    )}
                    {/* Avatar V / Inisial User */}
                    <div className="h-9 w-9 bg-primary text-white text-center rounded-full flex justify-center items-center font-bold uppercase select-none shadow">
                        {user?.fullName ? user.fullName[0] : (user?.userName ? user.userName[0] : "V")}
                    </div>

                    {/* Dialog Click User */}
                    <div className="absolute -z-10 bg-emerald-100 w-20 h-20 top-18 right-0 flex flex-col p-2">
                        <button className="bg-red-500 text-white rounded-md p-2 font-semibold self-center">Logout</button>
                    </div>



                </div>
            </div>
        </div>
    )
}
