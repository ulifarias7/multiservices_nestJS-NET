import { DomainError } from "./domain.errors";

export class EmailSendError extends DomainError {
  readonly statusCode = 500;

  constructor(message: string) {
    super(`Failed to send email: ${message}`);
  }
}
