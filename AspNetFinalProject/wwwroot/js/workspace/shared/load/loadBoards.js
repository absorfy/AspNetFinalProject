import {fetchBoardsAjax} from "../../../api/boards.js";
import {showBoard} from "../ui/showBoard.js"

export async function loadBoardsWithWorkspaceId(workspaceId, boardsContainer) {
  try {
    boardsContainer.innerHTML = "Завантаження дошок...";
    const boards = await fetchBoardsAjax(workspaceId);
    boardsContainer.innerHTML = "";

    if (boards.length === 0) {
      boardsContainer.innerHTML = "<p>Жодної дошки. Створіть першу!</p>";
      return;
    }

    boards.forEach(board => showBoard(board, boardsContainer));
  } catch (error) {
    console.error(error.message);
    alert("Не вдалося завантажити дошки.");
  }
}