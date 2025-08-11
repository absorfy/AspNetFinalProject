import {fetchWorkspaceParticipantsAjax} from "../../../api/workspaces.js";
import {currentWorkspace, participantContainer} from "../dom.js";
import {showWorkspaceParticipant} from "../ui/showWorkspaceParticipant.js";

export async function loadParticipants() {
  participantContainer.innerHTML = "Завантаження...";
  
  try {
    const participants = await fetchWorkspaceParticipantsAjax(currentWorkspace.id);

    if (participants.length === 0) {
      participantContainer.innerHTML = "<p>Немає учасників.</p>";
      return;
    }

    participantContainer.innerHTML = "";
    participants.forEach(p => {
      showWorkspaceParticipant(p, participantContainer);
    });
  } catch (err) {
    console.error("Error loading participants:", err);
    participantContainer.innerHTML = "<p>Не вдалося завантажити учасників.</p>";
  }
}