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

        public void AddMember(string name, string address, string telephone) {
            // Check if the repository is empty to determine the new ID
            int newId = 1;
            if (_memberRepository.GetAll().Any()) {
                newId = _memberRepository.GetAll().Max(m => m.Id) + 1;
            }

            var member = new Member(newId, name, address, telephone);
            _memberRepository.Add(member);

            Console.WriteLine("Member added successfully.");
        }


        public void UpdateMember(int id, string name, string address, string telephone) {
            var member = _memberRepository.GetById(id);
            if (member != null) {
                member.Name = name;
                member.Address = address;
                member.Telephone = telephone;
                _memberRepository.Update(member);

                Console.WriteLine("Member updated successfully.");
            } else {
                Console.WriteLine("Member not found.");
            }
        }

        public void DeleteMember(int id) {
            _memberRepository.Delete(id);
            Console.WriteLine("Member deleted successfully.");
        }

        public void ListMembers() {
            var members = _memberRepository.GetAll();
            foreach (var member in members) {
                Console.WriteLine($"ID: {member.Id}, Name: {member.Name}, Address: {member.Address}, Telephone: {member.Telephone}");
            }
        }
    }

}
