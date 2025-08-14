import {currentWorkspace, workspaceSettingsForm} from "../dom.js.js";
import {updateWorkspaceAjax} from "../../api/workspaceApi.js";

export function handleUpdateWorkspaceSubmit() {
  workspaceSettingsForm.addEventListener("submit", async (e) => {
    e.preventDefault();

    const formData = new FormData(workspaceSettingsForm);
    const data = Object.fromEntries(formData.entries());
    data.visibility = parseInt(data.visibility, 10);
    console.log(data);
    try {
      await updateWorkspaceAjax(currentWorkspace.id, data);
    } catch (err) {
      console.error("Error updating workspace settings:", err);
      alert("Не вдалося оновити налаштування робочого простору.");
      return;
    }
    alert("Зміни збережено успішно!");
  })
}