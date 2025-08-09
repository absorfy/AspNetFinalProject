import {fetchListsByBoard} from "../../../api/boardLists.js";
import {showBoardList} from "../ui/showBoardList.js";
import {loadCardsForList} from "./loadCard.js";
import {boardListsContainer} from "../dom.js";

export async function loadBoardLists(boardId, container) {
  try {
    container.innerHTML = "Завантаження списків...";
    const lists = await fetchListsByBoard(boardId);
    container.innerHTML = "";

    if (lists.length === 0) {
      container.innerHTML = "<p>Немає списків. Створіть перший!</p>";
      return;
    }

    lists.forEach(list => {
      showBoardList(list, boardListsContainer);
      loadCardsForList(list.id);
    });
  } catch (err) {
    console.error(err);
    alert("Не вдалося завантажити списки.");
  }
}