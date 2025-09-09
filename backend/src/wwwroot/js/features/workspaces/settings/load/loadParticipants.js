import {fetchWorkspaceParticipantsAjax} from "../../api/workspaceApi.js";
import {getWorkspaceParticipantDiv} from "../ui/getWorkspaceParticipantDiv.js";
import {createPaginationController} from "../../../../shared/ui/paginationController.js";
import {listSkeleton} from "../../../../shared/ui/skeletons.js";
import {bindDebouncedInput} from "../../../../shared/utils/debounceInputs.js";

let ctrl = null;

export function getParticipantPaginationController() {
  return ctrl;
}

export async function loadParticipants(workspaceId, container) {
  if(!container || !workspaceId) return;

  ctrl = createPaginationController({
    root: container,
    controlsPosition: "top",
    async fetchPage(state, signal) {
      // state: { page, pageSize, search, sortBy, descending }
      return await fetchWorkspaceParticipantsAjax(workspaceId, {
        page: state.page,
        pageSize: state.pageSize,
        search: state.search,
        sortBy: state.sortBy,
        descending: state.descending
      }, signal);
    },
    renderItem: getWorkspaceParticipantDiv,
    renderSkeleton: () => listSkeleton,
    inputId: "participantsSearchInput",
    controllerId: "participantPaginationController",
    emptyMessage: "Ще немає учасників",
    errorMessage: "Не вдалося завантажити учасників",
    initialState: { page: 1, pageSize: 10, sortBy: "", descending: true },
    pageSizeOptions: [5, 10, 20, 50],
    sortOptions: [
      { value: "", text: "Без сортування" },
      { value: "username", text: "За ім'ям" },
      { value: "date", text: "За датою" },
    ],
    searchPlaceholder: "Пошук користувачів..."
  });

  bindDebouncedInput({
    element: "#participantsSearchInput",
    delay: 300,
    minLength: 3,
    onChange: (q) => ctrl.setState({ search: q, page: 1 })
  });

  return ctrl;
}
