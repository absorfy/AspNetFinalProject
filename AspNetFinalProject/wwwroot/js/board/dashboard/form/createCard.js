import {createCardForm, createCardModal} from "../dom.js";
import {createCardAjax} from "../../../api/cards.js";
import {showCard} from "../ui/showCard.js";

export function handleCreateCardSubmit() {
  createCardForm.addEventListener("submit", async (e) => {
    e.preventDefault();

    const formData = new FormData(createCardForm);
    const data = Object.fromEntries(formData.entries());
    data.boardListId = parseInt(data.boardListId);

    try {
      const newCard = await createCardAjax(data);
      const container = document.getElementById(`cards-container-${data.boardListId}`);
      showCard(newCard, container);
      createCardModal.hide();
      createCardForm.reset();
    } catch (err) {
      console.error(err);
      alert("Не вдалося створити картку.");
    }
  });
}

window.openCreateCardModal = function(listId) {
  document.getElementById("createCardListId").value = listId;
  createCardModal.show();
};