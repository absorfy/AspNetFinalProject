
import {getCardDiv} from "../ui/getCardDiv.js";
import {fetchCardsByList} from "../../../cards/api/cardApi.js";


export async function loadCardsForList(listId) {
  try {
    const container = document.getElementById(`cards-container-${listId}`);
    container.innerHTML = "Завантаження...";
    const cards = await fetchCardsByList(listId);
    container.innerHTML = "";

    cards.forEach(card => {
      const div = getCardDiv(card);
      container.appendChild(div);
    });
  } catch (err) {
    console.error(err);
    alert("Не вдалося завантажити картки.");
  }
}