import { showNotification } from './ui.js';

let notifConn;

export async function connectNotificationsHub(currentUserId) {
  notifConn = new signalR.HubConnectionBuilder()
    .withUrl("/hubs/notifications")
    .withAutomaticReconnect()
    .build();

  notifConn.on("notificationCreated", (notification) => {
    showNotification(notification);
  });

  try {
    await notifConn.start();
    await notifConn.invoke("JoinMe", currentUserId);
    console.log("Connected to notifications hub");
  } catch (e) {
    console.error("Failed to connect:", e);
  }
}