import {addNewParticipantAjax} from "../../../api/workspaces.js";
import {currentWorkspace} from "../dom.js";

export async function handleAddNewWorkspaceParticipantButton(addButton, user, successCallback) {
  addButton.addEventListener('click', async () => {
    addButton.disabled = true;
    try {
      const participant = await addNewParticipantAjax(currentWorkspace.id, user.identityId);
      if(typeof successCallback === "function") {
        successCallback(participant);
      }
    } catch (e) {
      addButton.disabled = false;
      alert(`Не вдалося додати: ${e.message}`);
    }
  });
}