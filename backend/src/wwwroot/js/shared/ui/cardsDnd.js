import {changeListForCard} from "../../features/cards/api/cardApi.js";
import {getCardDiv} from "../../features/boards/dashboard/ui/getCardDiv.js";


export function initCardsDnd(boardRoot) {
  const lists = boardRoot.querySelectorAll("[data-cards]");
  console.log(lists);
  lists.forEach(listEl => initCardsDndForList(listEl));
}

export function initCardsDndForList(listEl) {
  Sortable.create(listEl, {
    group: "cards",                // між списками
    animation: 150,
    draggable: ".card-item",       // що можна тягнути
    handle: ".card-grip",          // за що тягнути (прибери, якщо тягнути за всю картку)
    ghostClass: "drag-ghost",
    chosenClass: "drag-chosen",
    dragClass: "dragging",

    onEnd: async (evt) => {
      const cardEl = evt.item;
      const newListEl = evt.to;
      
      const cardId = cardEl.dataset.cardId;
      const newListId = newListEl.dataset.listId;
      
      const ctrl = new AbortController();
      
      try {
        await changeListForCard(cardId, newListId, evt.newIndex, ctrl.signal)
      } catch (e) {
        evt.from.insertBefore(
          cardEl,
          evt.from.children[evt.oldIndex] || null
        );
      }
    }
  })
}