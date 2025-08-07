import { fetchBoardsAjax } from "../../../api/boards.js";
import { boardListContainer, workspaceTitle } from "../dom.js";
import { showBoard } from "../ui/showBoard.js";

let selectedWorkspaceId = null;

export async function selectWorkspace(workspace) {
  workspaceTitle.innerText = `Boards in Workspace ${workspace.title}`;
  selectedWorkspaceId = workspace.id;
  await loadBoardsWithWorkspaceId(workspace.id);
}

export async function loadBoardsWithWorkspaceId(workspaceId) {
  try {
    boardListContainer.innerHTML = "Завантаження дошок...";
    const boards = await fetchBoardsAjax(workspaceId);
    boardListContainer.innerHTML = "";

    if (boards.length === 0) {
      boardListContainer.innerHTML = "<p>Жодної дошки. Створіть першу!</p>";
      return;
    }

    boards.forEach(board => showBoard(board, boardListContainer));
  } catch (error) {
    console.error(error.message);
    alert("Не вдалося завантажити дошки.");
  }
}

export function getSelectedWorkspaceId() {
  return selectedWorkspaceId;
}
