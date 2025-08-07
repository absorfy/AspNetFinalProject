import { loadWorkspaces } from "./load/loadWorkspaces.js";
import { handleDeleteButton } from "./events/deleteHandler.js";
import { handleCreateWorkspaceSubmit } from "./form/createWorkspace.js";
import { handleCreateBoardSubmit } from "./form/createBoard.js";

document.addEventListener("DOMContentLoaded", () => {
  loadWorkspaces();
  handleDeleteButton();
  handleCreateWorkspaceSubmit();
  handleCreateBoardSubmit();
});