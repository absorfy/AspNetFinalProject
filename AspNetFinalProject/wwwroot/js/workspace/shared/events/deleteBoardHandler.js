import {deleteBoardAjax} from "../../../api/boards.js";

let boardIdToDelete = null;
let onDeleteSuccess = null;
let elements = {};

export function initDeleteBoardHandlers({confirmDeleteBtn, deleteModal, deleteModalText}) {
  elements.confirmDeleteBtn = confirmDeleteBtn;
  elements.deleteModal = deleteModal;
  elements.deleteModalText = deleteModalText;
  handleDeleteBoardSubmitButton();
}

export function handleDeleteBoardButton(deleteBtn, board, onSuccess) {
  deleteBtn.addEventListener("click", (e) => {
    e.stopPropagation();
    boardIdToDelete = board.id;
    onDeleteSuccess = onSuccess;
    elements.deleteModalText.textContent = `Ви дійсно хочете видалити робочу область "${board.title}"?`
    new bootstrap.Modal(elements.deleteModal).show();
  })
}

export function handleDeleteBoardSubmitButton() {
  elements.confirmDeleteBtn.addEventListener("click", async () => {
    if (!boardIdToDelete || !onDeleteSuccess) return;

    try {
      await deleteBoardAjax(boardIdToDelete);
      if(typeof onDeleteSuccess === "function") {
        onDeleteSuccess();
      }

      const modal = bootstrap.Modal.getInstance(elements.deleteModal);
      modal.hide();

      boardIdToDelete = null;
      onDeleteSuccess = null;
    } catch (err) {
      alert("Помилка при видаленні: " + err.message);
    }
  });
}