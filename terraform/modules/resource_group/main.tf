resource "azurerm_resource_group" "resource_group" {
  name     = var.name
  location = var.location
  tags     = var.tags
}

resource "time_sleep" "wait_after_rg" {
  depends_on = [azurerm_resource_group.resource_group]
  create_duration = var.wait_duration
}