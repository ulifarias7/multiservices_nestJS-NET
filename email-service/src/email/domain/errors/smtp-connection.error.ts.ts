import { DomainError } from "./domain.errors";

export class SmtpConnectionError extends DomainError {
  readonly statusCode = 503;

  constructor(message: string) {
    super(`SMTP connection failed: ${message}`);
  }
}