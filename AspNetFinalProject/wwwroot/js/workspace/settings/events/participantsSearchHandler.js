import {searchNewParticipantsAjax} from "../../../api/workspaces.js";
import {participantContainer, searchParticipantsContainer, searchParticipantsInput} from "../dom.js";
import {showNonWorkspaceParticipants} from "../ui/showWorkspaceParticipant.js";

let timeoutId;

export function handleParticipantSearch(workspaceId) {

  if (!searchParticipantsInput || !searchParticipantsContainer) return;
  
  searchParticipantsInput.addEventListener('input', () => {
    invokeParticipantSearch(workspaceId);
  });
}

export function invokeParticipantSearch(workspaceId) {
  const query = searchParticipantsInput.value.trim();

  clearTimeout(timeoutId);
  timeoutId = setTimeout(async () => {
    if (query.length < 3) {
      searchParticipantsContainer.innerHTML = '';
      return;
    }

    try {
      const users = await searchNewParticipantsAjax(workspaceId, query);
      showNonWorkspaceParticipants(users, searchParticipantsContainer, participantContainer);
    } catch (err) {
      searchParticipantsContainer.innerHTML = `<div class="text-danger">Помилка: ${err.message}</div>`;
    }
  }, 300); // debounce
}