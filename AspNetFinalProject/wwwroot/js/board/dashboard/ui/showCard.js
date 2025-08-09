export function showCard(card, container) {
  const div = document.createElement("div");
  div.classList.add("border", "p-1", "mb-2", "bg-white");
  div.innerHTML = `
        <strong>${card.title}</strong><br/>
        <small>${card.description ?? ""}</small>
    `;
  container.appendChild(div);
}