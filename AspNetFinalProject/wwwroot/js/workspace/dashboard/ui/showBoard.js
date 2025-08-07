export function showBoard(board, container) {
  const div = document.createElement("div");
  div.classList.add("border", "p-2", "mb-2");

  div.innerHTML = `
    <strong>${board.title}</strong><br/>
    ${board.description ?? ""}<br/>
    <small>Author: ${board.authorName}</small> | 
    <small>Participants: ${board.participantsCount}</small>
  `;

  div.addEventListener("click", () => {
    window.location.href = `/Boards/Dashboard/${board.id}`;
  });

  container.appendChild(div);
}
