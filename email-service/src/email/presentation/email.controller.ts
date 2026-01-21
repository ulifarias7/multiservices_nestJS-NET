import { Controller, Post, Body, UseFilters } from '@nestjs/common';
import { EmailService } from '../application/email.service';
import { SendEmailDto } from './dto/send-email.dto';
import { EmailExceptionFilter } from './filters/email-exception.filter';

@Controller('api/email')
@UseFilters(EmailExceptionFilter)
export class EmailController {
  constructor(private readonly emailService: EmailService) {}

  @Post('send')
  async sendEmail(@Body() dto: SendEmailDto) {
    const email = await this.emailService.sendEmail(dto);

    return {
      success: true,
      data: {
        id: email.id,
        status: email.status,
        sentAt: email.sentAt,
      },
    };
  }
}