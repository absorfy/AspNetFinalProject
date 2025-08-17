import {fetchBoardParticipantRoles} from "../api/boardApi.js";
import {initBoardSettingsEvents} from "./events/eventsHandler.js";
import {currentBoard} from "./dom.js";

let roles;

document.addEventListener("DOMContentLoaded", async () => {
  roles = await fetchBoardParticipantRoles();
  initBoardSettingsEvents(currentBoard.id);
});

export function getBoardRoles() {
  return roles
}