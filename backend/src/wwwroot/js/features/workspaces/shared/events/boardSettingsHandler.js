import {delegate} from "../../../../shared/utils/eventDelegator.js";
import {navigate} from "../../../../shared/utils/navigation.js";


export function initBoardSettingsHandler() {
  delegate("click", {
    "settings-board": async (btn) => {
      navigate.toSettingsBoard(btn.dataset.id);
    }
  })
}