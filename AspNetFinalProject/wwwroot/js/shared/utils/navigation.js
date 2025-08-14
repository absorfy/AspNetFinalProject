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
    this.to(`/Boards/Dashboard/${id}`);
  },

  toSettingsWorkspace(id) {
    this.to(`/Workspaces/${id}/Settings`);
  },
  
  toDashboard() {
    this.to(`/Home/Dashboard`);
  }
};