import {delegate} from "../../../../shared/utils/eventDelegator.js";
import {createWorkspaceAjax} from "../../api/workspaceApi.js";
import {initDeleteWorkspaceHandler} from "../../shared/events/deleteWorkspaceHandler.js";
import {initDeleteBoardHandler} from "../../shared/events/deleteBoardHandler.js";
import {initWorkspaceSubscribeHandler} from "../../shared/events/workspaceSubscribeHandler.js";
import {getWorkspaceDiv} from "../ui/getWorkspaceDiv.js";
import {boardListContainer, workspaceListContainer} from "../dom.js";
import {navigate} from "../../../../shared/utils/navigation.js";
import {initCreateBoardSubmitHandler} from "../../shared/events/createBoardHandler.js";

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
  initWorkspaceSubscribeHandler();
  initCreateBoardSubmitHandler(boardListContainer);
  
  
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