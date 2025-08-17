import {delegate} from "../../../../shared/utils/eventDelegator.js";
import {createCardAjax} from "../../../cards/api/cardApi.js";
import {getCardDiv} from "../ui/getCardDiv.js";


export function initCreateCardHandler() {
  delegate("submit", {
    "create-card-form": async (form, e) => {
      e.preventDefault();
      const data = Object.fromEntries(new FormData(form).entries());
      try {
        const newCard = await createCardAjax(data);
        form.reset();
        bootstrap.Modal.getInstance(form.closest(".modal")).hide();
        const container = document.getElementById(`cards-container-${data.boardListId}`);
        const div = getCardDiv(newCard);
        container.appendChild(div);
      } catch (err) {
        alert(`Не вдалося створити картку: ${err.message}`);
      }
    },
  });
}