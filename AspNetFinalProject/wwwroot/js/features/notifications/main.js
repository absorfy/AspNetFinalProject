import {connectNotificationsHub} from "./notificationHub.js";
import {currentUserId} from "./dom.js";
import {loadNotifications} from "./load/loadNotification.js";
import {initNotificationsEvents} from "./events/eventsHandler.js";

document.addEventListener("DOMContentLoaded", async() => {
  initNotificationsEvents();
  await connectNotificationsHub(currentUserId);
  await loadNotifications();
});