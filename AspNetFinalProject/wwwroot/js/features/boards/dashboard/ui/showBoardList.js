import {renderCardDiv} from "../../../../shared/ui/cardDivRenderer.js";


export function showBoardList(list, container) {
  const card = renderCardDiv({
    title: list.title,
    actions: [
      {
        text: "+ Додати картку",
        className: "btn btn-sm btn-outline-primary",
        attrs: {
          "data-action": "open-create-card-modal",
          "data-list-id": list.id,
        },
      },
    ],
    onCardClick: null,
    classes: ["bg-light"],
  });

  // Додаємо контейнер для карток усередині тіла картки
  const cardsContainer = document.createElement("div");
  cardsContainer.className = "cards-container mb-2";
  cardsContainer.id = `cards-container-${list.id}`;
  card.insertBefore(cardsContainer, card.querySelector(".mt-2")); // перед кнопками

  container.appendChild(card);
}