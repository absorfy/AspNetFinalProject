import {loadBoardLists} from "./load/loadBoardList.js";
import {boardListsContainer, currentBoardId} from "./dom.js";
import {initBoardDashboardEvents} from "./events/eventsHandler.js";
import {initCardsDnd} from "../../../shared/ui/cardsDnd.js";

document.addEventListener("DOMContentLoaded", async() => {
  initBoardDashboardEvents(currentBoardId);
  await loadBoardLists(currentBoardId, boardListsContainer);
});