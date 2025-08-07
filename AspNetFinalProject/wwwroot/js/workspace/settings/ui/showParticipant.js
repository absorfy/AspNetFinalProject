export function showParticipant(participant, container) {
  const div = document.createElement("div");
  div.classList.add("border", "p-2", "mb-2");
  div.innerHTML = `
                <strong>${participant.username}</strong> — ${participant.role}
                <br/><small>Став учасником: ${new Date(participant.joiningTimestamp).toLocaleString("uk-UA")}</small>
            `;
  container.appendChild(div);
}