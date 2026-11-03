// src/components/Notification.tsx
import { useNotification } from "../context/NotificationContext";
import close from "../assets/close.png";

export default function Notification() {
  const { notifications, removeQueue, triggerExit, holdReleaseNotification } = useNotification();

  if (notifications.length === 0) return null;

  const bgColorPreset: Record<string, string> = {
    info: "#4880FF",
    success: "#10B981",
    warning: "#F59E0B",
    error: "#EF4444",
    "": "#4880FF",
  };

  return (
    <div className="fixed top-5 right-5 z-50 flex flex-col gap-3 max-w-sm w-full pointer-events-none">
      {notifications.map((item) => {
        const isExiting = removeQueue.includes(item.id);

        return (
          <div
            key={item.id}
            style={{ background: bgColorPreset[item.type ?? ""] }}
            className={`relative pointer-events-auto text-white p-4 rounded-xl shadow-lg font-nunito flex flex-col ${
              isExiting ? "notification-animation-out" : "notification-animation-in"
            }`}
            onMouseEnter={() => holdReleaseNotification(item.id, true)}
            onMouseLeave={() => holdReleaseNotification(item.id, false)}
          >
            {/* Tombol X memicu animasi keluar terlebih dahulu */}
            <button
              onClick={() => triggerExit(item.id)}
              className="absolute top-2 right-2 bg-white rounded-full p-1.5 shadow hover:scale-110 active:scale-95 transition-transform cursor-pointer"
            >
              <img src={close} alt="close" className="w-2.5 h-2.5" />
            </button>

            <h1 className="font-bold text-base leading-tight pr-6">{item.title}</h1>
            <p className="text-sm mt-1 text-white/90">{item.desc}</p>
          </div>
        );
      })}
    </div>
  );
}