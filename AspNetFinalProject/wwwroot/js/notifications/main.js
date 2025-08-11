import {currentUserId} from "./dom.js";
import {connectNotificationsHub} from "./hub.js";
import {loadNotifications} from "./loadNotification.js";
document.addEventListener("DOMContentLoaded", async() => {
  connectNotificationsHub(currentUserId);
  loadNotifications();
});