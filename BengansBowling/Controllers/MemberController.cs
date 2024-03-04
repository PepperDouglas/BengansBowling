using BengansBowling.Models;
using BengansBowling.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BengansBowling.Controllers
{
    public class MemberController
    {
        private readonly IMemberRepository _memberRepository;

        public MemberController(IMemberRepository memberRepository) {
            _memberRepository = memberRepository;
        }

        public async Task AddMember(string name, string address, string telephone) {
            var member = new Member(name, address, telephone);
            await _memberRepository.AddAsync(member);

            Console.WriteLine("Member added successfully.");
        }

        public async Task UpdateMember(int id, string name, string address, string telephone) {
            var member = await _memberRepository.GetByIdAsync(id);
            if (member != null) {
                member.Name = name;
                member.Address = address;
                member.Telephone = telephone;
                await _memberRepository.UpdateAsync(member);

                Console.WriteLine("Member updated successfully.");
            } else {
                Console.WriteLine("Member not found.");
            }
        }

        public async Task DeleteMember(int id) {
            await _memberRepository.DeleteAsync(id);
            Console.WriteLine("Member deleted successfully.");
        }

        public async Task ListMembers() {
            var members = await _memberRepository.GetAllAsync();
            foreach (var member in members) {
                Console.WriteLine($"ID: {member.Id}, Name: {member.Name}, Address: {member.Address}, Telephone: {member.Telephone}");
            }
        }
    }


}
