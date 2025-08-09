export function showBoardList(list, container) {
  const div = document.createElement("div");
  div.classList.add("border", "p-2", "bg-light");
  div.style.minWidth = "250px";

  div.innerHTML = `
        <h5>${list.title}</h5>
        <div class="cards-container mb-2" id="cards-container-${list.id}"></div>
        <button class="btn btn-sm btn-outline-primary" onclick="openCreateCardModal(${list.id})">+ Додати картку</button>
    `;

  container.appendChild(div);
}