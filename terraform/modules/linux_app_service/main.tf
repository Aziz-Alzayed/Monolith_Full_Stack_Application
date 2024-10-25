resource "azurerm_linux_web_app" "linux_app_service" {
  name                = var.name
  resource_group_name = var.resource_group_name
  location            = var.location
  service_plan_id     = var.service_plan_id

  site_config {
    application_stack {
      docker_image_name            = "${var.docker_image}:${var.docker_image_tag}"
      docker_registry_url          = var.docker_registry_server_url
      docker_registry_username     = var.docker_registry_server_username
      docker_registry_password     = var.docker_registry_server_password
    }
  }

  app_settings = {
    "WEBSITES_ENABLE_APP_SERVICE_STORAGE" = "false"
    "ASPNETCORE_ENVIRONMENT"              = var.ASPNETCORE_ENVIRONMENT
  }

  identity {
    type = "SystemAssigned"
  }

  tags = var.tags
}
