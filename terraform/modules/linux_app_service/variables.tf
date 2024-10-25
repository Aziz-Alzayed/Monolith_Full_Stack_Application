variable "name" {
  description = "The name of the app service"
  type        = string
}

variable "location" {
  description = "The Azure region where the app service should be created"
  type        = string
}

variable "resource_group_name" {
  description = "The name of the resource group in which to create the app service"
  type        = string
}

variable "service_plan_id" {
  description = "The ID of the App Service Plan to be used for this app service"
  type        = string
}

variable "docker_image" {
  description = "The name of the Docker image to use, including the tag. For example, 'myregistry.azurecr.io/myapp:latest'."
  type        = string
}

variable "docker_image_tag" {
  description = "The tag of the Docker image to be used, such as 'latest' or a specific version number."
  type        = string
  default     = "latest"
}

variable "tags" {
  type        = map(string)
  description = "A mapping of tags to assign to the resource"
  default     = {}
}

variable "docker_registry_server_url" {
  description = "The URL of the Docker registry server. For Azure Container Registry, this is the login server name."
  type        = string
}

variable "docker_registry_server_username" {
  description = "The username for the Docker registry server. For Azure Container Registry, this can be the admin username."
  type        = string
}

variable "docker_registry_server_password" {
  description = "The password for the Docker registry server. For Azure Container Registry, this can be the admin password."
  type        = string
  sensitive   = true
}

variable "ASPNETCORE_ENVIRONMENT" {
  description = "Sets the environment for the ASP.NET Core application. Common values are Development, Staging, Production."
  type        = string
  default     = "Production"
}
