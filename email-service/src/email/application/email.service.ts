import { Injectable,Inject } from '@nestjs/common';
import { Email } from '../domain/entities/email.entity';
import type { IEmailRepository } from '../domain/entities/repositories/email.repository.interface';
import { EmailTempleateService } from './email-template.service';
import * as nodemailer from 'nodemailer';
import { ConfigService } from '@nestjs/config';

@Injectable()
export class EmailService {
 private transporter : nodemailer.transporter;

 constructor(
  @Inject('IEmailRepository')
    private readonly emailRepository: IEmailRepository,
    private readonly templateService: EmailTempleateService,
    private readonly configService: ConfigService,
 ){
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

  // leer este codigo para saber donde ubicar los temopleates o recibirlo directamente
  async execute(emailData: {
    to: string;
    subject: string;
    body?: string;
    templateType?: string;
    templateData?: Record<string, any>;
    sentAt? : Date
  }): Promise<Email> {
    const email = new Email({
      to: emailData.to,
      subject: emailData.subject,
      body: emailData.body || '',
      templeteType: emailData.templateType,
      templateData: emailData.templateData,
      status: 'pending', // cargar los estado en base 
      createdAt: new Date(),
      sentAt: emailData.sentAt
    });

    try {
      let htmlContent: string;
      
      if (emailData.templateType && emailData.templateData) {
        htmlContent = this.templateService.getTemplate(
          emailData.templateType,
          emailData.templateData,
        );
        email.body = htmlContent;
      } else {
        htmlContent = emailData.body || '';
      }

      await this.transporter.sendMail({
        from: this.configService.get('SMTP_FROM'),
        to: email.to,
        subject: email.subject,
        html: htmlContent,
      });

      email.status = 'sent';
      email.createdAt = new Date();
    } catch (error) {
      email.status = 'failed';
      email.errorMessage = error.message;
    }

    const savedEmail = await this.emailRepository.save(email);
    return savedEmail;
  }
}
