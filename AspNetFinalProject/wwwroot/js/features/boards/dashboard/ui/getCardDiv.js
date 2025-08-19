import {renderCardDiv} from "../../../../shared/ui/cardDivRenderer.js";
import {getDeleteButtonConfig} from "../../../../shared/actions/buttonsConfigs.js";
import {participantRole} from "../../../../shared/data/participantRole.js";

export function getCardDiv(card) {
  const cardEl = renderCardDiv({
    title: card.title,
    description: card.description ?? "",
    onCardClick: null,
    classes: ["p-1", "card-item"],
    attrs: { "data-card-id": card.id },
    actions: [
      getDeleteButtonConfig({
        targetAction: "delete-card",
        targetId: card.id,
        targetTitle: card.title,
        hidden: [participantRole.None, participantRole.Viewer].includes(card.userBoardRole),
      }),
    ]
  });

  const grip = document.createElement("span");
  grip.className = "card-grip me-2";
  grip.textContent = "⋮⋮";
  cardEl.insertBefore(grip, cardEl.firstChild);
  
  return cardEl;
}
