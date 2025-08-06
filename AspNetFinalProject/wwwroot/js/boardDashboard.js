import { fetchListsByBoard, createBoardListAjax } from "./api/boardlists.js";
import { fetchCardsByList, createCardAjax } from "./api/cards.js";

document.addEventListener("DOMContentLoaded", () => {
  loadBoardLists(boardId);
});

const boardListsContainer = document.getElementById("boardListsContainer");

const createListForm = document.getElementById("createBoardListForm");
const createListModal = bootstrap.Modal.getOrCreateInstance(document.getElementById("createBoardListModal"));
createListForm.addEventListener("submit", async (e) => {
  e.preventDefault(); // не перезавантажуємо сторінку

  const formData = new FormData(createListForm);
  const title = formData.get("title");

  try {
    const newList = await createBoardListAjax({ boardId, title });
    showBoardList(newList);
    createListModal.hide();
    createListForm.reset();
  } catch (err) {
    console.error(err);
    alert("Не вдалося створити список.");
  }
});

// Завантаження списків
async function loadBoardLists(boardId) {
  try {
    boardListsContainer.innerHTML = "Завантаження списків...";
    const lists = await fetchListsByBoard(boardId);
    boardListsContainer.innerHTML = "";

    if (lists.length === 0) {
      boardListsContainer.innerHTML = "<p>Немає списків. Створіть перший!</p>";
      return;
    }

    lists.forEach(list => showBoardList(list));
  } catch (err) {
    console.error(err);
    alert("Не вдалося завантажити списки.");
  }
}

// Відображення списку
function showBoardList(list) {
  const div = document.createElement("div");
  div.classList.add("border", "p-2", "bg-light");
  div.style.minWidth = "250px";

  div.innerHTML = `
        <h5>${list.title}</h5>
        <div class="cards-container mb-2" id="cards-container-${list.id}"></div>
        <button class="btn btn-sm btn-outline-primary" onclick="openCreateCardModal(${list.id})">+ Додати картку</button>
    `;

  boardListsContainer.appendChild(div);
  loadCardsForList(list.id);
}

const createCardForm = document.getElementById("createCardForm");
const createCardModal = bootstrap.Modal.getOrCreateInstance(document.getElementById("createCardModal"));

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

// Завантаження карток для списку
async function loadCardsForList(listId) {
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

// Відображення картки
function showCard(card, container) {
  const div = document.createElement("div");
  div.classList.add("border", "p-1", "mb-2", "bg-white");
  div.innerHTML = `
        <strong>${card.title}</strong><br/>
        <small>${card.description ?? ""}</small>
    `;
  container.appendChild(div);
}

// Відкрити модалку для картки
window.openCreateCardModal = function(listId) {
  document.getElementById("createCardListId").value = listId;
  createCardModal.show();
};