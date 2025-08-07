import { createWorkspaceAjax } from "../../../api/workspaces.js";
import {
  createWorkspaceForm,
  createWorkspaceModal,
  workspaceListContainer
} from "../dom.js";

import { showWorkspace } from "../ui/showWorkspace.js";

export function handleCreateWorkspaceSubmit() {
  createWorkspaceForm.addEventListener("submit", async (e) => {
    e.preventDefault();

    const formData = new FormData(createWorkspaceForm);
    const title = formData.get("title");
    const description = formData.get("description");

    try {
      const newWorkspace = await createWorkspaceAjax({ title, description });
      showWorkspace(newWorkspace, workspaceListContainer);
      createWorkspaceModal.hide();
      createWorkspaceForm.reset();
    } catch (error) {
      console.error(error);
      alert("Не вдалося створити робочий простір.");
    }
  });
}