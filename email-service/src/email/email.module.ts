import { Module } from '@nestjs/common';
import { TypeOrmModule } from '@nestjs/typeorm';
import { ConfigModule } from '@nestjs/config';
import { EmailController } from './presentations/email.controller';
import { EmailService } from './application/email.service';
import { EmailTempleateService } from './application/email-template.service';
import { EmailRepository } from './infraestructure/persistence/typeorm/repositories/email.repository';
import { EmailTypeOrmEntity } from './infraestructure/persistence/typeorm/entities/email.typeorm';

@Module({
  imports: [
    TypeOrmModule.forFeature([EmailTypeOrmEntity]),
    ConfigModule,
  ],
  controllers: [EmailController],
  providers: [
    EmailService,
    EmailTempleateService,
    {
      provide: 'IEmailRepository',
      useClass: EmailRepository,
    },
  ],
  exports: [EmailService], // Para que otros microservicios puedan usar el caso de uso
})
export class EmailModule {}