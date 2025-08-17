export const navigate = {
  to(url) {
    window.location.href = url;
  },

  reload() {
    window.location.reload();
  },

  back() {
    window.history.back();
  },
  

  toBoardDashboard(id) {
    this.to(`/Boards/${id}/Dashboard`);
  },
  
  toSettingsBoard(id) {
    this.to(`/Boards/${id}/Settings`);
  },

  toSettingsWorkspace(id) {
    this.to(`/Workspaces/${id}/Settings`);
  },
  
  toDashboard() {
    this.to(`/Home/Dashboard`);
  }
};