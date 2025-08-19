import { loadWorkspaces } from "./load/loadWorkspaces.js";
import {initWorkspaceDashboardEvents} from "./events/eventsHandler.js";
import {boardListContainer, workspaceListContainer} from "./dom.js";
import {loadBoards} from "../shared/load/loadBoards.js";


document.addEventListener("DOMContentLoaded", async () => {
  initWorkspaceDashboardEvents();
  loadWorkspaces(workspaceListContainer);
  loadBoards(null, boardListContainer);
});