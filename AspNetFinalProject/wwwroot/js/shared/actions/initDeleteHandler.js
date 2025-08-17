import {initConfirmDelete} from "./confirmDelete.js";
import {delegate} from "../utils/eventDelegator.js";

export function InitDeleteHandler({ entityName, nonTitleDescription, deleteAjax, onConfirm }) {
  const confirmDelete = initConfirmDelete({
    modalElement: document.getElementById("deleteModal"),
    confirmBtn: document.getElementById("confirmDeleteBtn"),
    modalTextElement: document.getElementById("deleteModalText"),
  });

  delegate("click", {
    [`delete-${entityName}`]: (btn) => {
      confirmDelete({
        title: btn.dataset.title || nonTitleDescription,
        onConfirm: async () => {
          await deleteAjax(btn.dataset.id);
          if(typeof onConfirm === "function") {
            onConfirm(btn.dataset.id);
          }
        }
      });
    },
  });
}