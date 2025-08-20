import {getNotification} from "./ui/getNotification.js";
import {notificationDropdown, notificationsContainer, notificationSpanCount} from "./dom.js";
import {markAsReadNotification} from "./api/notificationApi.js";

let notifConn;

let notifications = [];
let showNotificationCount = 0;

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
  updateNotificationCount();
}

export async function markAsRead(notificationId) {

  if (showNotificationCount > 0) {
    try {
      await markAsReadNotification(notificationId);
      showNotificationCount--;

      if(notifications.length > 0)
        showNotification(notifications.pop());
      else updateNotificationCount()
    }
    catch (error) {
      console.error("Failed to mark notification as read:", error.message);
    }
  }
}

function pushNotification(notification) {
  notifications.push(notification);
}

export function showNotification(notification) {
  //console.log(notification);
  if(showNotificationCount < 3) {
    const { liNotification, liDivider } = getNotification(notification);

    if (showNotificationCount === 0) {
      notificationsContainer.innerHTML = "";
    }

    notificationsContainer.appendChild(liNotification);
    notificationsContainer.appendChild(liDivider);

    showNotificationCount++;
  }
  else {
    pushNotification(notification);
  }

  updateNotificationCount()
}

function updateNotificationCount() {
  const allCount = notifications.length + showNotificationCount;
  notificationSpanCount.innerHTML = allCount;
  notificationSpanCount.hidden = allCount === 0;
  if(allCount === 0) {
    notificationDropdown.removeAttribute("data-bs-toggle");
  }
  else if(allCount === 1) {
    notificationDropdown.setAttribute("data-bs-toggle", "dropdown");
  }
  
}