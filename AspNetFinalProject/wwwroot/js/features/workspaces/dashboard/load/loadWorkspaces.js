import { listSkeleton } from "../../../../shared/ui/skeletons.js";
import {ContainerState, createContainerState} from "../../../../shared/ui/containerState.js";
import {fetchWorkspacesAjax} from "../../api/workspaceApi.js";
import {getWorkspaceDiv} from "../ui/getWorkspaceDiv.js";

export async function loadWorkspaces(container) {
  const controller = new AbortController();
  const view = createContainerState(container, { skeleton: listSkeleton});

  try {
    view.setState(ContainerState.LOADING);

    const data = await fetchWorkspacesAjax({ page: 1, pageSize: 20 }, controller.signal);
    console.log(data);
    
    if (!data || data.length === 0 || data.items?.length === 0) {
      view.setState(ContainerState.EMPTY, { message: "Ще немає робочих областей" });
      return;
    }
    
    view.setState(ContainerState.CONTENT, {
      builder: (root) => {
        // data.items.forEach(ws => {
        data.forEach(ws => {
          const div = getWorkspaceDiv(ws);
          root.appendChild(div);
        });
        
      }
    });
  } catch (e) {
    view.setState(ContainerState.ERROR, { message: "Не вдалося завантажити робочі області" });
    console.error(e);
  }
}
