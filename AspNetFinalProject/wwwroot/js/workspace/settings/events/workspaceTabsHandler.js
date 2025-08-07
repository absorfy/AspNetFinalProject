import {boardListContainer, currentWorkspaceId} from "../dom.js";
import {loadParticipants} from "../load/loadParticipants.js";
import {loadBoardsWithWorkspaceId} from "../../shared/load/loadBoards.js";

export function handleWorkspaceTabs() {
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
        await loadBoardsWithWorkspaceId(currentWorkspaceId, boardListContainer);
        loadedTabs.boards = true;
      }

      if (selectedTab === "participants" && !loadedTabs.participants) {
        await loadParticipants();
        loadedTabs.participants = true;
      }
    });
  });
}