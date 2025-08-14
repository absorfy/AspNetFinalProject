import {initCreateBoardListHandler} from "../form/createBoardList.js";
import {initCreateCardHandler} from "../form/createCard.js";
import {delegate} from "../../../../shared/utils/eventDelegator.js";
import {createCardModal} from "../dom.js";


export function initBoardDashboardEvents(currentBoardId) {
  initCreateBoardListHandler(currentBoardId);
  initCreateCardHandler();
  
  delegate("click", {
    "open-create-card-modal": (btn) => {
      document.getElementById("createCardListId").value = btn.dataset.listId;
      createCardModal.show();
    },
  });
}