import {initDeleteWorkspaceHandler} from "../../shared/events/deleteWorkspaceHandler.js";
import {initDeleteWorkspaceParticipantHandler} from "../../shared/events/deleteWorkspaceParticipantHandler.js";
import {initWorkspaceTabsHandler} from "./workspaceTabsHandler.js";
import {initWorkspaceParticipantsSearchHandler, triggerParticipantsSearch} from "./participantsSearchHandler.js";
import {delegate} from "../../../../shared/utils/eventDelegator.js";
import {subscribeToWorkspaceAjax, unsubscribeFromWorkspaceAjax, updateWorkspaceAjax} from "../../api/workspaceApi.js";
import {initAddNewParticipantHandler} from "./addNewParicipantHandler.js";
import {navigate} from "../../../../shared/utils/navigation.js";
import {getWorkspaceParticipantDiv} from "../ui/getWorkspaceParticipantDiv.js";
import {boardContainer, participantContainer} from "../dom.js";
import {initDeleteBoardHandler} from "../../shared/events/deleteBoardHandler.js";
import {initCreateBoardSubmitHandler} from "../../shared/events/createBoardHandler.js";
import {initBoardSettingsHandler} from "../../shared/events/boardSettingsHandler.js";
import {initSubscribeHandler} from "../../../shared/events/subscribeHandler.js";


export function initWorkspaceSettingsEvents(workspaceId) {
  initDeleteWorkspaceHandler(() => {
    navigate.toDashboard();
  });
  initDeleteWorkspaceParticipantHandler(workspaceId, (participantId) => {
    const div = document.querySelector(`[data-participant-id="${participantId}"]`)
    if(div) div.remove();
    triggerParticipantsSearch();
  });
  initWorkspaceTabsHandler({workspaceId});
  initWorkspaceParticipantsSearchHandler(workspaceId);
  initAddNewParticipantHandler(workspaceId, async (newParticipant) => {
    const div = await getWorkspaceParticipantDiv(newParticipant);
    participantContainer.appendChild(div);
    triggerParticipantsSearch();
  });
  initSubscribeHandler("workspace", subscribeToWorkspaceAjax, unsubscribeFromWorkspaceAjax);
  initCreateBoardSubmitHandler(boardContainer);
  initDeleteBoardHandler((boardId) => {
    const card = document.querySelector(`[data-board-id="${boardId}"]`);
    if(card) card.remove();
  });
  initBoardSettingsHandler();
  
  delegate("submit", {
    "update-workspace-form": async (form, e) => {
      e.preventDefault();
      const data = Object.fromEntries(new FormData(form).entries());
      // за потреби: приведення типів
      if (data.visibility !== undefined) data.visibility = parseInt(data.visibility, 10);
      try {
        await updateWorkspaceAjax(workspaceId, data);
        alert("Робочий простір оновлено!")
      } catch (err) {
        alert(`Не вдалося оновити робочу область: ${err.message}`);
      }
    },
  });
}