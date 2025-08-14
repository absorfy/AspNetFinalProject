import {delegate} from "../../../../shared/utils/eventDelegator.js";
import {addWorkspaceParticipantAjax} from "../../api/workspaceApi.js";


export function initAddNewParticipantHandler(workspaceId, onClick) {
  delegate("click", {
    "add-participant": async (btn) => {
      const userId = btn.dataset.userid;
      const newParticipant = await addWorkspaceParticipantAjax(workspaceId, { userProfileId: userId });
      if(typeof onClick == "function") {
        onClick(newParticipant);
      }
    },
  });
}
