import {renderCardDiv} from "../../../../shared/ui/cardDivRenderer.js";
import {getDeleteButtonConfig} from "../../../../shared/actions/buttonsConfigs.js";

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
      }),
    ]
  });

  const grip = document.createElement("span");
  grip.className = "card-grip me-2";
  grip.textContent = "⋮⋮";
  cardEl.insertBefore(grip, cardEl.firstChild);
  
  return cardEl;
}
