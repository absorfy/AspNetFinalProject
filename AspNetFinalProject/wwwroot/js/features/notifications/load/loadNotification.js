import {notificationsContainer} from "../dom.js";
import {fetchNotifications} from "../api/notificationApi.js";
import {showNotification} from "../notificationHub.js";


export async function loadNotifications() {
  notificationsContainer.innerHTML = "Завантаження...";

  try {
    const notifications = await fetchNotifications(true);
    if (notifications.length === 0) {
      notificationsContainer.innerHTML = "<p>Пусто..</p>";
      return;
    }

    notificationsContainer.innerHTML = "";
    notifications.forEach(n => {
      console.log(n);
      showNotification(n);
    });
  } catch (err) {
    console.error("Error loading notifications:", err);
    notificationsContainer.innerHTML = "<p>Не вдалося завантажити повідомлення.</p>";
  }
}