import {loadParticipants} from "../load/loadParticipants.js";
import {ContainerState, createContainerState} from "../../../../shared/ui/containerState.js";
import {delegate} from "../../../../shared/utils/eventDelegator.js";
import {loadBoardsWithWorkspaceId} from "../../shared/load/loadBoards.js";
import {listSkeleton} from "../../../../shared/ui/skeletons.js";
import {participantContainer} from "../dom.js";


let _inited = false;
const _tabControllers = new Map(); // tab => AbortController
const DEFAULT_TAB = "general";

// шукаємо контейнер вкладки за data-атрибутом
function getTabContainer(tab) {
  return document.querySelector(`[data-tab-content="${tab}"]`);
}

function setActiveTabButton(tab) {
  document.querySelectorAll(`[data-action="open-tab"]`).forEach(btn => {
    btn.classList.toggle("active", btn.dataset.tab === tab);
    btn.setAttribute("aria-current", btn.dataset.tab === tab ? "page" : "false");
  });
}

function showOnlyTab(tab) {
  document.querySelectorAll(`[data-tab-content]`).forEach(el => {
    el.classList.toggle("d-none", el.dataset.tabContent !== tab);
  });
}

function abortTab(tab) {
  const ctrl = _tabControllers.get(tab);
  if (ctrl) {
    ctrl.abort();
    _tabControllers.delete(tab);
  }
}

function newController(tab) {
  abortTab(tab);
  const ctrl = new AbortController();
  _tabControllers.set(tab, ctrl);
  return ctrl;
}

async function loadTab(tab, { workspaceId }) {
  const container = getTabContainer(tab);
  if (!container) return;

  // показати лише потрібний таб
  showOnlyTab(tab);
  setActiveTabButton(tab);

  // створюємо view для станів
  let view; //= createContainerState(container, {skeleton: listSkeleton});

  // готуємо контролер запиту для цього табу
  const ctrl = newController(tab);

  // завантаження контенту конкретної вкладки
  try {
    switch (tab) {
      case "participants":
        view = createContainerState(participantContainer, {skeleton: listSkeleton});
        view.setState(ContainerState.LOADING);
        await loadParticipants(workspaceId, container, ctrl.signal, view);
        break;
      case "boards":
        view = createContainerState(container, {skeleton: listSkeleton});
        view.setState(ContainerState.LOADING);
        await loadBoardsWithWorkspaceId(workspaceId, container, ctrl.signal, view);
        break;
      case "general":
      default:
    }
  } catch (e) {
    if (ctrl.signal.aborted) return; // ігноруємо скасоване
    console.error(e);
    if(view)
      view.setState(ContainerState.ERROR, { message: "Не вдалося завантажити вкладку" });
  }
}

function getTabFromHash() {
  const h = (location.hash || "").replace(/^#/, "").trim();
  return h || DEFAULT_TAB;
}

function setHash(tab) {
  if (getTabFromHash() !== tab) history.replaceState(null, "", `#${tab}`);
}

export function initWorkspaceTabsHandler(opts) {
  if (_inited) return; // ідемпотентність
  _inited = true;

  
  const { workspaceId } = opts || {};
  console.log(workspaceId)
  // делегування кліків по табам
  delegate("click", {
    "open-tab": (btn, e) => {
      e.preventDefault();
      const tab = btn.dataset.tab || DEFAULT_TAB;
      setHash(tab);
      loadTab(tab, { workspaceId });
    }
  });

  // реагуємо на зміну hash (якщо треба)
  window.addEventListener("hashchange", () => {
    loadTab(getTabFromHash(), { workspaceId });
  });

  // перший запуск — із hash або за замовчуванням
  const initial = getTabFromHash();
  setHash(initial);
  loadTab(initial, { workspaceId });
}
