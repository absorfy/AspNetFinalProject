import { boardListContainer, workspaceTitle } from "../dom.js";
import {loadBoardsWithWorkspaceId} from "../../shared/load/loadBoards.js";

let selectedWorkspaceId = null;

export async function selectWorkspace(workspace) {
  workspaceTitle.innerText = `Boards in Workspace ${workspace.title}`;
  selectedWorkspaceId = workspace.id;
  await loadBoardsWithWorkspaceId(workspace.id, boardListContainer);
}

export function getSelectedWorkspaceId() {
  return selectedWorkspaceId;
}
