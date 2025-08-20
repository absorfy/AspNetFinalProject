import {notificationsContainer} from "../dom.js";


export function getNotification(notification) {
  const liNotification = document.createElement("li");
  liNotification.innerHTML = `
    <a data-action="mark-as-read" data-id="${notification.id}" class="dropdown-item d-flex justify-content-between align-items-start" href="#">
        <div>
          <div class="fw-bold">Нове повідомлення від ${notification.userActionLog.userName}</div>
          <small class="text-muted">${notification.userActionLog.messages[0]}</small><br>
          <small class="text-secondary">${new Date(notification.userActionLog.timestamp).toLocaleString("uk-UA")}</small>
        </div>
        <span class="badge bg-primary ms-2">NEW</span>
    </a>
  `;
  const liDivider = document.createElement("li");
  liDivider.innerHTML = `
    <hr class="dropdown-divider">
  `;
  
  return { liNotification, liDivider };
}