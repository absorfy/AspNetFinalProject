import {initDeleteWorkspaceHandler} from "../../shared/events/deleteWorkspaceHandler.js";
import {initDeleteWorkspaceParticipantHandler} from "../../shared/events/deleteWorkspaceParticipantHandler.js";
import {initWorkspaceTabsHandler} from "./workspaceTabsHandler.js";
import {initWorkspaceParticipantsSearchHandler, triggerParticipantsSearch} from "./participantsSearchHandler.js";
import {delegate} from "../../../../shared/utils/eventDelegator.js";
import {subscribeToWorkspaceAjax, unsubscribeFromWorkspaceAjax, updateWorkspaceAjax} from "../../api/workspaceApi.js";
import {initAddNewParticipantHandler} from "./addNewParicipantHandler.js";
import {navigate} from "../../../../shared/utils/navigation.js";
import {boardContainer} from "../dom.js";
import {initDeleteBoardHandler} from "../../shared/events/deleteBoardHandler.js";
import {initCreateBoardSubmitHandler} from "../../shared/events/createBoardHandler.js";
import {initBoardSettingsHandler} from "../../shared/events/boardSettingsHandler.js";
import {initSubscribeHandler} from "../../../shared/events/subscribeHandler.js";
import {subscribeToBoardAjax, unsubscribeFromBoardAjax} from "../../../boards/api/boardApi.js";
import {getBoardPaginationController} from "../../shared/load/loadBoards.js";
import {getParticipantPaginationController} from "../load/loadParticipants.js";


export function initWorkspaceSettingsEvents(workspaceId) {
  initDeleteWorkspaceHandler(() => {
    navigate.toDashboard();
  });
  initDeleteWorkspaceParticipantHandler(workspaceId, (participantId) => {
    getParticipantPaginationController().refresh();
    triggerParticipantsSearch();
  });
  initWorkspaceTabsHandler({workspaceId});
  initWorkspaceParticipantsSearchHandler(workspaceId);
  initAddNewParticipantHandler(workspaceId, async (newParticipant) => {
    getParticipantPaginationController().refresh();
    triggerParticipantsSearch();
  });
  initSubscribeHandler("workspace", subscribeToWorkspaceAjax, unsubscribeFromWorkspaceAjax);
  initSubscribeHandler("board", subscribeToBoardAjax, unsubscribeFromBoardAjax)
  initCreateBoardSubmitHandler(boardContainer);
  initDeleteBoardHandler((boardId) => {
    getBoardPaginationController().refresh();
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