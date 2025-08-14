import { renderCardDiv } from "../../../../shared/ui/cardDivRenderer.js";
import {getDeleteButtonConfig, getSettingsButtonConfig} from "../../../../shared/actions/buttonsConfigs.js";

export function getBoardDiv(board) {
  const card = renderCardDiv({
    title: board.title,
    description: board.description ?? "",
    meta: [`Учасників: ${board.participantsCount}`],
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
    ],
    onCardClick: () => {
      window.location.href = `/Boards/Dashboard/${board.id}`;
    },
    attrs: {
      "data-board-id": board.id,
    }
  });
  return card;
}