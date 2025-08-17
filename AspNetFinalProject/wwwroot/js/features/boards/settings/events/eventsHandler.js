import {initDeleteBoardHandler} from "../../../workspaces/shared/events/deleteBoardHandler.js";
import {navigate} from "../../../../shared/utils/navigation.js";
import {initBoardTabsHandler} from "./boardTabsHandler.js";
import {initDeleteBoardParticipantHandler} from "./deleteBoardParticipantHandler.js";
import {initBoardParticipantsSearchHandler, triggerParticipantsSearch} from "./participantsSearchHandler.js";
import {getBoardParticipantDiv} from "../ui/getBoardParticipantDiv.js";
import {participantContainer} from "../dom.js";
import {initAddNewParticipantHandler} from "./addNewParticipantHandler.js";
import {initSubscribeHandler} from "../../../shared/events/subscribeHandler.js";
import {subscribeToBoardAjax, unsubscribeFromBoardAjax, updateBoardAjax} from "../../api/boardApi.js";
import {delegate} from "../../../../shared/utils/eventDelegator.js";


export function initBoardSettingsEvents(boardId) {
  initDeleteBoardHandler(() => {
    navigate.toDashboard();
  })
  
  initDeleteBoardParticipantHandler(boardId, (participantId) => {
    const div = document.querySelector(`[data-participant-id="${participantId}"]`)
    if(div) div.remove();
    triggerParticipantsSearch();
  })
  initBoardTabsHandler({boardId});
  initBoardParticipantsSearchHandler(boardId);
  initAddNewParticipantHandler(boardId, async (newParticipant) => {
    const div = await getBoardParticipantDiv(newParticipant);
    participantContainer.appendChild(div);
    triggerParticipantsSearch();
  })
  initSubscribeHandler("board", subscribeToBoardAjax, unsubscribeFromBoardAjax);
  delegate("submit", {
    "update-board-form": async (form, e) => {
      e.preventDefault();
      const data = Object.fromEntries(new FormData(form).entries());
      if(data.visibility !== undefined) data.visibility = parseInt(data.visibility, 10);
      try {
        await updateBoardAjax(boardId, data);
        alert("Дошку оновлено!");
      } catch (err) {
        alert(`Не вдалося оновити дошку: ${err.message}`);
      }
    }
  });
}