import {renderCardDiv} from "../../../../shared/ui/cardDivRenderer.js";
import {formatDateTime} from "../../../../shared/utils/formatDateTime.js";

export function getWorkspaceParticipantDiv(participant) {
  return renderCardDiv({
    title: `${participant.username}`,
    meta: [
      `Роль: ${participant.role}`,
      `Став учасником: ${formatDateTime(participant.joiningTimestamp)}`,
    ],
    actions: [
      {
        text: "Видалити",
        className: "btn btn-sm btn-outline-danger",
        attrs: {
          "data-action": "delete-participant",
          "data-userid": participant.userProfileId,
        },
      },
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
