import { Body, Controller, Get, Post, Query } from '@nestjs/common';
import { UserService } from '../application/user.service';
import { CreateUserDto } from './dtos/create-user.dto';
import { IsEmail } from 'class-validator';

@Controller('users')
export class UserController {
  constructor(private readonly userService: UserService) {}

  @Post('register')
   register(@Body() dto: CreateUserDto) {
    return this.userService.register(dto);
  }

  @Get('get-user-by-email')
  getUser(@Query('email') email: string) {
    return this.userService.getUser(email);
  }
}