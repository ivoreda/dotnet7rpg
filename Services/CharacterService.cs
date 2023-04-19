using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dotnet7rpg.Interfaces;

namespace dotnet7rpg.Services
{
    public class CharacterService : ICharacterService
    {
        private readonly IMapper _mapper;
        private readonly DataContext _context;

        public CharacterService(IMapper mapper, DataContext context)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ServiceResponse<List<GetCharacterDto>>> AddCharacter(AddCharacterDto newCharacter)
        {
            var serviceResponse = new ServiceResponse<List<GetCharacterDto>>();
            var character = _mapper.Map<Character>(newCharacter);
            _context.Characters.Add(character);
            await _context.SaveChangesAsync();
            serviceResponse.Data = await _context.Characters.Select(c => _mapper.Map<GetCharacterDto>(c)).ToListAsync();
            serviceResponse.Message = "Character added successfully";
            return serviceResponse;
        }

        public async Task<ServiceResponse<List<GetCharacterDto>>> DeleteCharacter(int Id)
        {
            var serviceResponse = new ServiceResponse<List<GetCharacterDto>>();
            var character = await _context.Characters.FirstOrDefaultAsync(c => c.Id == Id);
            if (character is not null){
                _context.Characters.Remove(character);

                await _context.SaveChangesAsync();

                serviceResponse.Data = await _context.Characters.Select(c => _mapper.Map<GetCharacterDto>(c)).ToListAsync();
                serviceResponse.Message = "Character deleted successfully";
                return serviceResponse;
            } else {
            serviceResponse.Message = "Character not found";
            serviceResponse.Success = false;
            return serviceResponse;
            }

        }

        public async Task<ServiceResponse<List<GetCharacterDto>>> GetAllCharacters()
        {
            var serviceResponse = new ServiceResponse<List<GetCharacterDto>>();
            var dbCharacters = await _context.Characters.ToListAsync();
            serviceResponse.Data =  dbCharacters.Select(c => _mapper.Map<GetCharacterDto>(c)).ToList();
            serviceResponse.Message = "Characters returned successfully";
            return serviceResponse;
        }

        public async Task<ServiceResponse<GetCharacterDto>> GetCharacterById(int Id)
        {
            var serviceResponse = new ServiceResponse<GetCharacterDto>();
            var dbCharacter =  await _context.Characters.FirstOrDefaultAsync(c => c.Id == Id);
            if (dbCharacter is not null){
                serviceResponse.Data = _mapper.Map<GetCharacterDto>(dbCharacter);
                serviceResponse.Message = "Character retrieved successfully";
                return serviceResponse;
            } else
            {
                serviceResponse.Success = false;
                serviceResponse.Message = "Character not found";
                return serviceResponse;
            }

        }

        public async Task<ServiceResponse<GetCharacterDto>> UpdateCharacter(UpdateCharacterDto updatedCharacter)
        {
            var serviceResponse = new ServiceResponse<GetCharacterDto>();
            var dbCharacter = await _context.Characters.FirstOrDefaultAsync(c => c.Id == updatedCharacter.Id);
            if (dbCharacter is not null){
                dbCharacter.Name = updatedCharacter.Name;
                dbCharacter.Class = updatedCharacter.Class;
                dbCharacter.defence = updatedCharacter.defence;
                dbCharacter.HitPoints = updatedCharacter.HitPoints;
                dbCharacter.Intellegence = updatedCharacter.Intellegence;
                dbCharacter.Strength = updatedCharacter.Strength;

                await _context.SaveChangesAsync();

                serviceResponse.Data = _mapper.Map<GetCharacterDto>(dbCharacter);
                serviceResponse.Message = "Character updated successfully";

                return serviceResponse;
            } else{
                serviceResponse.Success = false;
                serviceResponse.Message = "Character not found";
                return serviceResponse;
            }

        }
    }
}