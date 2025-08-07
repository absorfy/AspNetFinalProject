import { deleteWorkspaceAjax } from "../../../api/workspaces.js";
import {
  confirmDeleteBtn,
  deleteModal
} from "../dom.js";

import {
  getDeleteTargets,
  resetDeleteTargets
} from "../ui/showWorkspace.js";

export function handleDeleteButton() {
  confirmDeleteBtn.addEventListener("click", async () => {
    const { workspaceIdToDelete, divToDelete } = getDeleteTargets();

    if (!workspaceIdToDelete || !divToDelete) return;

    try {
      await deleteWorkspaceAjax(workspaceIdToDelete);
      divToDelete.remove();

      const modal = bootstrap.Modal.getInstance(deleteModal);
      modal.hide();

      resetDeleteTargets();
    } catch (err) {
      alert("Помилка при видаленні: " + err.message);
    }
  });
}
