import {handleCreateBoardListSubmit} from "./form/createBoardList.js";
import {loadBoardLists} from "./load/loadBoardList.js";
import {boardListsContainer, currentBoardId} from "./dom.js";
import {handleCreateCardSubmit} from "./form/createCard.js";

document.addEventListener("DOMContentLoaded", async() => {
  loadBoardLists(currentBoardId, boardListsContainer);
  handleCreateBoardListSubmit();
  handleCreateCardSubmit();
});