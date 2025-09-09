import { createPaginationController } from "../../../../shared/ui/paginationController.js";
import { bindDebouncedInput } from "../../../../shared/utils/debounceInputs.js";
import { getWorkspaceDiv } from "../ui/getWorkspaceDiv.js";
import { listSkeleton } from "../../../../shared/ui/skeletons.js";
import { fetchWorkspacesAjax } from "../../api/workspaceApi.js";

let ctrl = null;

export function getWorkSpacePaginationController() {
  return ctrl;
}

export function loadWorkspaces(container) {
  if (!container) return;

  ctrl = createPaginationController({
    root: container,
    controlsPosition: "top",
    async fetchPage(state, signal) {
      // state: { page, pageSize, search, sortBy, descending }
      return await fetchWorkspacesAjax({
        page: state.page,
        pageSize: state.pageSize,
        search: state.search,
        sortBy: state.sortBy,
        descending: state.descending
      }, signal);
    },
    renderItem: getWorkspaceDiv,
    renderSkeleton: () => listSkeleton,
    inputId: "workspacesSearchInput",
    controllerId: "workspacesPaginationController",
    emptyMessage: "Ще немає робочих областей",
    errorMessage: "Не вдалося завантажити робочі області",
    initialState: { page: 1, pageSize: 10, sortBy: "", descending: true },
    pageSizeOptions: [5, 10, 20, 50],
    sortOptions: [
      { value: "", text: "Без сортування" },
      { value: "title", text: "За назвою" },
      { value: "date", text: "За датою" },
      { value: "author", text: "За автором" },
    ],
    searchPlaceholder: "Пошук воркспейсів..."
  });

  // 🔍 окремий інпут пошуку з дебаунсом
  bindDebouncedInput({
    element: "#workspacesSearchInput",  // твій <input id="workspacesSearchInput" />
    delay: 300,
    minLength: 3,
    onChange: (q) => ctrl.setState({ search: q, page: 1 })
  });

  return ctrl;
}