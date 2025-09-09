import {apiClient} from "../../../shared/api/apiClient.js";

// GET /api/logs/entity-types
export async function fetchEntityTypes(payload, signal) {
  return await apiClient.get("api/logs/entity-types", payload, { signal });
}