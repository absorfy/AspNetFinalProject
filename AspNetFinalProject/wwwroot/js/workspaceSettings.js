import {fetchBoardsAjax} from "./api/boards.js";
import {fetchWorkspaceParticipantsAjax, updateWorkspaceAjax} from "./api/workspaces.js";

document.addEventListener("DOMContentLoaded", () => {
  const tabContent = {
    general: document.getElementById("tab-general"),
    boards: document.getElementById("tab-boards"),
    participants: document.getElementById("tab-participants"),
  };

  let loadedTabs = {
    boards: false,
    participants: false
  };

  document.querySelectorAll("#settingsTabs .nav-link").forEach(link => {
    link.addEventListener("click", async (e) => {
      e.preventDefault();

      // switch active
      document.querySelectorAll("#settingsTabs .nav-link").forEach(l => l.classList.remove("active"));
      link.classList.add("active");

      const selectedTab = link.dataset.tab;

      // hide all
      Object.values(tabContent).forEach(div => div.classList.add("d-none"));
      tabContent[selectedTab].classList.remove("d-none");

      if (selectedTab === "boards" && !loadedTabs.boards) {
        await loadBoards();
        loadedTabs.boards = true;
      }

      if (selectedTab === "participants" && !loadedTabs.participants) {
        await loadParticipants();
        loadedTabs.participants = true;
      }
    });
  });
});

const workspaceSettingsForm = document.getElementById("workspaceSettingsForm");
workspaceSettingsForm.addEventListener("submit", async (e) => {
  e.preventDefault();
  
  const formData = new FormData(workspaceSettingsForm);
  const data = Object.fromEntries(formData.entries());
  data.visibility = parseInt(data.visibility, 10);
  console.log(data);
  try {
    await updateWorkspaceAjax(workspaceId, data);
  } catch (err) {
    console.error("Error updating workspace settings:", err);
    alert("Не вдалося оновити налаштування робочого простору.");
    return;
  }
  alert("Зміни збережено успішно!");
})

async function loadBoards() {
  const container = document.getElementById("tab-boards");
  container.innerHTML = "Завантаження...";
  try {
    const boards = await fetchBoardsAjax(workspaceId);

    if (boards.length === 0) {
      container.innerHTML = "<p>Немає жодної дошки.</p>";
      return;
    }

    container.innerHTML = "";
    boards.forEach(board => {
      const div = document.createElement("div");
      div.classList.add("border", "p-2", "mb-2");
      div.innerHTML = `
                <strong>${board.title}</strong><br/>
                ${board.description ?? ""}<br/>
                <small>Автор: ${board.authorName}</small>
            `;
      container.appendChild(div);
    });
  } catch (err) {
    console.error("Error loading boards:", err);
    container.innerHTML = "<p>Не вдалося завантажити дошки.</p>";
  }
}

async function loadParticipants() {
  const container = document.getElementById("tab-participants");
  container.innerHTML = "Завантаження...";
  try {
    const participants = await fetchWorkspaceParticipantsAjax(workspaceId);

    if (participants.length === 0) {
      container.innerHTML = "<p>Немає учасників.</p>";
      return;
    }

    container.innerHTML = "";
    participants.forEach(p => {
      const div = document.createElement("div");
      div.classList.add("border", "p-2", "mb-2");
      div.innerHTML = `
                <strong>${p.username}</strong> — ${p.role}
                <br/><small>Став учасником: ${new Date(p.joiningTimestamp).toLocaleString("uk-UA")}</small>
            `;
      container.appendChild(div);
    });
  } catch (err) {
    console.error("Error loading participants:", err);
    container.innerHTML = "<p>Не вдалося завантажити учасників.</p>";
  }
}
