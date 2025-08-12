import { loadWorkspaces } from "./load/loadWorkspaces.js";
import { handleCreateWorkspaceSubmit } from "./form/createWorkspace.js";
import { handleCreateBoardSubmit } from "./form/createBoard.js";
import {confirmDeleteBtn, deleteModal, deleteModalText} from "./dom.js";
import {initDeleteWorkspaceHandlers} from "../shared/events/deleteWorkspaceHandler.js";
import {initDeleteBoardHandlers} from "../shared/events/deleteBoardHandler.js";


document.addEventListener("DOMContentLoaded", () => {
  initDeleteBoardHandlers({confirmDeleteBtn, deleteModal, deleteModalText});
  initDeleteWorkspaceHandlers({confirmDeleteBtn, deleteModal, deleteModalText});
  loadWorkspaces();
  handleCreateWorkspaceSubmit();
  handleCreateBoardSubmit();
  
});