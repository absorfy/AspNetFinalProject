import {delegate} from "../../../shared/utils/eventDelegator.js";


export function initLogsEvents() {
  delegate("click", {
    "follow-log-entity": (btn, e) => {
      const entityId = btn.dataset.id;
      const entityType = +btn.dataset.type;
      
    }
  })
}