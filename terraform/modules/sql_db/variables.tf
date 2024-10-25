variable "name" {
  description = "The name of the SQL database"
}

variable server_id {
  description = "The Id of the server on which to create the database"
}

variable "sku_name"{
  description = "Specifies the name of the SKU used by the database."
}

variable "max_size_gb" {
  description = "The maximum size of the database in gigabytes"
}

variable "tags" {
  type        = map(string)
  description = "A mapping of tags to assign to the resource"
  default     = {}
}
