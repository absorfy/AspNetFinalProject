import {handleDeleteWorkspaceParticipantButton} from "../../shared/events/deleteWorkspaceParticipantHandler.js";
import {handleAddNewWorkspaceParticipantButton} from "../events/addNewParicipantHandler.js";
import {invokeParticipantSearch} from "../events/participantsSearchHandler.js";

export function showWorkspaceParticipant(participant, container) {
  const div = document.createElement("div");
  div.classList.add("border", "p-2", "mb-2");
  div.innerHTML = `
                <strong>${participant.username}</strong> — ${participant.role}
                <br/><small>Став учасником: ${new Date(participant.joiningTimestamp).toLocaleString("uk-UA")}</small>
                <button class="btn btn-sm btn-danger ms-2 delete-btn" data-id="${participant.userProfileId}" data-name="${participant.username}">🗑</button>
            `;
  const deleteBtn = div.querySelector(".delete-btn");
  handleDeleteWorkspaceParticipantButton(deleteBtn, participant, () => {
    div.remove();
    invokeParticipantSearch(participant.workSpaceId);
  })
  container.appendChild(div);
}

export function showNonWorkspaceParticipants(users, containerNonParticipants, containerParticipants) {
  containerNonParticipants.innerHTML = '';

  if (!users.length) {
    containerNonParticipants.innerHTML = '<div class="text-muted">Нічого не знайдено</div>';
    return;
  }

  users.forEach(u => {
    const div = document.createElement('div');
    div.className = 'd-flex justify-content-between align-items-center border rounded p-2';
    div.innerHTML = `
      <div>
        <strong>${u.username}</strong><br/>
        <small>${u.email}</small>
      </div>
      <button class="btn btn-sm btn-primary" data-id="${u.identityId}">Додати</button>
    `;

    const btn = div.querySelector('button');
    handleAddNewWorkspaceParticipantButton(btn, u, (participant) => {
      div.remove();
      invokeParticipantSearch(participant.workSpaceId);
      showWorkspaceParticipant(participant, containerParticipants);
    })
    containerNonParticipants.appendChild(div);
  });
}