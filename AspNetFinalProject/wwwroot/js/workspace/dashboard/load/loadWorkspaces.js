import { fetchWorkspacesAjax } from "../../../api/workspaces.js";
import { showWorkspace } from "../ui/showWorkspace.js";
import { workspaceListContainer } from "../dom.js";

export async function loadWorkspaces() {
  try {
    workspaceListContainer.innerHTML = "Завантаження робочих просторів...";
    const workspaces = await fetchWorkspacesAjax();
    workspaceListContainer.innerHTML = "";

    if (workspaces.length === 0) {
      workspaceListContainer.innerHTML = "<p>Жодного робочого простору. Створіть перший!</p>";
      return;
    }

    workspaces.forEach(ws => showWorkspace(ws, workspaceListContainer));
  } catch (error) {
    console.error(error.message);
    workspaceListContainer.innerText = "Не вдалося завантажити робочі простори.";
  }
}
