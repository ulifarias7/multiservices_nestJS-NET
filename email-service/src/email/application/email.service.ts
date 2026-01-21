import { Injectable, Logger } from '@nestjs/common';
import { Email } from '../domain/entities/email.entity';
import type { IEmailRepository } from '../domain/repositories/email.repository.interface';
import { InvalidEmailError } from '../domain/errors/invalid.errors';
import { EmailSendError } from '../domain/errors/email-send.errors';

export interface SendEmailCommand {
  to: string;
  subject: string;
  html?: string;
  text?: string;
  cc?: string[];
  bcc?: string[];
  attachments?: Array<{ filename: string; content: string }>;
}

export interface IEmailProvider {
  send(email: Email, attachments?: Array<{ filename: string; content: string }>): Promise<void>;
}

@Injectable()
export class EmailService {
  private readonly logger = new Logger(EmailService.name);
  
  constructor(
    private readonly emailRepository: IEmailRepository,
    private readonly emailProvider: IEmailProvider,
  ) {}

  async sendEmail(command: SendEmailCommand): Promise<Email> {
    this.validateEmail(command.to);
    command.cc?.forEach((email) => this.validateEmail(email));
    command.bcc?.forEach((email) => this.validateEmail(email));

    let email = Email.create(command);
    email = await this.emailRepository.save(email);

    try {
      this.logger.log(`Sending email to ${command.to}`);
      await this.emailProvider.send(email, command.attachments);

      email = email.markAsSent();
      await this.emailRepository.save(email);

      this.logger.log(`Email sent successfully: ${email.id}`);
      return email;
    } catch (error) {
      this.logger.error(`Failed to send email: ${error.message}`, error.stack);

      email = email.markAsFailed(error.message);
      await this.emailRepository.save(email);

      throw new EmailSendError(error.message);
    }
  }

  private validateEmail(email: string): void {
    const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    if (!emailRegex.test(email)) {
      throw new InvalidEmailError(email);
    }
  }
}
