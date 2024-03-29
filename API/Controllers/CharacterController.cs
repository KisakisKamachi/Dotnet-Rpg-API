using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using API.Dtos.Character;
using API.Models;
using API.Services.CharacterService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
	[Authorize]
	[ApiController]
	[Route("api/[controller]")]
	public class CharacterController : ControllerBase
	{
		private readonly ICharacterService _characterService;

		public CharacterController(ICharacterService characterService)
		{
			_characterService = characterService;
		}
		
		[HttpGet("GetAll")]
		public async Task<ActionResult<ServiceResponse<List<GetCharacterDto>>>> Get()
		{
			int userId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)!.Value);
			return Ok(await _characterService.GetAllCharacters());
		}
		
		[HttpGet("{id}")]
		public async Task<ActionResult<ServiceResponse<List<GetCharacterDto>>>> GetSingle(int id)
		{
			return Ok(await _characterService.GetCharacterById(id));
		}
		
		[HttpPost]
		public async Task<ActionResult<ServiceResponse<List<Character>>>> AddCharacter(AddCharacterDto newCharacter)
		{
			return Ok(await _characterService.AddCharacter(newCharacter));
		}
		
		[HttpPut]
		public async Task<ActionResult<ServiceResponse<List<Character>>>> UpdateCharacter(UpdateCharacterDto updatedCharacter)
		{
			var response = await _characterService.UpdateCharacter(updatedCharacter);
			
			if (response.Data == null)
			{
				return NotFound(response);
			}
			
			return Ok(response);
		}
		
		[HttpDelete("{id}")]
		public async Task<ActionResult<ServiceResponse<List<GetCharacterDto>>>> DeleteCharacter(int id)
		{
			var response = await _characterService.DeleteCharacter(id);
			
			if (response.Data == null)
			{
				return NotFound(response);
			}
			
			return Ok(response);
		}
	}
}