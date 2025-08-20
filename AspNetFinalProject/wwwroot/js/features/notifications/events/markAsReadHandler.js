import {delegate} from "../../../shared/utils/eventDelegator.js";
import {toggleAction} from "../../../shared/actions/toggleAction.js";
import {markAsRead} from "../notificationHub.js";

export function initMarkAsReadHandler() {
  delegate("click", {
    "mark-as-read": (li) => {
      toggleAction({
        btn: li,
        getState: () => false, // завжди read після кліку
        setState: () => {
          document.querySelector(`[data-dividerid="${li.dataset.id}"]`)?.remove();
          li.remove();
        },
        doAjax: () => markAsRead(li.dataset.id),
      });
    },
  });
}
