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
    });//poner en configuracion 
  }

  // ver si no puedo devolver modelos  y por parametro tambien mandar un modelo 
   async execute(emailData: {
    to: string;
    subject: string;
    htmlContent: string; // ✅ Recibimos el HTML ya formateado desde el microservicio padre
    templateType?: string; // Solo para registro, no para generar template
    templateData?: Record<string, any>; // Solo para registro
  }): Promise<Email> {
    
    const email = new Email({
      to: emailData.to,
      subject: emailData.subject,
      body: emailData.htmlContent,
      templateType: emailData.templateType,
      templateData: emailData.templateData,
      status: 'pending',
      createdAt: new Date(),
    });

    try {
      // ✅ Enviamos el HTML que viene desde el microservicio padre
      await this.transporter.sendMail({
        from: this.configService.get('SMTP_FROM'),
        to: email.to,
        subject: email.subject,
        html: emailData.htmlContent,
      });

      email.status = 'sent';
      email.sentAt = new Date();
    } catch (error) {
      email.status = 'failed';
      email.errorMessage = error.message;
    }

    // Guardamos en la base de datos
    const savedEmail = await this.emailRepository.save(email);
    return savedEmail;
  }
}
