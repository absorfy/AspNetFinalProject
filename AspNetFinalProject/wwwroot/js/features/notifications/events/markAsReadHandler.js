import {delegate} from "../../../shared/utils/eventDelegator.js";
import {markAsReadNotification} from "../api/notificationApi.js";
import {toggleAction} from "../../../shared/actions/toggleAction.js";

export function initMarkAsReadHandler() {
  delegate("click", {
    "mark-as-read": (li) => {
      toggleAction({
        btn: li,
        getState: () => false, // завжди read після кліку
        setState: () => li.remove(),
        doAjax: () => markAsReadNotification(li.dataset.id),
      });
    },
  });
}
