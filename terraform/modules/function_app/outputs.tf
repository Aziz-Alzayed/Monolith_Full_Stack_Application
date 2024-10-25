# Output for the Function App ID
output "function_app_id" {
  description = "The ID of the Function App"
  value       = azurerm_linux_function_app.function_app.id
}

# Output for the Function App default hostname
output "function_app_default_hostname" {
  description = "The default hostname of the Function App"
  value       = azurerm_linux_function_app.function_app.default_hostname
}

# Output for the Function App URL
output "function_app_url" {
  description = "The URL of the Function App"
  value       = "https://${azurerm_linux_function_app.function_app.default_hostname}"
}

# Output for the Function App name
output "function_app_name" {
  description = "The name of the Function App"
  value       = azurerm_linux_function_app.function_app.name
}
