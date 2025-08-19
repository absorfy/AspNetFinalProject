// shared/ui/paginationController.js
//
// Універсальний контролер пагінації з пошуком і сортуванням.
// Працює з будь-яким бекендом, який повертає форму PagedResult<T>:
// { items, page, pageSize, totalCount, totalPages, hasPrevious, hasNext }

export function createPaginationController({
                                             // DOM
                                             root,                   // HTMLElement контейнера списку (обов’язково)
                                             controlsContainer,      // HTMLElement куди рендерити контролли (за замовч. над root)
                                             controlsPosition = "top", // "top" | "bottom" | "both"
                                             inputId,
                                             controllerId,

                                             // Дані/рендер
                                             fetchPage,              // async (state, abortSignal) => PagedResult<any>
                                             renderItem,             // (item) => HTMLElement
                                             renderSkeleton,         // () => HTMLElement | HTMLElement[]
                                             emptyMessage = "Нічого не знайдено",
                                             errorMessage = "Сталася помилка",

                                             // Стан і опції
                                             initialState = {},      // { page, pageSize, search, sortBy, descending }
                                             pageSizeOptions = [5, 10, 20, 50],
                                             sortOptions = [         // [{ value:"title", text:"За назвою" }, ...]
                                               { value: "", text: "Без сортування" }
                                             ],

                                             // Дрібні опції
                                             searchPlaceholder = "Пошук...",
                                             urlSync = false,        // синхронізувати стан з query-параметрами URL
                                           }) {
  if (!root) throw new Error("paginationController: 'root' is required");
  const state = {
    page: 1,
    pageSize: 10,
    search: "",
    sortBy: "",
    descending: false,
    ...initialState
  };

  // URL <-> state (опційно)
  if (urlSync) {
    const params = new URLSearchParams(location.search);
    state.page = parseInt(params.get("page") || state.page, 10);
    state.pageSize = parseInt(params.get("pageSize") || state.pageSize, 10);
    state.search = params.get("search") ?? state.search;
    state.sortBy = params.get("sortBy") ?? state.sortBy;
    state.descending = (params.get("desc") ?? String(state.descending)) === "true";
  }

  let _last = { totalPages: 1, page: state.page, items: [] };
  
  let _ctrl = null; // AbortController для запиту
  let _destroyed = false;

  // ---------- helpers ----------
  function setUrlFromState() {
    if (!urlSync) return;
    const params = new URLSearchParams(location.search);
    params.set("page", String(state.page));
    params.set("pageSize", String(state.pageSize));
    params.set("search", state.search || "");
    params.set("sortBy", state.sortBy || "");
    params.set("desc", String(state.descending));
    history.replaceState(null, "", "?" + params.toString());
  }

  function clearRoot() { root.innerHTML = ""; }

  function showSkeleton() {
    clearRoot();
    const sk = typeof renderSkeleton === "function" ? renderSkeleton() : null
    if (sk) root.innerHTML = sk;
  }

  function showEmpty(msg = emptyMessage) {
    clearRoot();
    const div = document.createElement("div");
    div.className = "text-muted py-3";
    div.textContent = msg;
    root.appendChild(div);
  }

  function showError(msg = errorMessage) {
    clearRoot();
    const div = document.createElement("div");
    div.className = "text-danger py-3";
    div.textContent = msg;
    root.appendChild(div);
  }

  // ---------- controls ----------
  const controls = buildControls({
    pageGetter: () => state.page,
    totalGetter: () => (_last && typeof _last.totalPages === "number" ? _last.totalPages : 1),
    pageSizeGetter: () => state.pageSize,
    onPageChange: (p) => updateState({ page: p }),
    onPageSizeChange: (ps) => updateState({ pageSize: ps, page: 1 }),
    onSortChange: (sortBy) => updateState({ sortBy, page: 1 }),
    onSortDirToggle: () => updateState({ descending: !state.descending, page: 1 }),
    inputId,
    pageSizeOptions,
    sortOptions,
    searchPlaceholder,
  });

  // Куди рендерити контролли
  const controlsTop = document.createElement("div");
  const controlsBottom = document.createElement("div");
  controlsTop.className = "d-flex flex-wrap align-items-center gap-2 mb-2";
  controlsBottom.className = "d-flex flex-wrap align-items-center gap-2 mt-2";
  controlsTop.id = controllerId;
  controlsBottom.id = controllerId;

  controlsTop.appendChild(controls.search);
  controlsTop.appendChild(controls.sortSelect);
  controlsTop.appendChild(controls.sortDirBtn);
  controlsTop.appendChild(controls.pageSizeSelect);
  controlsTop.appendChild(controls.nav);

  // bottom — навігація і pageSize (за бажанням можна змінити)
  controlsBottom.appendChild(controls.nav.cloneNode(true));

  const _controlsHost = controlsContainer ?? root.parentElement ?? root;
  document.getElementById(controllerId)?.remove();
  if (controlsPosition === "top" || controlsPosition === "both") {
    _controlsHost.insertBefore(controlsTop, root);
  }
  if (controlsPosition === "bottom" || controlsPosition === "both") {
    _controlsHost.insertBefore(controlsBottom, root.nextSibling);
  }
  
  async function load() {
    if (_destroyed) return;
    if (_ctrl) _ctrl.abort();
    _ctrl = new AbortController();

    setUrlFromState();
    showSkeleton();

    try {
      let data = await fetchPage(structuredClone(state), _ctrl.signal);
      _last = data;
      if(!data.items)
        data = data.result
      
      clearRoot();
      if (!data || !Array.isArray(data.items) || data.items.length === 0) {
        showEmpty();
      } else {
        data.items.forEach(item => {
          const el = renderItem(item);
          if (el) root.appendChild(el);
        });
      }

      // оновити пагінаційний текст (X / Y)
      const infoEls = [..._controlsHost.querySelectorAll('[data-pg-info]')];
      infoEls.forEach(el => el.textContent = `${data.page} / ${data.totalPages}`);

      // кнопки
      const prevEls = [..._controlsHost.querySelectorAll('[data-pg-prev]')];
      const nextEls = [..._controlsHost.querySelectorAll('[data-pg-next]')];
      prevEls.forEach(btn => btn.disabled = data.page <= 1);
      nextEls.forEach(btn => btn.disabled = data.page >= data.totalPages);
    } catch (e) {
      if (_ctrl?.signal.aborted) return;
      console.error(e);
      showError();
    }
  }

  function updateState(patch) {
    Object.assign(state, patch);
    // нормалізація
    state.page = Math.max(1, parseInt(state.page || 1, 10));
    state.pageSize = Math.max(1, parseInt(state.pageSize || 10, 10));
    load();
  }

  // публічне API
  const api = {
    refresh: () => load(),
    setState: (patch) => updateState(patch),
    getState: () => structuredClone(state),
    destroy: () => {
      _destroyed = true;
      if (_ctrl) _ctrl.abort();
      // прибрати контролли?
      // controlsTop.remove(); controlsBottom.remove(); // якщо треба
    }
  };

  // 1-й прогін
  load();
  return api;
}

