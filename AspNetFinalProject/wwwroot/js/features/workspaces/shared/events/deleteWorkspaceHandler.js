import {deleteWorkspaceAjax} from "../../api/workspaceApi.js";
import {InitDeleteHandler} from "../../../../shared/actions/initDeleteHandler.js";


export function initDeleteWorkspaceHandler(onConfirm) {
  InitDeleteHandler({
    entityName: "workspace",
    nonTitleDescription: "цю робочу область",
    deleteAjax: deleteWorkspaceAjax,
    onConfirm
  });
}