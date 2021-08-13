using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NetRPG.Data;
using NetRPG.DTOs.Fight;
using NetRPG.Models;

namespace NetRPG.Services.FightService
{
    public class FightService : IFightService
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public FightService(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ServiceResponse<AttackResultDTO>> WeaponAttack(WeaponAttackDTO request)
        {
            var serviceResponse = new ServiceResponse<AttackResultDTO>();
            try
            {
                var attacker = await _context.Characters
                    .Include(c => c.Weapon)
                    .FirstOrDefaultAsync(c => c.ID == request.AttackerID);

                var opponent = await _context.Characters
                    .FirstOrDefaultAsync(c => c.ID == request.OpponentID);

                var damage = DoWeaponDamage(attacker, opponent);

                if (opponent.HitPoints <= 0)
                {
                    serviceResponse.Message = $"{opponent.Name} has been defeated!";
                }

                await _context.SaveChangesAsync();

                serviceResponse.Data = new AttackResultDTO
                {
                    Attacker = attacker.Name,
                    Opponent = opponent.Name,
                    AttackerHP = attacker.HitPoints,
                    OpponentHP = opponent.HitPoints,
                    Damage = damage
                };
            }
            catch (Exception err)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = err.Message;
            }

            return serviceResponse;
        }

        private static int DoWeaponDamage(Character attacker, Character opponent)
        {
            int damage = attacker.Weapon.Damage + (new Random().Next(attacker.Str));
            damage -= new Random().Next(opponent.Con);

            if (damage > 0)
            {
                opponent.HitPoints -= damage;
            }

            return damage;
        }

        public async Task<ServiceResponse<AttackResultDTO>> SkillAttack(SkillAttackDTO request)
        {
            var serviceResponse = new ServiceResponse<AttackResultDTO>();
            try
            {
                var attacker = await _context.Characters
                    .Include(c => c.Skills)
                    .FirstOrDefaultAsync(c => c.ID == request.AttackerID);

                var opponent = await _context.Characters
                    .FirstOrDefaultAsync(c => c.ID == request.OpponentID);

                var skill = attacker.Skills.FirstOrDefault(s => s.ID == request.SkillID);

                if (skill == null)
                {
                    serviceResponse.Success = false;
                    serviceResponse.Message = $"{attacker.Name} does not know this skill";
                    return serviceResponse;
                }

                var damage = DoSkillDamage(skill, attacker, opponent);

                if (opponent.HitPoints <= 0)
                {
                    serviceResponse.Message = $"{opponent.Name} has been defeated!";
                }

                await _context.SaveChangesAsync();

                serviceResponse.Data = new AttackResultDTO
                {
                    Attacker = attacker.Name,
                    Opponent = opponent.Name,
                    AttackerHP = attacker.HitPoints,
                    OpponentHP = opponent.HitPoints,
                    Damage = damage
                };
            }
            catch (Exception err)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = err.Message;
            }

            return serviceResponse;
        }

        private static int DoSkillDamage(Skill? skill, Character attacker, Character opponent)
        {
            int damage = skill.Damage + (new Random().Next(attacker.Str));
            damage -= new Random().Next(opponent.Con);

            if (damage > 0)
            {
                opponent.HitPoints -= damage;
            }

            return damage;
        }

        public async Task<ServiceResponse<FightResultDTO>> Fight(FightRequestDTO request)
        {
            var serviceResponse = new ServiceResponse<FightResultDTO>
            {
                Data = new FightResultDTO()
            };

            try
            {
                var characters = await _context.Characters
                    .Include(c => c.Weapon)
                    .Include(c => c.Skills)
                    .Where(c => request.CharacterIDs.Contains(c.ID)).ToListAsync();

                bool defeated = false;

                while (!defeated)
                {
                    foreach (var attacker in characters)
                    {
                        var opponents = characters.Where(c => c.ID != attacker.ID).ToList();
                        var opponent = opponents[new Random().Next(opponents.Count)];

                        int damage = 0;
                        string attackUsed = string.Empty;

                        bool useWeapon = new Random().Next(2) == 0;
                        if (useWeapon)
                        {
                            attackUsed = attacker.Weapon.Name;
                            damage = DoWeaponDamage(attacker, opponent);
                        }
                        else
                        {
                            var skill = attacker.Skills[new Random().Next(attacker.Skills.Count)];
                            attackUsed = skill.Name;
                            damage = DoSkillDamage(skill, attacker, opponent);
                        }


                        serviceResponse.Data.Log.Add(
                            $"{attacker.Name} attacks {opponent.Name} using {attackUsed} and deals {damage} damage!"
                        );

                        if (opponent.HitPoints<=0)
                        {
                            defeated = true;
                            attacker.Victories++;
                            opponent.Defeats++;
                            serviceResponse.Data.Log.Add(
                                $"{attacker.Name} defeats {opponent.Name} with {attacker.HitPoints}HP remaining!"
                            );
                            break;
                        }
                    }
                }

                characters.ForEach(c =>
                {
                    c.Fights++;
                    c.HitPoints = 100;
                });

                await _context.SaveChangesAsync();
            }
            catch (Exception err)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = err.Message;
            }

            return serviceResponse;
        }

        public async Task<ServiceResponse<List<HighScoreDTO>>> GetHighScore()
        {
            var characters = await _context.Characters
                .Where(c => c.Fights > 0)
                .OrderByDescending(c => c.Victories)
                .ThenBy(c => c.Defeats)
                .ToListAsync();

            var serviceResponse = new ServiceResponse<List<HighScoreDTO>>
            {
                Data = characters.Select(c => _mapper.Map<HighScoreDTO>(c)).ToList()
            };

            return serviceResponse;
        }
    }
}