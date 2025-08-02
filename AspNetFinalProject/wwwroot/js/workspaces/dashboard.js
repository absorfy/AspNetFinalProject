document.addEventListener("DOMContentLoaded", loadWorkspaces);

async function loadWorkspaces() {
  const response = await fetch("/api/workspaces");
  if (!response.ok) {
    document.getElementById("workspaceList").innerText = "Failed to load workspaces.";
    return;
  }

  const workspaces = await response.json();
  const listContainer = document.getElementById("workspaceList");
  listContainer.innerHTML = "";

  if (workspaces.length === 0) {
    listContainer.innerHTML = "<p>No workspaces yet. Create one!</p>";
    return;
  }

  workspaces.forEach(ws => {
    const div = document.createElement("div");
    div.classList.add("border", "p-2", "mb-2");
    div.innerHTML = `
                <strong>${ws.title}</strong><br/>
                ${ws.description ?? ""}<br/>
                <small>Author: ${ws.authorName}</small> | 
                <small>Participants: ${ws.participantsCount}</small>
            `;
    listContainer.appendChild(div);
  });
}

async function createWorkspace() {
  const title = document.getElementById("workspaceTitle").value;
  const description = document.getElementById("workspaceDescription").value;

  const response = await fetch("/api/workspaces", {
    method: "POST",
    headers: {
      "Content-Type": "application/json"
    },
    body: JSON.stringify({ title, description })
  });

  if (response.ok) {
    const newWorkspace = await response.json();

    // Додаємо новий workspace у список без перезавантаження
    const listContainer = document.getElementById("workspaceList");
    const div = document.createElement("div");
    div.classList.add("border", "p-2", "mb-2");
    div.innerHTML = `
                    <strong>${newWorkspace.title}</strong><br/>
                    ${newWorkspace.description ?? ""}<br/>
                    <small>Author: ${newWorkspace.authorName}</small> | 
                    <small>Participants: ${newWorkspace.participantsCount}</small>
                `;
    listContainer.appendChild(div);

    // Очищення форми
    document.getElementById("workspaceTitle").value = "";
    document.getElementById("workspaceDescription").value = "";

    // Закриваємо модальне вікно Bootstrap
    const modalElement = document.getElementById("createWorkspaceModal");
    const modal = bootstrap.Modal.getInstance(modalElement);
    modal.hide();
  } else {
    alert("Failed to create workspace");
  }
}