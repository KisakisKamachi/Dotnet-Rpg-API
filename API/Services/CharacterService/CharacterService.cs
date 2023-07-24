using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Dtos.Character;
using API.Models;
using AutoMapper;

namespace API.Services.CharacterService
{
	public class CharacterService : ICharacterService
	{
		private readonly IMapper _mapper;
		
		static List<Character> _characters = new List<Character> 
		{
			new Character(),
			new Character {Id = 1, Name = "Sam"}
		};
		public CharacterService(IMapper mapper)
		{
			_mapper = mapper;
		}
	
		public async Task<ServiceResponse<List<GetCharacterDto>>> AddCharacter(AddCharacterDto newCharacter)
		{
			var serviceResponse = new ServiceResponse<List<GetCharacterDto>>();
			var character = _mapper.Map<Character>(newCharacter);
			character.Id = _characters.Max(c => c.Id) + 1;
			
			_characters.Add(character);
			serviceResponse.Data = _characters.Select(c => _mapper.Map<GetCharacterDto>(c)).ToList();
			return serviceResponse;
		}

		public async Task<ServiceResponse<List<GetCharacterDto>>> GetAllCharacters()
		{
			var serviceResponse = new ServiceResponse<List<GetCharacterDto>>();
			serviceResponse.Data = _characters.Select(c => _mapper.Map<GetCharacterDto>(c)).ToList();
			return serviceResponse;
		}

		public async Task<ServiceResponse<GetCharacterDto>> GetCharacterById(int id)
		{
			var serviceResponse = new ServiceResponse<GetCharacterDto>();
			var character = _characters.FirstOrDefault(c => c.Id == id);
			serviceResponse.Data = _mapper.Map<GetCharacterDto>(character);
			
			return serviceResponse;
		}

		public async Task<ServiceResponse<GetCharacterDto>> UpdateCharacter(UpdateCharacterDto updatedCharacter)
		{
			var serviceResponse = new ServiceResponse<GetCharacterDto>();
			
			try
			{
				var character = _characters.FirstOrDefault(c => c.Id == updatedCharacter.Id);
				
				if (character == null)
				{
					throw new Exception($"Character with Id '{updatedCharacter.Id}' not found ");
				}
				
				character.Name = updatedCharacter.Name;
				character.HitPoints = updatedCharacter.HitPoints;
				character.Strength = updatedCharacter.Strength;
				character.intelligence = updatedCharacter.intelligence;
				character.Defence = updatedCharacter.Defence;
				character.Class = updatedCharacter.Class;
				
				serviceResponse.Data = _mapper.Map<GetCharacterDto>(character);
			}
			catch (Exception ex)
			{
				serviceResponse.Success = false;
				serviceResponse.Message = ex.Message;
			}
			
			
			return serviceResponse;
		}

		public async Task<ServiceResponse<List<GetCharacterDto>>> DeleteCharacter(int id)
		{
			var serviceResponse = new ServiceResponse<List<GetCharacterDto>>();
			
			try
			{
				var character = _characters.First(c => c.Id == id);
				
				if (character == null)
				{
					throw new Exception($"Character with Id '{id}' not found ");
				}
				
					_characters.Remove(character);
				
				
				serviceResponse.Data = _characters.Select(c => _mapper.Map<GetCharacterDto>(c)).ToList();
			}
			catch (Exception ex)
			{
				serviceResponse.Success = false;
				serviceResponse.Message = ex.Message;
			}
			
			
			return serviceResponse;
		}
	}
}