import { Injectable, Logger } from '@nestjs/common';
import { ConfigService } from '@nestjs/config';
import * as nodemailer from 'nodemailer';
import { Transporter } from 'nodemailer';
import { Email } from '../../domain/entities/email.entity';
import { IEmailProvider } from '../../application/email.service';
import { SmtpConnectionError } from 'src/email/domain/errors/smtp-connection.error.ts';

@Injectable()
export class SmtpProvider implements IEmailProvider {
  private readonly logger = new Logger(SmtpProvider.name);
  private transporter: Transporter;

  constructor(private readonly configService: ConfigService) {
    this.initializeTransporter();
  }

  private initializeTransporter(): void {
    this.transporter = nodemailer.createTransport({
      host: this.configService.get('SMTP_HOST'),
      port: this.configService.get('SMTP_PORT'),
      secure: this.configService.get('SMTP_SECURE') === 'true',
      auth: {
        user: this.configService.get('SMTP_USER'),
        pass: this.configService.get('SMTP_PASS'),
      },
    });
  }

  async send(
    email: Email,
    attachments?: Array<{ filename: string; content: string }>,
  ): Promise<void> {
    try {
      await this.transporter.sendMail({
        from: this.configService.get('MAIL_FROM'),
        to: email.to,
        cc: email.cc,
        bcc: email.bcc,
        subject: email.subject,
        html: email.html,
        text: email.text,
        attachments: attachments?.map((att) => ({
          filename: att.filename,
          content: Buffer.from(att.content, 'base64'),
        })),
      });
    } catch (error) {
      this.logger.error(`SMTP error: ${error.message}`, error.stack);
      throw new SmtpConnectionError(error.message);
    }
  }
}