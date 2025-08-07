import {
  createWorkspaceAjax,
  deleteWorkspaceAjax,
  fetchWorkspacesAjax,
  subscribeToWorkspaceAjax, unsubscribeFromWorkspaceAjax
} from "./api/workspaces.js";
import {createBoardAjax, fetchBoardsAjax} from "./api/boards.js";

const createWorkspaceForm = document.getElementById("createWorkspaceForm");
const createWorkspaceModal = bootstrap.Modal.getOrCreateInstance(document.getElementById("createWorkspaceModal"));

const createBoardForm = document.getElementById("createBoardForm");
const createBoardModal = bootstrap.Modal.getOrCreateInstance(document.getElementById("createBoardModal"));
let selectedWorkspaceId = null;

document.addEventListener("DOMContentLoaded", () => {
  loadWorkspaces();

  document.getElementById("confirmDeleteBtn").addEventListener("click", async () => {
    if (!workspaceIdToDelete || !divToDelete) return;

    try {
      await deleteWorkspaceAjax(workspaceIdToDelete);
      divToDelete.remove(); // Видаляємо тільки конкретний div
      const modal = bootstrap.Modal.getInstance(document.getElementById("deleteModal"));
      modal.hide();
      // Скидаємо змінні
      workspaceIdToDelete = null;
      divToDelete = null;
    } catch (err) {
      alert("Помилка при видаленні: " + err.message);
    }
  });
});

const workspaceListContainer = document.getElementById("workspaceList");
const boardListContainer = document.getElementById("boardList");

async function selectWorkspace(workspace) {
  const title = document.getElementById("workspaceTitle");
  title.innerText = `Boards in Workspace ${workspace.title}`;
  selectedWorkspaceId = workspace.id;
  await loadBoardsWithWorkspaceId(workspace.id);
}

async function loadBoardsWithWorkspaceId(workspaceId) {
  try {
    boardListContainer.innerHTML = "Завантаження дошок...";
    const boards = await fetchBoardsAjax(workspaceId);
    boardListContainer.innerHTML = "";
    if (boards.length === 0) {
      boardListContainer.innerHTML = "<p>Жодної дошки. Створіть першу!</p>";
      return;
    }
    boards.forEach(b => showBoard(b, boardListContainer));
  }
  catch (error) {
    console.error(error.message);
    alert("Failed to load boards. Please try again later.");
  }
}

function showBoard(board, container) {
  const div = document.createElement("div");
  console.log(board);
  div.classList.add("border", "p-2", "mb-2");
  div.innerHTML = `
                <strong>${board.title}</strong><br/>
                ${board.description ?? ""}<br/>
                <small>Author: ${board.authorName}</small> | 
                <small>Participants: ${board.participantsCount}</small>
            `;
  
  div.addEventListener("click", () => {
    window.location.href = `/Boards/Dashboard/${board.id}`;
  });
  
  container.appendChild(div);
}

let divToDelete = null;
let workspaceIdToDelete = null;

function showWorkspace(workspace, container) {
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
        <button 
          class="btn btn-sm btn-outline-primary settings-btn"
          data-id="${workspace.id}">
          Налаштувати
        </button>
        <button 
          class="btn btn-sm ${workspace.isSubscribed ? 'btn-outline-secondary' : 'btn-outline-success'} subscribe-btn"
          data-id="${workspace.id}">
          ${workspace.isSubscribed ? 'Відписатися' : 'Підписатися'}
        </button>
        <button 
          class="btn btn-sm btn-danger ms-2 delete-btn"
          data-id="${workspace.id}"
          data-name="${workspace.title}">
          🗑
        </button>
      </div>
    </div>
  `;
  
  div.addEventListener("click", (e) => {
    if (!e.target.closest("button")) {
      selectWorkspace(workspace);
    }
  });
  
  const settingsBtn = div.querySelector("button.settings-btn");
  settingsBtn.addEventListener("click", (e) => {
    e.stopPropagation();
    window.location.href = `/workspaces/${settingsBtn.dataset.id}/settings`;
  });
  
  const deleteBtn = div.querySelector("button.delete-btn");
  deleteBtn.addEventListener("click", (e) => {
    e.stopPropagation(); // не активувати selectWorkspace

    const name = deleteBtn.dataset.name;
    const id = deleteBtn.dataset.id;

    workspaceIdToDelete = id;
    divToDelete = div;

    document.getElementById("deleteModalText").textContent =
      `Ви дійсно хочете видалити робочу область "${name}"?`;

    const modal = new bootstrap.Modal(document.getElementById("deleteModal"));
    modal.show();
  });

  const subscribeBtn = div.querySelector("button.subscribe-btn");

  subscribeBtn.addEventListener("click", async (e) => {
    e.stopPropagation();

    const id = subscribeBtn.dataset.id;

    try {
      if (workspace.isSubscribed) {
        await unsubscribeFromWorkspaceAjax(id);
        workspace.isSubscribed = false;
        subscribeBtn.classList.remove("btn-outline-secondary");
        subscribeBtn.classList.add("btn-outline-success");
        subscribeBtn.textContent = "Підписатися";
      } else {
        await subscribeToWorkspaceAjax(id);
        workspace.isSubscribed = true;
        subscribeBtn.classList.remove("btn-outline-success");
        subscribeBtn.classList.add("btn-outline-secondary");
        subscribeBtn.textContent = "Відписатися";
      }
    } catch (error) {
      alert("Помилка під час підписки/відписки: " + error.message);
    }
  });
  
  container.appendChild(div);
}

async function loadWorkspaces() {
  try {
    workspaceListContainer.innerHTML = "Завантаження робочих просторів...";
    const workspaces = await fetchWorkspacesAjax();
    workspaceListContainer.innerHTML = "";
    if (workspaces.length === 0) {
      workspaceListContainer.innerHTML = "<p>Жодного робочого простору. Створіть перший!</p>";
      return;
    }
    workspaces.forEach(ws => showWorkspace(ws, workspaceListContainer));
  }
  catch (error) {
    document.getElementById("workspaceList").innerText = "Failed to load workspaces. Please try again later.";
    console.error(error.message);
  }
}

createWorkspaceForm.addEventListener("submit", async (e) => {
  e.preventDefault();
  
  const formData = new FormData(createWorkspaceForm);
  const title = formData.get("title");
  const description = formData.get("description");
  
  try {
    const newWorkspace = await createWorkspaceAjax({ title, description });
    showWorkspace(newWorkspace, workspaceListContainer);
    createWorkspaceModal.hide();
    createWorkspaceForm.reset();
  } catch (error) {
    console.error(error);
    alert("Не вдалося створити робочий простір.");
  }
});

createBoardForm.addEventListener("submit", async (e) => {
  e.preventDefault();
  
  const formData = new FormData(createBoardForm);
  const title = formData.get("title");
  const description = formData.get("description");
  
  try {
    const newBoard = await createBoardAjax({workspaceId: selectedWorkspaceId, title, description});
    showBoard(newBoard, boardListContainer);
    createBoardModal.hide();
    createBoardForm.reset();
  } catch (error) {
    console.error(error);
    alert("Не вдалося створити дошку.");
  }
});

