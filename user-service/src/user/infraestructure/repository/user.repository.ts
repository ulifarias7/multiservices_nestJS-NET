import { Injectable } from '@nestjs/common';
import { InjectRepository } from '@nestjs/typeorm';
import { Repository } from 'typeorm';
import { User } from '../../domain/entities/user.entity';
import { CreateUserDto } from 'src/user/presentation/dtos/create-user.dto';

@Injectable()
export class UserRepository {
  constructor(
    @InjectRepository(User)
    private readonly repo: Repository<User>,
  ) {}

 async createUser(user: Partial<User>): Promise<User> {
   const newUser = this.repo.create(user);
   return this.repo.save(newUser);
  }

  async findByEmail(email: string): Promise<User | null> {  
    return this.repo.findOne({ where: { email } });
  }
}