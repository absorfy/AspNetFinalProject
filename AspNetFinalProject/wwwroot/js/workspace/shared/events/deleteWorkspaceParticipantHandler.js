import {removeParticipantAjax} from "../../../api/workspaces.js";

let workspaceId = null;
let userProfileId = null
let onDeleteSuccess = null;
let elements = {};

export function initDeleteWorkspaceParticipantHandlers({confirmDeleteBtn, deleteModal, deleteModalText}) {
  elements.confirmDeleteBtn = confirmDeleteBtn;
  elements.deleteModal = deleteModal;
  elements.deleteModalText = deleteModalText;
  handleDeleteWorkspaceParticipantSubmitButton();
}

export function handleDeleteWorkspaceParticipantButton(deleteBtn, participant, successCallback) {
  deleteBtn.addEventListener("click", (e) => {
    e.stopPropagation();
    userProfileId = participant.userProfileId;
    workspaceId = participant.workSpaceId;
    onDeleteSuccess = successCallback;
    elements.deleteModalText.textContent = `Ви дійсно хочете видалити учасника "${participant.username}"?`;
    new bootstrap.Modal(elements.deleteModal).show();
  });
}

export function handleDeleteWorkspaceParticipantSubmitButton() {
  elements.confirmDeleteBtn.addEventListener("click", async () => {
    if (!workspaceId || !userProfileId || !onDeleteSuccess) return;

    try {
      await removeParticipantAjax(workspaceId, userProfileId);
      if(typeof onDeleteSuccess === "function") {
        onDeleteSuccess();
      }

      const modal = bootstrap.Modal.getInstance(elements.deleteModal);
      modal.hide();

      userProfileId = null;
      workspaceId = null;
      onDeleteSuccess = null;
    } catch (err) {
      alert("Помилка при видаленні: " + err.message);
    }
  });
}