import {renderCardDiv} from "../../../../shared/ui/cardDivRenderer.js";
import {boardListContainer, createBoardForm, workspaceTitle} from "../dom.js";
import {loadBoardsWithWorkspaceId} from "../../shared/load/loadBoards.js";
import {
  getDeleteButtonConfig,
  getSettingsButtonConfig,
  getSubscribeButtonConfig
} from "../../../../shared/actions/buttonsConfigs.js";
import {createContainerState} from "../../../../shared/ui/containerState.js";
import {listSkeleton} from "../../../../shared/ui/skeletons.js";

export function getWorkspaceDiv(workspace) {
  const card = renderCardDiv({
    title: workspace.title,
    description: workspace.description ?? "",
    meta: [
      `Автор: ${workspace.authorName}`,
      `Учасників: ${workspace.participantsCount}`,
    ],
    attrs: { "data-workspace-id": workspace.id },
    actions: [
      getSettingsButtonConfig({
        targetAction: "settings-workspace",
        targetId: workspace.id,
      }),
      getSubscribeButtonConfig({
        targetAction: "subscribe-workspace",
        targetId: workspace.id,
        isSubscribed: workspace.isSubscribed,
      }),
      getDeleteButtonConfig({
        targetAction: "delete-workspace",
        targetId: workspace.id,
        targetTitle: workspace.title,
      }),
    ],
    onCardClick: async () => {
      workspaceTitle.innerText = `Boards in Workspace ${workspace.title}`;
      createBoardForm.setAttribute("data-workspace-id", workspace.id);
      const controller = new AbortController();
      const view = createContainerState(boardListContainer, { skeleton: listSkeleton });
      loadBoardsWithWorkspaceId(workspace.id, boardListContainer, controller.signal, view);
    },
  });
  return card
}
