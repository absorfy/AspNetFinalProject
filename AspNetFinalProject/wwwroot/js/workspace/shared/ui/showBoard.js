import {handleDeleteBoardButton} from "../events/deleteBoardHandler.js";

export function showBoard(board, container) {
  const div = document.createElement("div");
  div.classList.add("border", "p-2", "mb-2");

  div.innerHTML = `
    <div class="d-flex justify-content-between align-items-start">
      <div>
        <strong>${board.title}</strong><br/>
        ${board.description ?? ""}<br/>
        <small>Author: ${board.authorName}</small> | 
        <small>Participants: ${board.participantsCount}</small>
       </div>
       <div class="text-end">
        <button class="btn btn-sm btn-outline-primary settings-btn" data-id="${board.id}">Налаштувати</button>
        <button class="btn btn-sm ${board.isSubscribed ? 'btn-outline-secondary' : 'btn-outline-success'} subscribe-btn" data-id="${board.id}">
          ${board.isSubscribed ? 'Відписатися' : 'Підписатися'}
        </button>
        <button class="btn btn-sm btn-danger ms-2 delete-btn" data-id="${board.id}" data-name="${board.title}">🗑</button>
      </div>
    </div>
  `;

  div.addEventListener("click", () => {
    window.location.href = `/Boards/Dashboard/${board.id}`;
  });
  
  const deleteBtn = div.querySelector(".delete-btn");
  handleDeleteBoardButton(deleteBtn, board, () => {
    div.remove();
  })

  container.appendChild(div);
}
