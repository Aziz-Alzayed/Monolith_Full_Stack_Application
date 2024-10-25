export enum AppRoles {
  Admin = "Admin",
  User = "User",
  Super = "Super",
}

export function getAllAppRoles(): string[] {
  return Object.values(AppRoles);
}
