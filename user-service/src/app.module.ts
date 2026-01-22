import { Module } from '@nestjs/common';
import { TypeOrmModule } from '@nestjs/typeorm';
import { UserService } from './user/application/user.service';
import { UserController } from './user/presentation/user.controller';
import { User } from './user/domain/entities/user.entity';
import { UserRepository } from './user/infraestructure/repository/user.repository';
import { TypeOrmConfig } from './user/infraestructure/configurations/typeorm.config';  
import { ConfigModule } from '@nestjs/config';
import * as fs from 'fs';

@Module({
  imports: [
    ConfigModule.forRoot({
            isGlobal: true,
            envFilePath: process.env.NODE_ENV === 'local'
              ? '.env.local'
              : '.env',
    }),

    TypeOrmModule.forRootAsync({
      useFactory: () => TypeOrmConfig.getOrmConfig(),
    }),

    TypeOrmModule.forFeature([User]),
  ],
  controllers: [UserController],
  providers: [UserService, UserRepository],
  exports: [UserRepository],
})    
export class AppModule {}
