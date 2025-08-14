import {delegate} from "../../../../shared/utils/eventDelegator.js";
import {toggleAction} from "../../../../shared/actions/toggleAction.js";
import {subscribeToWorkspaceAjax, unsubscribeFromWorkspaceAjax} from "../../api/workspaceApi.js";


export function initWorkspaceSubscribeHandler() {
  delegate("click", {
    "subscribe-workspace": (btn) => {
      let isSubscribed = btn.dataset.subscribed.toLowerCase() === "true";
      toggleAction({
        btn,
        getState: () => isSubscribed,
        setState: (newState) => {
          isSubscribed = newState;
          btn.dataset.subscribed = String(newState);
          btn.textContent = newState ? "Відписатися" : "Підписатися";
          btn.classList.toggle("btn-success", !newState);
          btn.classList.toggle("btn-danger", newState);
        },
        doAjax: () =>
          isSubscribed
            ? unsubscribeFromWorkspaceAjax(btn.dataset.id)
            : subscribeToWorkspaceAjax(btn.dataset.id),
        loadingText: "Зачекайте...",
      });
    },
  });
}
