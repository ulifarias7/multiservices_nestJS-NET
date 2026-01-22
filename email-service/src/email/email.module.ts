    import { Module } from '@nestjs/common';
import { TypeOrmModule } from '@nestjs/typeorm';
import { ConfigModule } from '@nestjs/config';
import { EmailController } from './presentation/email.controller';
import { EmailService, IEmailProvider } from './application/email.service';
import { IEmailRepository } from './domain/repositories/email.repository.interface';
import { EmailRepository } from './Infraestructure/persistence/email.repository';
import { EmailOrmEntity } from './infraestructure/persistence/email.orm-entity';
import { SmtpProvider } from './Infraestructure/smtp/smtp.provider';
import smtpConfig from './Infraestructure/config/smtp.config';

@Module({
  imports: [
    ConfigModule.forFeature(smtpConfig),
    TypeOrmModule.forFeature([EmailOrmEntity]),
  ],
  controllers: [EmailController],
  providers: [
    EmailService,
    {
      provide: 'IEmailRepository',
      useClass: EmailRepository,
    },
    {
      provide: 'IEmailProvider',
      useClass: SmtpProvider,
    }
  ],
  exports: [EmailService],
})
export class EmailModule {}