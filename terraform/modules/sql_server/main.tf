resource "azurerm_mssql_server" "sql_server" {
  name                         = var.name
  resource_group_name          = var.resource_group_name
  location                     = var.location
  version                      = var.sql_version
  administrator_login          = var.admin_login
  administrator_login_password = var.admin_password
  tags                         = var.tags
}