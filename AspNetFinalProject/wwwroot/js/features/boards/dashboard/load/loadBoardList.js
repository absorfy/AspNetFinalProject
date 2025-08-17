import {ContainerState, createContainerState} from "../../../../shared/ui/containerState.js";
import {listSkeleton} from "../../../../shared/ui/skeletons.js";
import {fetchListsByBoard} from "../../../boardLists/api/boardListApi.js";
import {getBoardListDiv} from "../ui/getBoardListDiv.js";
import {loadCardsForList} from "./loadCard.js";

export async function loadBoardLists(boardId, container) {
  const controller = new AbortController();
  const view = createContainerState(container, { skeleton: listSkeleton});

  try {
    view.setState(ContainerState.LOADING);

    const data = await fetchListsByBoard(boardId, controller.signal);

    if(!data || data.length === 0 || data.items?.length === 0) {
      view.setState(ContainerState.EMPTY, { message: "Ще немає дошок"});
      return;
    }

    view.setState(ContainerState.CONTENT, {
      builder: (root) => {
        data.forEach(bl => {
          const div = getBoardListDiv(bl, root);
          root.appendChild(div);
          loadCardsForList(bl.id);
        })
      }
    })
  }
  catch (error) {
    
  }
}