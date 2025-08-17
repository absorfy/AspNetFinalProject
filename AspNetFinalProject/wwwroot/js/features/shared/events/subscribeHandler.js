import {delegate} from "../../../shared/utils/eventDelegator.js";
import {toggleAction} from "../../../shared/actions/toggleAction.js";


export function initSubscribeHandler(entityName, subscribe, unsubscribe) {
  if(typeof subscribe !== "function" || typeof unsubscribe !== "function") return;
  delegate("click", {
    [`subscribe-${entityName}`]: (btn) => {
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
            ? unsubscribe(btn.dataset.id)
            : subscribe(btn.dataset.id),
        loadingText: "Зачекайте...",
      });
    },
  });
}
