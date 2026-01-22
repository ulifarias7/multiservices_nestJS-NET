import { Module } from '@nestjs/common';
import { ConfigModule, ConfigService } from '@nestjs/config';
import { TypeOrmModule } from '@nestjs/typeorm';
import { EmailModule } from './email/email.module';
import { Email } from './email/domain/entities/email.entity';
import { TypeOrmConfig } from './email/infraestructure/persistence/configurations/typeorm.config';


@Module({
  imports: [
    ConfigModule.forRoot({
      isGlobal: true,
      envFilePath: '.env',
    }),
    TypeOrmModule.forRootAsync({
      useFactory: () => TypeOrmConfig.getOrmConfig(),
    }),

    TypeOrmModule.forFeature([Email]),

    EmailModule,
  ],
})
export class AppModule {}