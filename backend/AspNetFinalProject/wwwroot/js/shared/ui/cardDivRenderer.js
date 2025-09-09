/**
 * Рендер універсальної картки
 * @param {Object} opts
 * @param {string} opts.title - заголовок картки
 * @param {string} [opts.description] - опис/підзаголовок
 * @param {Array<string|HTMLElement>} [opts.meta] - додаткова інфа (рядки або DOM)
 * @param {Array<Object>} [opts.actions] - кнопки дій [{ text, icon?, className?, attrs?, onClick? }]
 * @param {Function} [opts.onCardClick] - клік по картці (крім кнопок)
 * @param {string[]} [opts.classes] - додаткові класи картки
 * @param {Object} [opts.attrs] - HTML-атрибути для головного контейнера (наприклад, { "data-id": 5 })
 */
export function renderCardDiv({
                             title,
                             description,
                             meta = [],
                             actions = [],
                             onCardClick,
                             classes = [],
                             attrs = {},
                           }) {
  const card = document.createElement("div");
  card.classList.add("border", "rounded", "p-2", "mb-2", "bg-white", "position-relative", ...classes);
  card.tabIndex = 0;

  if (attrs && typeof attrs === "object") {
    Object.entries(attrs).forEach(([k, v]) => {
      card.setAttribute(k, v.toString());
    })
  }
  
  // Клік по картці, який ігнорує кнопки
  if (onCardClick) {
    card.addEventListener("click", (e) => {
      if (e.target.closest("button, a")) return;
      onCardClick(e);
    });
  }

  // Заголовок
  const titleEl = document.createElement("strong");
  titleEl.textContent = title;
  card.appendChild(titleEl);

  // Опис
  if (description) {
    const descEl = document.createElement("div");
    descEl.classList.add("text-muted");
    descEl.textContent = description;
    card.appendChild(descEl);
  }

  // Meta-інфо
  if (meta.length) {
    const metaEl = document.createElement("div");
    metaEl.classList.add("small", "text-secondary", "mt-1");
    meta.forEach((m, i) => {
      if (i > 0) metaEl.append(" | ");
      if (typeof m === "string") metaEl.append(m);
      else metaEl.appendChild(m);
    });
    card.appendChild(metaEl);
  }

  // Actions (кнопки)
  if (actions.length) {
    const actionsWrap = document.createElement("div");
    actionsWrap.classList.add("mt-2", "d-flex", "flex-wrap", "align-items-center", "gap-2");
    actions.forEach((a) => {
      const type = a.type || "button";

      if (type === "select") {
        const labelWrap = document.createElement("label");
        labelWrap.className = "d-inline-flex align-items-center gap-1 m-0";

        if (a.label) {
          const lbl = document.createElement("span");
          lbl.className = "small text-secondary";
          lbl.textContent = a.label;
          labelWrap.appendChild(lbl);
        }

        const select = document.createElement("select");
        select.className = a.className || "form-select form-select-sm w-auto";

        if (a.attrs) {
          Object.entries(a.attrs).forEach(([k, v]) => select.setAttribute(k, v));
        }

        (a.options || []).forEach(opt => {
          const o = document.createElement("option");
          o.value = String(opt.value);
          o.textContent = opt.text ?? String(opt.value);
          if (opt.selected) o.selected = true;
          if (opt.hidden) o.hidden = true;
          if (opt.attrs) Object.entries(opt.attrs).forEach(([k, v]) => o.setAttribute(k, v));
          select.appendChild(o);
        });

        if (a.onChange) select.addEventListener("change", a.onChange);

        labelWrap.appendChild(select);
        actionsWrap.appendChild(labelWrap);

      } else {
        const btn = document.createElement("button");
        btn.type = "button";
        btn.className = a.className || "btn btn-sm btn-outline-primary";
        if (a.icon) btn.innerHTML = `${a.icon} ${a.text || ""}`;
        else btn.textContent = a.text;
        if (a.attrs) {
          Object.entries(a.attrs).forEach(([k, v]) => btn.setAttribute(k, v));
        }
        if (a.onClick) btn.addEventListener("click", a.onClick);
        actionsWrap.appendChild(btn);
      }
      
    });
    card.appendChild(actionsWrap);
  }

  return card;
}
