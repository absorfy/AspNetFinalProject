import {renderCardDiv} from "../../../../shared/ui/cardDivRenderer.js";

export function showCard(card, container) {
  const cardEl = renderCardDiv({
    title: card.title,
    description: card.description ?? "",
    onCardClick: null,
    classes: ["p-1"],
  });

  container.appendChild(cardEl);
}
