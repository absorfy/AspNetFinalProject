import {initCreateBoardListHandler} from "../form/createBoardList.js";
import {initCreateCardHandler} from "../form/createCard.js";
import {delegate} from "../../../../shared/utils/eventDelegator.js";
import {createCardModal} from "../dom.js";
import {initDeleteListHandler} from "./deleteListHandler.js";
import {initDeleteCardHandler} from "./deleteCardHandler.js";


export function initBoardDashboardEvents(currentBoardId) {
  initCreateBoardListHandler(currentBoardId);
  initCreateCardHandler();
  initDeleteListHandler((listId) => {
    const div = document.querySelector(`[data-list-id="${listId}"]`);
    if(div) div.remove();
  })
  initDeleteCardHandler((cardId) => {
    const div = document.querySelector(`[data-card-id="${cardId}"]`);
    if(div) div.remove();
  })
  
  delegate("click", {
    "open-create-card-modal": (btn) => {
      document.getElementById("createCardListId").value = btn.dataset.listId;
      createCardModal.show();
    },
  });
}