# Resource Group
module "resource_group" {
  source   = "./modules/resource_group"
  name     = local.resource_group_name
  location = local.location
  tags     = local.common_tags
  wait_duration = "30s"
}

# Storage Account depends on the Resource Group
module "fstd_storage_account" {
  source              = "./modules/storage_account"
  name                = local.storage_account_name
  resource_group_name = module.resource_group.name
  location            = module.resource_group.location
  tags                = local.common_tags
  depends_on          = [module.resource_group]
}

# Container Registry depends on the Resource Group
module "fstd_container_registry" {
  source              = "./modules/container_registry"
  acr_name            = "fstdsacr"
  resource_group_name = module.resource_group.name
  location            = module.resource_group.location
  sku                 = "Basic"
  admin_enabled       = true
  tags                = local.common_tags
  depends_on          = [module.resource_group]
}

# Service Plan depends on the Resource Group
module "fstd_service_plan" {
  source              = "./modules/service_plan"
  name                = "${local.app_service_name}-plan"
  location            = module.resource_group.location
  resource_group_name = module.resource_group.name
  sku_name            = "B1"
  os_type             = "Linux"
  tags                = local.common_tags
  depends_on          = [module.resource_group]
}

# Frontend App Service depends on Service Plan creation
module "fstd_frontend_app_service" {
  source                          = "./modules/linux_app_service"
  name                            = "${local.app_service_name}-frontend"
  resource_group_name             = module.resource_group.name
  location                        = module.resource_group.location
  service_plan_id                 = module.fstd_service_plan.app_service_plan_id
  docker_registry_server_url      = "https://${module.fstd_container_registry.acr_login_server}"
  docker_registry_server_username = module.fstd_container_registry.acr_admin_username
  docker_registry_server_password = module.fstd_container_registry.acr_admin_password
  docker_image                    = local.docker_images.frontend.image
  docker_image_tag                = local.docker_images.frontend.tag
  ASPNETCORE_ENVIRONMENT          = local.app_environment
  tags                            = local.common_tags
  depends_on                      = [module.fstd_service_plan]
}

# Backend App Service depends on Service Plan creation
module "fstd_backend_app_service" {
  source                          = "./modules/linux_app_service"
  name                            = "${local.app_service_name}-backend"
  resource_group_name             = module.resource_group.name
  location                        = module.resource_group.location
  service_plan_id                 = module.fstd_service_plan.app_service_plan_id
  docker_registry_server_url      = "https://${module.fstd_container_registry.acr_login_server}"
  docker_registry_server_username = module.fstd_container_registry.acr_admin_username
  docker_registry_server_password = module.fstd_container_registry.acr_admin_password
  docker_image                    = local.docker_images.backend.image
  docker_image_tag                = local.docker_images.backend.tag
  ASPNETCORE_ENVIRONMENT          = local.app_environment
  tags                            = local.common_tags
  depends_on                      = [module.fstd_service_plan]
}

# Trigger Function App depends on Storage Account and Service Plan
module "fstd_trigger_function_app" {
  source                          = "./modules/function_app"
  name                            = "${local.app_service_name}-trigger-function"
  resource_group_name             = module.resource_group.name
  location                        = module.resource_group.location
  service_plan_id                 = module.fstd_service_plan.app_service_plan_id
  storage_account_name            = module.fstd_storage_account.storage_account_name
  storage_account_access_key      = module.fstd_storage_account.storage_account_access_key
  docker_registry_server_url      = "https://${module.fstd_container_registry.acr_login_server}"
  docker_registry_server_username = module.fstd_container_registry.acr_admin_username
  docker_registry_server_password = module.fstd_container_registry.acr_admin_password
  docker_image                    = local.docker_images.function_trigger.image
  docker_image_tag                = local.docker_images.function_trigger.tag
  tags                            = local.common_tags
  depends_on                      = [module.fstd_service_plan, module.fstd_storage_account]
}

# SQL Server depends on Resource Group
module "fstd_sql_server" {
  source              = "./modules/sql_server"
  name                = local.sql_server_name
  resource_group_name = module.resource_group.name
  location            = module.resource_group.location
  admin_login         = var.sql_server_login
  admin_password      = var.sql_server_login_password
  sql_version         = "12.0"
  tags                = local.common_tags
  depends_on          = [module.resource_group]
}

# SQL DB depends on SQL Server creation
module "fstd_sql_db" {
  source      = "./modules/sql_db"
  name        = "${local.sql_db_name}-dtu"
  server_id   = module.fstd_sql_server.sql_server_id
  sku_name    = "S0"
  max_size_gb = 250
  tags        = local.common_tags
  depends_on  = [module.fstd_sql_server]
}

