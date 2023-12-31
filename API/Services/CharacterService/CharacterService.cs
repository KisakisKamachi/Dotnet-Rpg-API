using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.Dtos.Character;
using API.Models;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace API.Services.CharacterService
{
	public class CharacterService : ICharacterService
	{
		private readonly IMapper _mapper;
		
		public DataContext _context { get; }
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
			
			serviceResponse.Data = 
				await _context.Characters.Select(c => _mapper.Map<GetCharacterDto>(c)).ToListAsync();
			return serviceResponse;
		}

		public async Task<ServiceResponse<List<GetCharacterDto>>> GetAllCharacters(int userId)
		{
			var serviceResponse = new ServiceResponse<List<GetCharacterDto>>();
			var dbCharacters = await _context.Characters.Where(c => c.User!.Id == userId).ToListAsync();
			serviceResponse.Data = dbCharacters.Select(c => _mapper.Map<GetCharacterDto>(c)).ToList();
			return serviceResponse;
		}

		public async Task<ServiceResponse<GetCharacterDto>> GetCharacterById(int id)
		{
			var serviceResponse = new ServiceResponse<GetCharacterDto>();
			var dbCharacter = await _context.Characters.FirstOrDefaultAsync(c => c.Id == id);
			serviceResponse.Data = _mapper.Map<GetCharacterDto>(dbCharacter);
			
			return serviceResponse;
		}

		public async Task<ServiceResponse<GetCharacterDto>> UpdateCharacter(UpdateCharacterDto updatedCharacter)
		{
			var serviceResponse = new ServiceResponse<GetCharacterDto>();
			
			try
			{
				var character = 
					await _context.Characters.FirstOrDefaultAsync(c => c.Id == updatedCharacter.Id);
				
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
				
				await _context.SaveChangesAsync();
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
				var character =
					await _context.Characters.FirstOrDefaultAsync(c => c.Id == id);
				
				if (character == null)
				{
					throw new Exception($"Character with Id '{id}' not found ");
				}
				
				_context.Characters.Remove(character);
				
				await _context.SaveChangesAsync();
				
				
				serviceResponse.Data = 
					await _context.Characters.Select(c => _mapper.Map<GetCharacterDto>(c)).ToListAsync();
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