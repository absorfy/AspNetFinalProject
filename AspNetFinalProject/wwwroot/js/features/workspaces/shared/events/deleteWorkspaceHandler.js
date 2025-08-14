import {initConfirmDelete} from "../../../../shared/actions/confirmDelete.js";
import {deleteWorkspaceAjax} from "../../api/workspaceApi.js";
import {delegate} from "../../../../shared/utils/eventDelegator.js";


export function initDeleteWorkspaceHandler(onConfirm) {
  const confirmDelete = initConfirmDelete({
    modalElement: document.getElementById("deleteModal"),
    confirmBtn: document.getElementById("confirmDeleteBtn"),
    modalTextElement: document.getElementById("deleteModalText"),
  });

  delegate("click", {
    "delete-workspace": (btn) => {
      confirmDelete({
        title: btn.dataset.title || "цю робочу область",
        onConfirm: async () => {
          await deleteWorkspaceAjax(btn.dataset.id);
          if(typeof onConfirm === "function") {
            onConfirm(btn.dataset.id);
          }
        }
      });
    },
  });
}