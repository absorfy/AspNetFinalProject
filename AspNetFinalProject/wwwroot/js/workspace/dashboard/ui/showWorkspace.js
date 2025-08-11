import { selectWorkspace } from "../load/loadBoards.js";
import {handleSubscribeToggle} from "../../shared/events/subscribeHandler.js";
import {handleDeleteWorkspaceButton} from "../../shared/events/deleteWorkspaceHandler.js";

export function showWorkspace(workspace, container) {
  const div = document.createElement("div");

  div.classList.add("border", "p-2", "mb-2", "position-relative", "cursor-pointer");
  div.innerHTML = `
    <div class="d-flex justify-content-between align-items-start">
      <div>
        <strong>${workspace.title}</strong><br/>
        ${workspace.description ?? ""}<br/>
        <small>Author: ${workspace.authorName}</small> | 
        <small>Participants: ${workspace.participantIds.length}</small>
      </div>
      <div class="text-end">
        <button class="btn btn-sm btn-outline-primary settings-btn" data-id="${workspace.id}">Налаштувати</button>
        <button class="btn btn-sm ${workspace.isSubscribed ? 'btn-outline-secondary' : 'btn-outline-success'} subscribe-btn" data-id="${workspace.id}">
          ${workspace.isSubscribed ? 'Відписатися' : 'Підписатися'}
        </button>
        <button class="btn btn-sm btn-danger ms-2 delete-btn" data-id="${workspace.id}" data-name="${workspace.title}">🗑</button>
      </div>
    </div>
  `;

  // клік по картці
  div.addEventListener("click", (e) => {
    if (!e.target.closest("button")) {
      selectWorkspace(workspace);
    }
  });

  // кнопка налаштування
  div.querySelector(".settings-btn").addEventListener("click", (e) => {
    e.stopPropagation();
    window.location.href = `/workspaces/${workspace.id}/settings`;
  });
  
  const deleteBtn = div.querySelector(".delete-btn");
  handleDeleteWorkspaceButton(deleteBtn, workspace, () => {
    div.remove();
  });
  
  const subscribeBtn = div.querySelector(".subscribe-btn");
  handleSubscribeToggle(subscribeBtn, workspace);

  container.appendChild(div);
}