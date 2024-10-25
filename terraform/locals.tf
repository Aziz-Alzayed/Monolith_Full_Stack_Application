locals {
  resource_group_name  = "fstd-rg"
  location             = "North Europe"
  static_web_app_name  = "fstd-static-web"
  app_service_name     = "fstd-app-service"
  sql_server_name      = "fstd-sql-server"
  sql_db_name          = "fstd-sql-db"
  storage_account_name = "fstdstorageaccount"
  app_environment      = "Production"
  common_tags = {
    Environment = "Production"
    Project     = "FSTD"
    CostCenter  = "FSTDProdBudget"
    Owner       = "Aziz Alzayed"
    ManagedBy   = "Terraform"
  }


  docker_images = {
    frontend = {
      image = "fstd-frontend"
      tag   = "latest"
    }
    backend = {
      image = "fstd-backend"
      tag   = "latest"
    }
    function_trigger = {
      image = "fstd-trigger-function"
      tag   = "latest"
    }
  }
}