import {ContainerState, createContainerState} from "../../../../shared/ui/containerState.js";
import {fetchBoardsAjax} from "../../../boards/api/boardApi.js";
import {getBoardDiv} from "../ui/getBoardDiv.js";

export async function loadBoardsWithWorkspaceId(workspaceId, container, signal, view) {
  try {
    view.setState(ContainerState.LOADING);

    const boards = await fetchBoardsAjax(workspaceId, signal);

    if (!boards || boards.length === 0) {
      view.setState(ContainerState.EMPTY, { message: "Жодної дошки" });
      return;
    }

    view.setState(ContainerState.CONTENT, {
      builder: (root) => {
        boards.forEach(board => {
          const div = getBoardDiv(board);
          root.appendChild(div);
        });
      }
    });
  } catch (e) {
    view.setState(ContainerState.ERROR, { message: "Не вдалося завантажити дошки" });
    console.error(e);
  }
}