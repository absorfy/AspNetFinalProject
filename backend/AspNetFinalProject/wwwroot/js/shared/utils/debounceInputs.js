/**
 * Універсальна debounce-функція
 * @param {Function} fn - функція, яку треба викликати з відкладенням
 * @param {number} ms - затримка у мс
 * @returns {Function} нова функція, яка дебаунсить виклик fn
 */
export function debounceInput(fn, ms) {
  let t = null;
  return (...args) => {
    clearTimeout(t);
    t = setTimeout(() => fn(...args), ms);
  };
}

export function bindDebouncedInput({
                                     element,           // HTMLElement або CSS-селектор
                                     delay = 300,       // мс
                                     minLength = 0,     // мінімальна довжина, з якої викликаємо onChange
                                     getValue,          // кастомний рідер значення: (el) => any
                                     onChange,          // (value) => void
                                   }) {
  const el = typeof element === "string" ? document.querySelector(element) : element;
  if (!el) throw new Error("bindDebouncedInput: element not found");

  const valReader = typeof getValue === "function"
    ? getValue
    : (node) => ("value" in node ? node.value : "");

  let t = null;
  const handler = () => {
    const v = valReader(el);
    if (minLength > 0 && String(v).trim().length < minLength) {
      onChange(""); // або нічого не робити — залежно від потреб
      return;
    }
    clearTimeout(t);
    t = setTimeout(() => onChange(v), delay);
  };

  el.addEventListener("input", handler);

  return {
    dispose() { el.removeEventListener("input", handler); },
    trigger() { handler(); }
  };
}