// внутрішній будівник контролів
function buildControls({
                         pageGetter,
                         totalGetter,
                         pageSizeGetter,
                         onPageChange,
                         onPageSizeChange,
                         onSortChange,
                         onSortDirToggle,
                         pageSizeOptions,
                         sortOptions,
                         searchPlaceholder,
                         inputId
                       }) {
  // 🔍 search
  const search = document.createElement("input");
  search.type = "search";
  search.id = inputId
  search.placeholder = searchPlaceholder;
  search.className = "form-control form-control-sm";

  // ↕ sort select
  const sortSelect = document.createElement("select");
  sortSelect.className = "form-select form-select-sm w-auto";
  sortOptions.forEach(o => {
    const opt = document.createElement("option");
    opt.value = o.value;
    opt.textContent = o.text;
    sortSelect.appendChild(opt);
  });
  sortSelect.addEventListener("change", () => onSortChange(sortSelect.value));

  // direction
  const sortDirBtn = document.createElement("button");
  sortDirBtn.type = "button";
  sortDirBtn.className = "btn btn-sm btn-outline-secondary";
  const setDirText = (desc) => sortDirBtn.textContent = desc ? "↓ Спадання" : "↑ Зростання";
  setDirText(false);
  sortDirBtn.addEventListener("click", () => {
    setDirText(!sortDirBtn.textContent.includes("↓"));
    onSortDirToggle();
  });

  // page size
  const pageSizeSelect = document.createElement("select");
  pageSizeSelect.className = "form-select form-select-sm w-auto";
  pageSizeOptions.forEach(ps => {
    const o = document.createElement("option");
    o.value = String(ps);
    o.textContent = String(ps);
    pageSizeSelect.appendChild(o);
  });
  pageSizeSelect.value = String(pageSizeGetter());
  pageSizeSelect.addEventListener("change", () => onPageSizeChange(parseInt(pageSizeSelect.value, 10)));

  // nav
  const nav = document.createElement("div");
  nav.className = "d-inline-flex align-items-center gap-1";

  const prev = document.createElement("button");
  prev.type = "button";
  prev.className = "btn btn-sm btn-outline-primary";
  prev.textContent = "◀";
  prev.setAttribute("data-pg-prev", "1");
  prev.addEventListener("click", () => onPageChange(Math.max(1, pageGetter() - 1)));

  const info = document.createElement("span");
  info.className = "mx-2";
  info.setAttribute("data-pg-info", "1");
  info.textContent = `${pageGetter()} / ${totalGetter()}`;

  const next = document.createElement("button");
  next.type = "button";
  next.className = "btn btn-sm btn-outline-primary";
  next.textContent = "▶";
  next.setAttribute("data-pg-next", "1");
  next.addEventListener("click", () => onPageChange(pageGetter() + 1));

  nav.appendChild(prev);
  nav.appendChild(info);
  nav.appendChild(next);

  return { search, sortSelect, sortDirBtn, pageSizeSelect, nav };
}
