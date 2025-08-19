import {renderCardDiv} from "../../../../shared/ui/cardDivRenderer.js";
import {formatDateTime} from "../../../../shared/utils/formatDateTime.js";
import {getDeleteButtonConfig} from "../../../../shared/actions/buttonsConfigs.js";
import {getRoleSelectConfig} from "../../../../shared/actions/selectsConfigs.js";
import {getWorkspaceRoles} from "../main.js";
import {changeWorkspaceParticipantRole} from "../../api/workspaceApi.js";


export function getWorkspaceParticipantDiv(participant) {
  let oldValue = participant.role;
  const ctrl = new AbortController();
  
  return renderCardDiv({
    title: `${participant.username}`,
    meta: [
      `Став учасником: ${formatDateTime(participant.joiningTimestamp)}`,
    ],
    actions: [
      getDeleteButtonConfig({
        targetAction: "delete-participant",
        targetId: participant.userProfileId,
        targetTitle: participant.username,
        hidden: !participant.isChanging,
      }),
      getRoleSelectConfig({
        targetAction: "select-participant-role",
        targetId: participant.userProfileId,
        currentRole: participant.role,
        roles: getWorkspaceRoles(),
        onChange: async (e) => {
          try {
            await changeWorkspaceParticipantRole(participant.workSpaceId, participant.userProfileId, e.target.value, ctrl.signal);
          }
          catch (error) {
            e.target.value = oldValue;
            console.log("Не вдалося змінити роль:", error.message);
          }
          finally {
            oldValue = e.target.value;
          }
        },
        disabled: !participant.isChanging,
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
