import {initLogsEvents} from "./events/eventsHandler.js";
import {fetchEntityTypes} from "./api/logApi.js";

document.addEventListener("DOMContentLoaded", async () => {
  initLogsEvents();
});

