import {createWorkspaceAjax, fetchWorkspacesAjax} from "../api/workspaces.js";
import {createBoardAjax, fetchBoardsAjax} from "../api/boards.js";

document.addEventListener("DOMContentLoaded", loadWorkspaces);
const workspaceListContainer = document.getElementById("workspaceList");
const boardListContainer = document.getElementById("boardList");

const createWorkspaceModalWindow = {
  title: document.getElementById("createWorkspaceTitle"),
  description: document.getElementById("createWorkspaceDescription"),
  modalWindow: bootstrap.Modal.getOrCreateInstance(document.getElementById("createWorkspaceModal"))
}
createWorkspaceModalWindow.clearInputs = function() {
  this.title.value = "";
  this.description.value = "";
}

const createBoardModalWindow = {
  title: document.getElementById("createBoardTitle"),
  description: document.getElementById("createBoardDescription"),
  workspaceId: null, // This will be set when a workspace is selected
  modalWindow: bootstrap.Modal.getOrCreateInstance(document.getElementById("createBoardModal"))
}
createBoardModalWindow.clearInputs = function() {
  this.title.value = "";
  this.description.value = "";
}

async function selectWorkspace(workspace) {
  const title = document.getElementById("workspaceTitle");
  title.innerText = `Boards in Workspace ${workspace.title}`;
  createBoardModalWindow.workspaceId = workspace.id;
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

async function createWorkspace() {
  
  try {
    const newWorkspace = await createWorkspaceAjax({
      title: createWorkspaceModalWindow.title.value, 
      description: createWorkspaceModalWindow.description.value 
    });
    showWorkspace(newWorkspace, workspaceListContainer);
    createWorkspaceModalWindow.modalWindow.hide();
  }
  catch (error) {
    console.error(error.message);
    alert("Failed to create workspace");
  }
  finally {
    createWorkspaceModalWindow.clearInputs();
  }
}

async function createBoard() {

  try {
    const newBoard = await createBoardAjax({
      workspaceId: createBoardModalWindow.workspaceId,
      title: createBoardModalWindow.title.value,
      description: createBoardModalWindow.description.value
    });
    showBoard(newBoard, boardListContainer);
    createBoardModalWindow.modalWindow.hide();
  }
  catch (error) {
    console.error(error.message);
    alert("Failed to create board");
  }
  finally {
    createBoardModalWindow.clearInputs();
  }
}

window.createWorkspace = createWorkspace;
window.createBoard = createBoard;

