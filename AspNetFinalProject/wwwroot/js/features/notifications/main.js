import {connectNotificationsHub} from "./hub.js";
import {currentUserId} from "./dom.js";
import {loadNotifications} from "./load/loadNotification.js";

document.addEventListener("DOMContentLoaded", async() => {
  await connectNotificationsHub(currentUserId);
  await loadNotifications();
});