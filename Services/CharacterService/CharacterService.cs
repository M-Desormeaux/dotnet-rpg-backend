using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using NetRPG.Data;
using NetRPG.DTOs.Character;
using NetRPG.Models;

namespace NetRPG.Services.CharacterService
{
    public class CharacterService : ICharacterService
    {
        private readonly IMapper _mapper;
        private readonly DataContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CharacterService(IMapper mapper, DataContext context, IHttpContextAccessor httpContextAccessor)
        {
            _mapper = mapper;
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        private int GetUserID() =>
            int.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));

        public async Task<ServiceResponse<List<GetCharacterDTO>>> GetAllCharacters()
        {
            var serviceResponse = new ServiceResponse<List<GetCharacterDTO>>();
            var dbCharacters = await _context.Characters
                .Include(c => c.Weapon)
                    .Include(c => c.Skills)
                .Where(c => c.User.ID == GetUserID()).ToListAsync();
            serviceResponse.Data = dbCharacters.Select(c => _mapper.Map<GetCharacterDTO>(c)).ToList();
            if (serviceResponse.Data == null)
            {
                serviceResponse.Message = "No Characters Found";
                serviceResponse.Success = false;
            }
            else
            {
                serviceResponse.Message = "Characters Loaded";
            }

            return serviceResponse;
        }

        public async Task<ServiceResponse<GetCharacterDTO>> GetCharacterById(int id)
        {
            var serviceResponse = new ServiceResponse<GetCharacterDTO>();
            var dbCharacter =
                await _context.Characters
                    .Include(c => c.Weapon)
                    .Include(c => c.Skills)
                    .FirstOrDefaultAsync(c => c.ID == id && c.User.ID == GetUserID());
            serviceResponse.Data = _mapper.Map<GetCharacterDTO>(dbCharacter);
            if (serviceResponse.Data == null)
            {
                serviceResponse.Message = "No Character Found";
                serviceResponse.Success = false;
            }
            else
            {
                serviceResponse.Message = "Character Loaded";
            }

            return serviceResponse;
        }

        public async Task<ServiceResponse<List<GetCharacterDTO>>> AddCharacter(PostCharacterDTO newCharacter)
        {
            var serviceResponse = new ServiceResponse<List<GetCharacterDTO>>();
            Character character = _mapper.Map<Character>(newCharacter);
            character.User = await _context.Users.FirstOrDefaultAsync(c => c.ID == GetUserID());

            _context.Characters.Add(character);
            await _context.SaveChangesAsync();
            serviceResponse.Data = await _context.Characters
                .Where(c => c.User.ID == GetUserID())
                .Select(c => _mapper.Map<GetCharacterDTO>(c)).ToListAsync();
            serviceResponse.Message = "Character Added";
            return serviceResponse;
        }

        public async Task<ServiceResponse<GetCharacterDTO>> UpdateCharacter(PutCharacterDTO updatedCharacter)
        {
            var serviceResponse = new ServiceResponse<GetCharacterDTO>();
            try
            {
                Character character = await _context.Characters
                    .Include(c => c.User)
                    .FirstOrDefaultAsync(c => c.ID == updatedCharacter.ID);
                if (character.User.ID == GetUserID())
                {
                    character.Name = updatedCharacter.Name;
                    character.HitPoints = updatedCharacter.HitPoints;
                    character.Str = updatedCharacter.Str;
                    character.Con = updatedCharacter.Con;
                    character.Dex = updatedCharacter.Dex;
                    character.Int = updatedCharacter.Int;
                    character.Wis = updatedCharacter.Wis;
                    character.Cha = updatedCharacter.Cha;
                    character.ClassType = updatedCharacter.ClassType;

                    await _context.SaveChangesAsync();

                    serviceResponse.Data = _mapper.Map<GetCharacterDTO>(character);
                    serviceResponse.Message = "Character Updated";
                }
                else
                {
                    serviceResponse.Success = false;
                    serviceResponse.Message = "Characters Not Found";
                }
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }

            return serviceResponse;
        }

        public async Task<ServiceResponse<List<GetCharacterDTO>>> DeleteCharacter(int id)
        {
            var serviceResponse = new ServiceResponse<List<GetCharacterDTO>>();
            try
            {
                Character character =
                    await _context.Characters.FirstOrDefaultAsync(c => c.ID == id && c.User.ID == GetUserID());
                if (character != null)
                {
                    _context.Characters.Remove(character);
                    await _context.SaveChangesAsync();
                    serviceResponse.Data = _context.Characters
                        .Where(c => c.User.ID == GetUserID())
                        .Select(c => _mapper.Map<GetCharacterDTO>(c)).ToList();
                    serviceResponse.Message = "Character Deleted";
                }
                else
                {
                    serviceResponse.Success = false;
                    serviceResponse.Message = "Character Not Found";
                }
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = "Character Not Found";
            }

            return serviceResponse;
        }

        public async Task<ServiceResponse<GetCharacterDTO>> AddCharacterSkill(AddCharacterSkillDTO newCharacterSkill)
        {
            var serviceResponse = new ServiceResponse<GetCharacterDTO>();
            try
            {
                var character = await _context.Characters
                    .Include(c => c.Weapon)
                    .Include(c => c.Skills)
                    .FirstOrDefaultAsync(c => c.ID == newCharacterSkill.CharacterID && c.User.ID == GetUserID());

                if (character == null)
                {
                    serviceResponse.Success = false;
                    serviceResponse.Message = "Character Not Found";
                    return serviceResponse;
                }

                var skill = await _context.Skills.FirstOrDefaultAsync(s => s.ID == newCharacterSkill.SkillID);
                
                if (skill== null)
                {
                    serviceResponse.Success = false;
                    serviceResponse.Message = "Skill Not Found";
                    return serviceResponse;
                }
                
                character.Skills.Add(skill);
                await _context.SaveChangesAsync();
                
                serviceResponse.Data = _mapper.Map<GetCharacterDTO>(character);
                serviceResponse.Message = "Skill Added";

            }
            catch (Exception err)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = err.Message;
            }

            return serviceResponse;
        }
    }
}