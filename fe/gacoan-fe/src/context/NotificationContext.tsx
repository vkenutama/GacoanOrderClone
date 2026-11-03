// src/context/NotificationContext.tsx
import { createContext, useContext, useState, type ReactNode } from "react";

export interface NotificationItem {
  id: string;
  title: string;
  desc: string;
  type?: "info" | "success" | "error" | "warning";
  duration?: number;
  cancellation?: ReturnType<typeof setTimeout>;
}

interface NotificationContextType {
  notifications: NotificationItem[];
  removeQueue: string[]; 
  addNotification: (data: Omit<NotificationItem, "id">, duration?: number) => void;
  triggerExit: (id: string) => void;
  holdReleaseNotification: (id: string, hold: boolean) => void;
}

const NotificationContext = createContext<NotificationContextType | undefined>(undefined);

export function NotificationProvider({ children }: { children: ReactNode }) {
  const [notifications, setNotifications] = useState<NotificationItem[]>([]);
  const [removeQueue, setRemoveQueue] = useState<string[]>([]);

  // Hapus permanen dari state setelah animasi keluar selesai (300ms)
  const triggerExit = (id: string) => {
    setRemoveQueue((prev) => (prev.includes(id) ? prev : [...prev, id]));

    setTimeout(() => {
      setNotifications((prev) => prev.filter((item) => item.id !== id));
      setRemoveQueue((prev) => prev.filter((item) => item !== id));
    }, 300); // Sesuaikan dengan durasi animasi CSS (300ms)
  };

  const addNotification = (data: Omit<NotificationItem, "id">, duration = 4000) => {
    const id = `${Date.now()}-${Math.random()}`;
    const newItem: NotificationItem = { ...data, id, duration };

    if (duration > 0) {
      // Tunggu selama durasi (misal 4 detik), baru jalankan animasi keluar
      newItem.cancellation = setTimeout(() => {
        triggerExit(id);
      }, duration);
    }

    setNotifications((prev) => [...prev, newItem]);
  };

  const holdReleaseNotification = (id: string, hold: boolean) => {
    const item = notifications.find((n) => n.id === id);
    if (!item) return;

    if (hold) {
      if (item.cancellation) clearTimeout(item.cancellation);
    } else {
      item.cancellation = setTimeout(() => {
        triggerExit(id);
      }, item.duration || 4000);
    }
  };

  return (
    <NotificationContext.Provider
      value={{
        notifications,
        removeQueue,
        addNotification,
        triggerExit,
        holdReleaseNotification,
      }}
    >
      {children}
    </NotificationContext.Provider>
  );
}

export function useNotification() {
  const context = useContext(NotificationContext);
  if (!context) {
    throw new Error("useNotification must be used within a NotificationProvider");
  }
  return context;
}