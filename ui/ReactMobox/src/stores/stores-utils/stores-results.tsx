import { HttpStatusCode } from "axios";

export interface StoresResults {
  passed: boolean;
  message?: string;
  statusCode?: HttpStatusCode;
}
