import {renderCardDiv} from "../../../../shared/ui/cardDivRenderer.js";
import {boardListContainer, createBoardButton, createBoardForm, workspaceTitle} from "../dom.js";
import {loadBoards} from "../../shared/load/loadBoards.js";
import {
  getDeleteButtonConfig,
  getSettingsButtonConfig,
  getSubscribeButtonConfig
} from "../../../../shared/actions/buttonsConfigs.js";
import {createContainerState} from "../../../../shared/ui/containerState.js";
import {listSkeleton} from "../../../../shared/ui/skeletons.js";
import {participantRole} from "../../../../shared/data/participantRole.js";

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
        hidden: [participantRole.None, participantRole.Viewer ].includes(workspace.userRole) 
      }),
      getSubscribeButtonConfig({
        targetAction: "subscribe-workspace",
        targetId: workspace.id,
        isSubscribed: workspace.isSubscribed,
        hidden: workspace.userRole == participantRole.None,
      }),
      getDeleteButtonConfig({
        targetAction: "delete-workspace",
        targetId: workspace.id,
        targetTitle: workspace.title,
        hidden: [participantRole.None, participantRole.Viewer ].includes(workspace.userRole),
      }),
    ],
    onCardClick: async () => {
      workspaceTitle.innerText = `Boards in Workspace ${workspace.title}`;
      createBoardForm.setAttribute("data-workspace-id", workspace.id);
      const controller = new AbortController();
      const view = createContainerState(boardListContainer, { skeleton: listSkeleton });
      createBoardButton.hidden = [participantRole.Viewer, participantRole.None].includes(workspace.userRole);
      loadBoards(workspace.id, boardListContainer, controller.signal, view);
    },
  });
  return card
}
