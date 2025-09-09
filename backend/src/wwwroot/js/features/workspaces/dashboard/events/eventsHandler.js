import {delegate} from "../../../../shared/utils/eventDelegator.js";
import {createWorkspaceAjax, subscribeToWorkspaceAjax, unsubscribeFromWorkspaceAjax} from "../../api/workspaceApi.js";
import {initDeleteWorkspaceHandler} from "../../shared/events/deleteWorkspaceHandler.js";
import {initDeleteBoardHandler} from "../../shared/events/deleteBoardHandler.js";
import {boardListContainer} from "../dom.js";
import {navigate} from "../../../../shared/utils/navigation.js";
import {initCreateBoardSubmitHandler} from "../../shared/events/createBoardHandler.js";
import {initBoardSettingsHandler} from "../../shared/events/boardSettingsHandler.js";
import {initSubscribeHandler} from "../../../shared/events/subscribeHandler.js";
import {subscribeToBoardAjax, unsubscribeFromBoardAjax} from "../../../boards/api/boardApi.js";
import {getWorkSpacePaginationController} from "../load/loadWorkspaces.js";
import {getBoardPaginationController} from "../../shared/load/loadBoards.js";

export function initWorkspaceDashboardEvents() {
  initDeleteWorkspaceHandler((workspaceId) => {
    getWorkSpacePaginationController().refresh();
  });
  initDeleteBoardHandler((boardId) => {
    getBoardPaginationController().refresh();
  });
  initSubscribeHandler("workspace", subscribeToWorkspaceAjax, unsubscribeFromWorkspaceAjax);
  initSubscribeHandler("board", subscribeToBoardAjax, unsubscribeFromBoardAjax);
  initCreateBoardSubmitHandler(boardListContainer);
  initBoardSettingsHandler();
  
  delegate("click", {
    "settings-workspace": async (btn) => {
      navigate.toSettingsWorkspace(btn.dataset.id);
    }
  })
  delegate("submit", {
    "create-workspace-form": async (form, e) => {
      e.preventDefault();
      const data = Object.fromEntries(new FormData(form).entries());
      try {
        await createWorkspaceAjax(data);
        form.reset();
        bootstrap.Modal.getInstance(form.closest(".modal"))?.hide();
        getWorkSpacePaginationController().refresh();
      } catch (err) {
        alert(`Не вдалося створити робочу область: ${err.message}`);
      }
    },
  });
}