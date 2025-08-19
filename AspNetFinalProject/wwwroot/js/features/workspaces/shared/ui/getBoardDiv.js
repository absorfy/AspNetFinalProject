import { renderCardDiv } from "../../../../shared/ui/cardDivRenderer.js";
import {
  getDeleteButtonConfig,
  getSettingsButtonConfig,
  getSubscribeButtonConfig
} from "../../../../shared/actions/buttonsConfigs.js";
import {navigate} from "../../../../shared/utils/navigation.js";
import {participantRole} from "../../../../shared/data/participantRole.js";

export function getBoardDiv(board) {
  const card = renderCardDiv({
    title: board.title,
    description: board.description ?? "",
    meta: [
      `Автор: ${board.authorName}`,
      `Учасників: ${board.participantsCount}`
    ],
    actions: [
      getSettingsButtonConfig({
        targetAction: "settings-board",
        targetId: board.id,
        hidden: !([participantRole.Owner, participantRole.Admin].includes(board.userWorkSpaceRole) ||
          [participantRole.Owner, participantRole.Admin, participantRole.Member].includes(board.userBoardRole)),
      }),
      getDeleteButtonConfig({
        targetAction: "delete-board", 
        targetId: board.id, 
        targetTitle: board.title,
        hidden: ![participantRole.Owner, participantRole.Admin].includes(board.userBoardRole),
      }),
      getSubscribeButtonConfig({
        targetAction: "subscribe-board",
        targetId: board.id,
        isSubscribed: board.isSubscribed,
        hidden: [participantRole.None].includes(board.userBoardRole) &&
          [participantRole.None].includes(board.userWorkSpaceRole),
      })
    ],
    onCardClick: () => {
      navigate.toBoardDashboard(board.id);
    },
    attrs: {
      "data-board-id": board.id,
    }
  });
  return card;
}