import {removeWorkspaceParticipantAjax} from "../../api/workspaceApi.js";
import {InitDeleteHandler} from "../../../../shared/actions/initDeleteHandler.js";


export function initDeleteWorkspaceParticipantHandler(workspaceId, onConfirm) {
  InitDeleteHandler({
    entityName: "participant",
    nonTitleDescription: "цього учасника",
    deleteAjax: (participantId) => removeWorkspaceParticipantAjax(workspaceId, participantId),
    onConfirm
  });
}
