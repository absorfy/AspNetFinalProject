import {notificationsContainer} from "../dom.js";


export function showNotification(notification) {
  const liNotification = document.createElement("li");
  liNotification.innerHTML = `
    <li>
      <a class="dropdown-item d-flex justify-content-between align-items-start" href="#">
        <div>
          <div class="fw-bold">Нове повідомлення</div>
          <small class="text-muted">${notification.userActionLog.description}</small><br>
          <small class="text-secondary">${new Date(notification.userActionLog.timestamp).toLocaleString("uk-UA")}</small>
        </div>
        <span class="badge bg-primary ms-2">NEW</span>
      </a>
    </li>
  `;
  const liDivider = document.createElement("li");
  liDivider.innerHTML = `
    <hr class="dropdown-divider">
  `;
  
  notificationsContainer.appendChild(liNotification);
  notificationsContainer.appendChild(liDivider);
}