export class ApiError extends Error {
  constructor(message, { status, details, url } = {}) {
    super(message);
    this.name = "ApiError";
    this.status = status ?? 0;
    this.details = details;
    this.url = url;
  }
}

const BASE_URL = "/api";
const DEFAULT_HEADERS = {
  Accept: "application/json",
  "Content-Type": "application/json",
};
const DEFAULT_TIMEOUT = 10_000; // мс

function buildUrl(path, query) {
  const origin = window.location.origin;
  const p = String(path).startsWith("/") ? path : `/${path}`;
  const url = `${origin}${BASE_URL}${p}`;
  const u = new URL(url);

  if (query && typeof query === "object") {
    Object.entries(query)
      .filter(([, v]) => v !== undefined && v !== null && v !== "")
      .forEach(([k, v]) => u.searchParams.append(k, String(v)));
  }
  return u.toString();
}

function withTimeout(signal, timeoutMs = DEFAULT_TIMEOUT) {
  const controller = new AbortController();
  const timeoutId = setTimeout(() => controller.abort(new DOMException("Request timeout", "TimeoutError")), timeoutMs);

  const cleanup = () => clearTimeout(timeoutId);

  const onAbort = () => {
    controller.abort(signal.reason);
    cleanup();
  };
  if (signal) {
    if (signal.aborted) onAbort();
    else signal.addEventListener("abort", onAbort, { once: true });
  }
  return { signal: controller.signal, cleanup };
}

async function parseBody(response) {
  if (response.status === 204) return undefined;
  const contentType = response.headers.get("content-type") || "";
  if (contentType.includes("application/json")) {
    return await response.json();
  }
  return await response.text();
}

async function request(path, { method = "GET", query, body, headers, signal, timeout } = {}) {
  const url = buildUrl(path, query);
  const finalHeaders = { ...DEFAULT_HEADERS, ...(headers || {}) };

  // якщо body — об’єкт, серіалізуємо у JSON
  let payload = body;
  if (payload && typeof payload === "object" && !(payload instanceof FormData)) {
    payload = JSON.stringify(payload);
  }
  // якщо FormData — прибираємо Content-Type, хай браузер поставить boundary
  if (payload instanceof FormData) {
    delete finalHeaders["Content-Type"];
  }

  const { signal: timedSignal, cleanup } = withTimeout(signal, timeout);
  let response;
  try {
    response = await fetch(url, {
      method,
      headers: finalHeaders,
      body: payload,
      signal: timedSignal,
      credentials: "same-origin",
    });
  } catch (err) {
    cleanup();
    // network/timeout
    if (err?.name === "AbortError" || err?.name === "TimeoutError") {
      throw new ApiError("Запит перервано або перевищено час очікування.", { status: 0, url });
    }
    throw new ApiError("Проблема з мережею. Спробуйте ще раз.", { status: 0, url, details: err });
  }
  cleanup();

  if (response.redirected) {
    window.location.href = response.url;
    return;
  }
  
  const data = await parseBody(response);
  if (!response.ok) {
    const msg =
      (data && typeof data === "object" && (data.message || data.error)) ||
      (typeof data === "string" && data) ||
      `HTTP ${response.status}`;
    const apiErr = new ApiError(msg, { status: response.status, url, details: data });
    
    if (response.status === 403) {
      // window.dispatchEvent(new CustomEvent("auth:required"));
    }
    throw apiErr;
  }

  return data; // JSON | string | undefined (для 204)
}

// зручні шорткати
export const apiClient = {
  get: (path, body, opts) => request(path, { ...opts, method: "GET", body }),
  post: (path, body, opts) => request(path, { ...opts, method: "POST", body }),
  put: (path, body, opts) => request(path, { ...opts, method: "PUT", body }),
  patch: (path, body, opts) => request(path, { ...opts, method: "PATCH", body }),
  delete: (path, body, opts) => request(path, { ...opts, method: "DELETE", body }), // повертає undefined (void)
};