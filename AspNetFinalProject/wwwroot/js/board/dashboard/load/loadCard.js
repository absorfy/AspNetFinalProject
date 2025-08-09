import {fetchCardsByList} from "../../../api/cards.js";
import {showCard} from "../ui/showCard.js";


export async function loadCardsForList(listId) {
  try {
    const container = document.getElementById(`cards-container-${listId}`);
    container.innerHTML = "Завантаження...";
    const cards = await fetchCardsByList(listId);
    container.innerHTML = "";

    cards.forEach(card => showCard(card, container));
  } catch (err) {
    console.error(err);
    alert("Не вдалося завантажити картки.");
  }
}