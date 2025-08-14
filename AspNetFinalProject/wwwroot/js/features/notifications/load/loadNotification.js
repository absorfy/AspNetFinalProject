import {notificationsContainer} from "../dom.js";
import {showNotification} from "../ui/showNotification.js";
import {fetchNotifications} from "../api/notificationApi.js";


export async function loadNotifications() {
  notificationsContainer.innerHTML = "Завантаження...";

  try {
    const notifications = await fetchNotifications(true, 3);
    if (notifications.length === 0) {
      notificationsContainer.innerHTML = "<p>Пусто..</p>";
      return;
    }

    notificationsContainer.innerHTML = "";
    notifications.forEach(n => {
      showNotification(n);
    });
  } catch (err) {
    console.error("Error loading notifications:", err);
    notificationsContainer.innerHTML = "<p>Не вдалося завантажити повідомлення.</p>";
  }
}