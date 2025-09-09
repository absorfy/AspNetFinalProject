export function formatDateTime(dateInput, locale = "uk-UA", options) {
  if (!dateInput) return "";

  const date = dateInput instanceof Date ? dateInput : new Date(dateInput);

  // Якщо дата некоректна
  if (isNaN(date.getTime())) return "";

  const defaultOptions = {
    year: "numeric",
    month: "2-digit",
    day: "2-digit",
    hour: "2-digit",
    minute: "2-digit",
  };

  return date.toLocaleString(locale, { ...defaultOptions, ...options });
}