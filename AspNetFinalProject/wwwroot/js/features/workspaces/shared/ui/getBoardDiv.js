import { renderCardDiv } from "../../../../shared/ui/cardDivRenderer.js";
import {
  getDeleteButtonConfig,
  getSettingsButtonConfig,
  getSubscribeButtonConfig
} from "../../../../shared/actions/buttonsConfigs.js";
import {navigate} from "../../../../shared/utils/navigation.js";

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
      }),
      getDeleteButtonConfig({
        targetAction: "delete-board", 
        targetId: board.id, 
        targetTitle: board.title
      }),
      getSubscribeButtonConfig({
        targetAction: "subscribe-board",
        targetId: board.id,
        isSubscribed: board.isSubscribed,
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