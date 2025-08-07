import {
  subscribeToWorkspaceAjax,
  unsubscribeFromWorkspaceAjax
} from "../../../api/workspaces.js";

import {
  deleteModal,
  deleteModalText
} from "../dom.js";

import { selectWorkspace } from "../load/loadBoards.js";

let workspaceIdToDelete = null;
let divToDelete = null;

export function showWorkspace(workspace, container) {
  const div = document.createElement("div");

  div.classList.add("border", "p-2", "mb-2", "position-relative", "cursor-pointer");
  div.innerHTML = `
    <div class="d-flex justify-content-between align-items-start">
      <div>
        <strong>${workspace.title}</strong><br/>
        ${workspace.description ?? ""}<br/>
        <small>Author: ${workspace.authorName}</small> | 
        <small>Participants: ${workspace.participantsCount}</small>
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

  // кнопка видалення
  div.querySelector(".delete-btn").addEventListener("click", (e) => {
    e.stopPropagation();
    workspaceIdToDelete = workspace.id;
    divToDelete = div;
    deleteModalText.textContent = `Ви дійсно хочете видалити робочу область "${workspace.title}"?`;
    new bootstrap.Modal(deleteModal).show();
  });

  // кнопка підписки
  const subscribeBtn = div.querySelector(".subscribe-btn");
  subscribeBtn.addEventListener("click", async (e) => {
    e.stopPropagation();

    try {
      if (workspace.isSubscribed) {
        await unsubscribeFromWorkspaceAjax(workspace.id);
        workspace.isSubscribed = false;
        subscribeBtn.classList.replace("btn-outline-secondary", "btn-outline-success");
        subscribeBtn.textContent = "Підписатися";
      } else {
        await subscribeToWorkspaceAjax(workspace.id);
        workspace.isSubscribed = true;
        subscribeBtn.classList.replace("btn-outline-success", "btn-outline-secondary");
        subscribeBtn.textContent = "Відписатися";
      }
    } catch (error) {
      alert("Помилка під час підписки/відписки: " + error.message);
    }
  });

  container.appendChild(div);
}

// доступ до змінних з іншого модуля
export function getDeleteTargets() {
  return { workspaceIdToDelete, divToDelete };
}

export function resetDeleteTargets() {
  workspaceIdToDelete = null;
  divToDelete = null;
}