import { loadWorkspaces } from "./load/loadWorkspaces.js";
import { handleCreateWorkspaceSubmit } from "./form/createWorkspace.js";
import { handleCreateBoardSubmit } from "./form/createBoard.js";
import {confirmDeleteBtn, deleteModal, deleteModalText} from "./dom.js";
import {initDeleteWorkspaceHandlers} from "../shared/events/deleteWorkspaceHandler.js";


document.addEventListener("DOMContentLoaded", () => {
  loadWorkspaces();
  initDeleteWorkspaceHandlers({confirmDeleteBtn, deleteModal, deleteModalText});
  handleCreateWorkspaceSubmit();
  handleCreateBoardSubmit();
});