import {fetchWorkspaceParticipantsAjax} from "../../api/workspaceApi.js";
import {ContainerState} from "../../../../shared/ui/containerState.js";
import {getWorkspaceParticipantDiv} from "../ui/getWorkspaceParticipantDiv.js";

export async function loadParticipants(workspaceId, container, signal, view) {
  try {
    const data = await fetchWorkspaceParticipantsAjax(workspaceId, { page: 1, pageSize: 50 }, signal);
    const items = Array.isArray(data) ? data : (data?.items ?? []);

    if (signal?.aborted) return;

    if (!items.length) {
      view.setState(ContainerState.EMPTY, { message: "Немає учасників" });
      return;
    }

    view.setState(ContainerState.CONTENT, {
      builder: (root) => {
        root.innerHTML = "";
        items.forEach(p => root.appendChild(getWorkspaceParticipantDiv(p)));
      }
    });
  } catch (e) {
    if (signal?.aborted) return;
    console.error(e);
    view.setState(ContainerState.ERROR, { message: "Не вдалося завантажити учасників" });
  }
}
