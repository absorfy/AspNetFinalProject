import {createWorkspaceAjax, fetchWorkspacesAjax} from "./api/workspaces.js";
import {createBoardAjax, fetchBoardsAjax} from "./api/boards.js";

const createWorkspaceForm = document.getElementById("createWorkspaceForm");
const createWorkspaceModal = bootstrap.Modal.getOrCreateInstance(document.getElementById("createWorkspaceModal"));

const createBoardForm = document.getElementById("createBoardForm");
const createBoardModal = bootstrap.Modal.getOrCreateInstance(document.getElementById("createBoardModal"));
let selectedWorkspaceId = null;

document.addEventListener("DOMContentLoaded", loadWorkspaces);

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

function showWorkspace(workspace, container) {
  const div = document.createElement("div");
  div.addEventListener("click", () => selectWorkspace(workspace));
  div.classList.add("border", "p-2", "mb-2");
  div.innerHTML = `
                <strong>${workspace.title}</strong><br/>
                ${workspace.description ?? ""}<br/>
                <small>Author: ${workspace.authorName}</small> | 
                <small>Participants: ${workspace.participantsCount}</small>
            `;
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

