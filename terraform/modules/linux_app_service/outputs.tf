output "linux_app_service_id" {
  value = azurerm_linux_web_app.linux_app_service.id
}

output "linux_app_service_default_hostname" {
  value = azurerm_linux_web_app.linux_app_service.default_hostname
}

output "identity_principal_id" {
  value = azurerm_linux_web_app.linux_app_service.identity[0].principal_id
}