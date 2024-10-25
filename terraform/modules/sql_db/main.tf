resource "azurerm_mssql_database" "sql_db" {
  name                             = var.name
  server_id                        = var.server_id
  create_mode                      = "Default"
  sku_name                         = var.sku_name
  max_size_gb                      = var.max_size_gb
  tags                             = var.tags
  lifecycle {
  prevent_destroy                  = true
  }
}
