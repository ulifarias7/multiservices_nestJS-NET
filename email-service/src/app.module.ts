import { Module } from '@nestjs/common';
import { AppController } from './email/presentation/app.controller';
import { AppService } from './email/application/app.service';

@Module({
  imports: [],
  controllers: [AppController],
  providers: [AppService],
})
export class AppModule {}
