import { Controller, Post, Body, Get, Param, HttpStatus } from '@nestjs/common';
import { EmailService } from '../application/email.service';
import { SendEmailDto } from './dto/send-email.dto';
import { EmailResponseDto } from './dto/email-response-dto';
import type { IEmailRepository } from '../domain/entities/repositories/email.repository.interface';
import { Inject } from '@nestjs/common';

@Controller('emails')
export class EmailController {
  constructor(
    private readonly emailService: EmailService,
    @Inject('IEmailRepository')
    private readonly emailRepository: IEmailRepository,
  ) {}

  @Post('send')
  async sendEmail(@Body() sendEmailDto: SendEmailDto) {
    const email = await this.emailService.execute(sendEmailDto);

    return {
      statusCode: HttpStatus.OK,
      message: email.status === 'sent' 
        ? 'Email enviado y guardado exitosamente' 
        : 'Email guardado pero falló el envío',
      data: new EmailResponseDto({
        id: email.id,
        to: email.to,
        subject: email.subject,
        status: email.status,
        sentAt: email.sentAt,
        createdAt: email.createdAt,
      }),
    };
  }
}