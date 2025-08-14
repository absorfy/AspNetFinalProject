import {delegate} from "../../../../shared/utils/eventDelegator.js";
import {initConfirmDelete} from "../../../../shared/actions/confirmDelete.js";
import {deleteBoardAjax} from "../../../boards/api/boardApi.js";

export function initDeleteBoardHandler() {
  const confirmDelete = initConfirmDelete({
    modalElement: document.getElementById("deleteModal"),
    confirmBtn: document.getElementById("confirmDeleteBtn"),
    modalTextElement: document.getElementById("deleteModalText"),
  });

  delegate("click", {
    "delete-board": (btn) => {
      confirmDelete({
        title: btn.dataset.title || "цю дошку",
        onConfirm: () => deleteBoardAjax(btn.dataset.id),
      });
    },
  });
}