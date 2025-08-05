import {createWorkspaceAjax, fetchWorkspacesAjax} from "../api/workspaces.js";

document.addEventListener("DOMContentLoaded", loadWorkspaces);
const listContainer = document.getElementById("workspaceList");

const createModalWindow = {
  title: document.getElementById("workspaceTitle"),
  description: document.getElementById("workspaceDescription"),
  modalWindow: bootstrap.Modal.getOrCreateInstance(document.getElementById("createWorkspaceModal"))
}
createModalWindow.clearInputs = function() {
  this.title.value = "";
  this.description.value = "";
}

function loadWorkspace(workspace, container) {
  const div = document.createElement("div");
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
    listContainer.innerHTML = "Завантаження робочих просторів...";
    const workspaces = await fetchWorkspacesAjax();
    listContainer.innerHTML = "";
    if (workspaces.length === 0) {
      listContainer.innerHTML = "<p>No workspaces yet. Create one!</p>";
      return;
    }
    workspaces.forEach(ws => loadWorkspace(ws, listContainer));
  }
  catch (error) {
    document.getElementById("workspaceList").innerText = "Failed to load workspaces. Please try again later.";
    console.error(error.message);
  }
}

async function createWorkspace() {
  
  try {
    const newWorkspace = await createWorkspaceAjax({ 
      title: createModalWindow.title.value, 
      description: createModalWindow.description.value 
    });
    loadWorkspace(newWorkspace, listContainer);
    createModalWindow.modalWindow.hide();
  }
  catch (error) {
    console.error(error.message);
    alert("Failed to create workspace");
  }
  finally {
    createModalWindow.clearInputs();
  }
}

window.createWorkspace = createWorkspace;

