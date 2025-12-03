import { Injectable } from "@nestjs/common";
import { UserRepository } from "../infraestructure/repository/user.repository";
import { CreateUserDto } from "../presentation/dtos/create-user.dto";
import { UserDto } from "../presentation/dtos/user.dto";

@Injectable()
export class UserService {
  constructor(
    private readonly userRepository: UserRepository,
  ) {}

  async register(dto : CreateUserDto) {
  const user = await this.userRepository.findByEmail(dto.email);
  if (user) {
    throw new Error('el usuario ya existe');
  }

  const newUser = await this.userRepository.createUser({
      name: dto.name,
      lastName: dto.lastName,
      email: dto.email,
      cuil: dto.cuil,
      dni: dto.dni
    });
  return newUser;
  }

  async getUser(email: string) {
    const user = await this.userRepository.findByEmail(email);
    if (!user) {
      throw new Error('Usuario no encontrado');
    }
  
    const userdto = new UserDto();
    userdto.name = user.name;
    userdto.lastName = user.lastName;
    userdto.email = user.email;

    return userdto;
  }
}