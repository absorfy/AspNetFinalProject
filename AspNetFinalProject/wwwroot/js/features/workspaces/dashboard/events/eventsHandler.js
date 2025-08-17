import {delegate} from "../../../../shared/utils/eventDelegator.js";
import {createWorkspaceAjax, subscribeToWorkspaceAjax, unsubscribeFromWorkspaceAjax} from "../../api/workspaceApi.js";
import {initDeleteWorkspaceHandler} from "../../shared/events/deleteWorkspaceHandler.js";
import {initDeleteBoardHandler} from "../../shared/events/deleteBoardHandler.js";
import {getWorkspaceDiv} from "../ui/getWorkspaceDiv.js";
import {boardListContainer, workspaceListContainer} from "../dom.js";
import {navigate} from "../../../../shared/utils/navigation.js";
import {initCreateBoardSubmitHandler} from "../../shared/events/createBoardHandler.js";
import {initBoardSettingsHandler} from "../../shared/events/boardSettingsHandler.js";
import {initSubscribeHandler} from "../../../shared/events/subscribeHandler.js";
import {subscribeToBoardAjax, unsubscribeFromBoardAjax} from "../../../boards/api/boardApi.js";

export function initWorkspaceDashboardEvents() {
  initDeleteWorkspaceHandler((workspaceId) => {
    const card = document.querySelector(`[data-workspace-id="${workspaceId}"]`)
    if(!card) return;
    card.remove();
  });
  initDeleteBoardHandler((boardId) => {
    const card = document.querySelector(`[data-board-id="${boardId}"]`);
    if(card) card.remove();
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
        const newWorkspace = await createWorkspaceAjax(data);
        form.reset();
        bootstrap.Modal.getInstance(form.closest(".modal"))?.hide();
        workspaceListContainer.appendChild(getWorkspaceDiv(newWorkspace));
      } catch (err) {
        alert(`Не вдалося створити робочу область: ${err.message}`);
      }
    },
  });
}