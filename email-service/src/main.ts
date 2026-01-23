import { NestFactory } from '@nestjs/core';
import { AppModule } from './app.module';
import { Logger, ValidationPipe } from '@nestjs/common';
import { DocumentBuilder, SwaggerModule } from '@nestjs/swagger';

async function bootstrap() {
  const app = await NestFactory.create(AppModule);
  const port = Number(process.env.PORT) || 4000;

    //Swagger Config
  const config = new DocumentBuilder()
    .setTitle('EmailServices.API')
    .setBasePath(`http://localhost:${port}/api`)
    .setDescription('Documentación de la API con Swagger del microservicio de email')
    .setVersion('1.0')
    .build();

  const document = SwaggerModule.createDocument(app, config);
  SwaggerModule.setup('api', app, document);

  await app.listen(port);

  const logger = new Logger('Bootstrap');
  logger.log(`La aplicación está corriendo en: http://localhost:${port}`);
  logger.log(`Swagger: http://localhost:${port}/api`);
 }
bootstrap();
