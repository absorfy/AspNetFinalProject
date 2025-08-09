import {subscribeToWorkspaceAjax, unsubscribeFromWorkspaceAjax} from "../../../api/workspaces.js";

export function handleSubscribeToggle(subscribeBtn, workspace) {
  subscribeBtn.addEventListener("click", async (e) => {
    e.stopPropagation();

    try {
      if (workspace.isSubscribed) {
        await unsubscribeFromWorkspaceAjax(workspace.id);
        workspace.isSubscribed = false;
        subscribeBtn.classList.replace("btn-outline-secondary", "btn-outline-success");
        subscribeBtn.textContent = "Підписатися";
      } else {
        await subscribeToWorkspaceAjax(workspace.id);
        workspace.isSubscribed = true;
        subscribeBtn.classList.replace("btn-outline-success", "btn-outline-secondary");
        subscribeBtn.textContent = "Відписатися";
      }
    } catch (error) {
      alert("Помилка під час підписки/відписки: " + error.message);
    }
  });
}