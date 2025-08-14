import {renderCardDiv} from "../../../../shared/ui/cardDivRenderer.js";
import {formatDateTime} from "../../../../shared/utils/formatDateTime.js";
import {getDeleteButtonConfig} from "../../../../shared/actions/buttonsConfigs.js";
import {getRoleSelectConfig} from "../../../../shared/actions/selectsConfigs.js";
import {fetchWorkspaceParticipantRoles} from "../../api/workspaceApi.js";

const roles = await fetchWorkspaceParticipantRoles();

export function getWorkspaceParticipantDiv(participant) {
  return renderCardDiv({
    title: `${participant.username}`,
    meta: [
      `Роль: ${participant.role}`,
      `Став учасником: ${formatDateTime(participant.joiningTimestamp)}`,
    ],
    actions: [
      getDeleteButtonConfig({
        targetAction: "delete-participant",
        targetId: participant.userProfileId,
        targetTitle: participant.username
      }),
      getRoleSelectConfig({
        targetAction: "select-participant-role",
        targetId: participant.userProfileId,
        currentRole: participant.role,
        roles
      })
    ],
    onCardClick: null,
    attrs: {
      "data-participant-id": participant.userProfileId,
    }
  });
}

export function getNewWorkspaceParticipantDiv(userProfile) {
  return renderCardDiv({
    title: `${userProfile.username}`,
    actions: [
      {
        text: "Додати",
        className: "btn btn-sm btn-outline-success",
        attrs: {
          "data-action": "add-participant",
          "data-userid": userProfile.identityId,
        }
      }
    ],
    onCardClick: null,
  })
}
