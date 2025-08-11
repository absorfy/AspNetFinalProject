import { deleteWorkspaceAjax } from "../../../api/workspaces.js";

let workspaceIdToDelete = null;
let onDeleteSuccess = null;
let elements = {};

export function initDeleteWorkspaceHandlers({confirmDeleteBtn, deleteModal, deleteModalText}) {
  elements.confirmDeleteBtn = confirmDeleteBtn;
  elements.deleteModal = deleteModal;
  elements.deleteModalText = deleteModalText;
  handleDeleteWorkspaceSubmitButton();
}

export function handleDeleteWorkspaceButton(deleteBtn, workspace, successCallback) {
  deleteBtn.addEventListener("click", (e) => {
    e.stopPropagation();
    workspaceIdToDelete = workspace.id;
    onDeleteSuccess = successCallback;
    elements.deleteModalText.textContent = `Ви дійсно хочете видалити робочу область "${workspace.title}"?`;
    new bootstrap.Modal(elements.deleteModal).show();
  });
}

export function handleDeleteWorkspaceSubmitButton() {
  elements.confirmDeleteBtn.addEventListener("click", async () => {
    if (!workspaceIdToDelete || !onDeleteSuccess) return;

    try {
      await deleteWorkspaceAjax(workspaceIdToDelete);
      if(typeof onDeleteSuccess === "function") {
        onDeleteSuccess();
      }

      const modal = bootstrap.Modal.getInstance(elements.deleteModal);
      modal.hide();

      workspaceIdToDelete = null;
      onDeleteSuccess = null;
    } catch (err) {
      alert("Помилка при видаленні: " + err.message);
    }
  });
}
