import {removeWorkspaceParticipantAjax} from "../../api/workspaceApi.js";
import {initConfirmDelete} from "../../../../shared/actions/confirmDelete.js";
import {delegate} from "../../../../shared/utils/eventDelegator.js";


export function initDeleteWorkspaceParticipantHandler(workspaceId, onConfirm) {
  const confirmDelete = initConfirmDelete({
    modalElement: document.getElementById("deleteModal"),
    confirmBtn: document.getElementById("confirmDeleteBtn"),
    modalTextElement: document.getElementById("deleteModalText"),
  });

  delegate("click", {
    "delete-participant": (btn) => {
      confirmDelete({
        title: btn.dataset.name || "цього учасника",
        onConfirm: async () => {
          await removeWorkspaceParticipantAjax(workspaceId, btn.dataset.id);
          if(typeof onConfirm === "function") {
            onConfirm(btn.dataset.id);
          }
        }
      });
    },
  });
}
