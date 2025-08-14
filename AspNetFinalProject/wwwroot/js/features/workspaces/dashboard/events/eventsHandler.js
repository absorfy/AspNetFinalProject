import {delegate} from "../../../../shared/utils/eventDelegator.js";
import {createWorkspaceAjax} from "../../api/workspaceApi.js";
import {createBoardAjax} from "../../../boards/api/boardApi.js";
import {initDeleteWorkspaceHandler} from "../../shared/events/deleteWorkspaceHandler.js";
import {initDeleteBoardHandler} from "../../shared/events/deleteBoardHandler.js";
import {initWorkspaceSubscribeHandler} from "../../shared/events/workspaceSubscribeHandler.js";
import {getWorkspaceDiv} from "../ui/getWorkspaceDiv.js";
import {boardListContainer, workspaceListContainer} from "../dom.js";
import {getBoardDiv} from "../../shared/ui/getBoardDiv.js";
import {navigate} from "../../../../shared/utils/navigation.js";

export function initWorkspaceDashboardEvents() {
  initDeleteWorkspaceHandler((workspaceId) => {
    const card = document.querySelector(`[data-workspace-id="${workspaceId}"]`)
    if(!card) return;
    card.remove();
  });
  initDeleteBoardHandler();
  initWorkspaceSubscribeHandler();
  
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
    "create-board-form": async (form, e) => {
      e.preventDefault();
      const data = Object.fromEntries(new FormData(form).entries());
      try {
        const newBoard = await createBoardAjax({...data, workspaceId: form.getAttribute("data-workspace-id")});
        form.reset();
        bootstrap.Modal.getInstance(form.closest(".modal"))?.hide();
        boardListContainer.appendChild(getBoardDiv(newBoard));
      } catch (err) {
        alert(`Не вдалося створити дошку: ${err.message}`);
      }
    },
  });
}