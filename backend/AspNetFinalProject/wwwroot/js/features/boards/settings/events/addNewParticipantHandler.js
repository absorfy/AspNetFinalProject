import {delegate} from "../../../../shared/utils/eventDelegator.js";
import {addBoardParticipantAjax} from "../../api/boardApi.js";

export function initAddNewParticipantHandler(boardId, onClick) {
  delegate("click", {
    "add-participant": async (btn) => {
      const userId = btn.dataset.userid;
      const newParticipant = await addBoardParticipantAjax(boardId, { userProfileId: userId });
      if(typeof onClick == "function") {
        onClick(newParticipant);
      }
    },
  });
}