import LoginPage from "./pages/LoginPage"
import { useNotification } from "./context/NotificationContext"
import Notification from "./components/Notification"
import SignUpPage from "./pages/SignUpPage"
import { Navigate, Route, Routes, useNavigate } from "react-router-dom"
import { useEffect } from "react"
import { me } from "./api/auth.api"
import AdminHome from "./pages/admin/AdminHome"

function App() {
  const navigate = useNavigate()
  const { addNotification } = useNotification()

  useEffect(() => {
    handleUserLoggedId()
  }, [])

  const handleUserLoggedId = async () => {
    try {
      const res = await me()

      console.log(res)

      if (res.statusCode === 200 && res.data) {
        localStorage.setItem('user', JSON.stringify(res.data))
        navigate(res.data.role.toLocaleLowerCase() == 'admin' ? '/admin/home' : 'home')

      }
      else {
        navigate('/login')
        addNotification({
          title: "Info",
          desc: "Login terlebih dahulu untuk mengakses"
        }, 3000)
      }

    } catch (e) {
      navigate('/login')
    }
  }

  return (
    <div className="">
      <Routes>
        <Route path='/' element={<Navigate to={'/login'} />} />
        <Route path='/admin/home' element={<AdminHome />} />
        <Route path='/login' element={<LoginPage />} />
        <Route path="/signup" element={<SignUpPage />} />
        <Route path='*' element={<Navigate to={'/login'} />} />
      </Routes>

      <Notification />

    </div>
  )
}

export default App
