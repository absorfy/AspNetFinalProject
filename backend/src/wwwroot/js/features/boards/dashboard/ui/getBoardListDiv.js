import {renderCardDiv} from "../../../../shared/ui/cardDivRenderer.js";
import {initCardsDndForList} from "../../../../shared/ui/cardsDnd.js";
import {getDeleteButtonConfig} from "../../../../shared/actions/buttonsConfigs.js";
import {participantRole} from "../../../../shared/data/participantRole.js";


export function getBoardListDiv(list) {
  const card = renderCardDiv({
    title: list.title,
    attrs: {
      "data-list-id": list.id,
    },
    actions: [
      {
        text: "+ Додати картку",
        className: "btn btn-sm btn-outline-primary",
        attrs: {
          "data-action": "open-create-card-modal",
          "data-list-id": list.id,
          ...([participantRole.Viewer, participantRole.None].includes(list.userBoardRole) ? { hidden: "hidden" } : {}),
        },
      },
      getDeleteButtonConfig({
        targetAction: "delete-list",
        targetId: list.id,
        targetTitle: list.title,
        hidden: [participantRole.Viewer, participantRole.None].includes(list.userBoardRole),
      }),
    ],
    onCardClick: null,
    classes: ["bg-light"],
  });

  // Додаємо контейнер для карток усередині тіла картки
  const cardsContainer = document.createElement("div");
  cardsContainer.className = "cards-container mb-2";
  cardsContainer.id = `cards-container-${list.id}`;

  cardsContainer.setAttribute("data-cards", "");
  cardsContainer.setAttribute("data-list-id", list.id);
  
  card.insertBefore(cardsContainer, card.querySelector(".mt-2")); // перед кнопками

  if(![participantRole.Viewer, participantRole.None].includes(list.userBoardRole)) {
    initCardsDndForList(cardsContainer);
  }
  return card;
}