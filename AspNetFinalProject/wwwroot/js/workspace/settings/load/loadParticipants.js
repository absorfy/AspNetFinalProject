import {fetchWorkspaceParticipantsAjax} from "../../../api/workspaces.js";
import {currentWorkspaceId, participantContainer} from "../dom.js";
import {showParticipant} from "../ui/showParticipant.js";

export async function loadParticipants() {
  participantContainer.innerHTML = "Завантаження...";
  
  try {
    const participants = await fetchWorkspaceParticipantsAjax(currentWorkspaceId);

    if (participants.length === 0) {
      participantContainer.innerHTML = "<p>Немає учасників.</p>";
      return;
    }

    participantContainer.innerHTML = "";
    participants.forEach(p => {
      showParticipant(p, participantContainer);
    });
  } catch (err) {
    console.error("Error loading participants:", err);
    participantContainer.innerHTML = "<p>Не вдалося завантажити учасників.</p>";
  }
}