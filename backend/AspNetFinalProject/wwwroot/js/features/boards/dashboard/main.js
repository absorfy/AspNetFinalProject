import {loadBoardLists} from "./load/loadBoardList.js";
import {boardListsContainer, currentBoardId} from "./dom.js";
import {initBoardDashboardEvents} from "./events/eventsHandler.js";

document.addEventListener("DOMContentLoaded", async() => {
  initBoardDashboardEvents(currentBoardId);
  await loadBoardLists(currentBoardId, boardListsContainer);
});