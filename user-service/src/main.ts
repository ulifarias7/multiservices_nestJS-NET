import { NestFactory } from '@nestjs/core';
import { AppModule } from './app.module';
import { Logger, ValidationPipe } from '@nestjs/common';
import { DocumentBuilder, SwaggerModule } from '@nestjs/swagger';
import { log } from 'console';

async function bootstrap() {
  const app = await NestFactory.create(AppModule);

  app.useGlobalPipes(
    new ValidationPipe({
      whitelist: true,
      forbidNonWhitelisted: true,
      transform: true,
    }),
  );

  const port = process.env.PORT ?? 3000;
  
  //Swagger Config
  const config = new DocumentBuilder()
    .setTitle('UserServices.API')
    .setBasePath(`http://localhost:${port}/api`)
    .setDescription('Documentación de la API con Swagger del microservicio de user')
    .setVersion('1.0')
    .build();

  const document = SwaggerModule.createDocument(app, config);
  SwaggerModule.setup('api', app, document);

  await app.listen(process.env.PORT ?? 3000);

  const logger = new Logger('Bootstrap');
  logger.log(`La aplicación está corriendo en: http://localhost:${port}`);
  logger.log(`Swagger: http://localhost:${port}/api`);
}
bootstrap();
