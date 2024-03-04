using BengansBowling.Models;
using BengansBowling.Observers;
using BengansBowling.Singletons;
using BengansBowling.Utilities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BengansBowling.Repositories
{
    public class MemberRepository : IMemberRepository
    {

        private readonly BowlingAlleyContext _context;
        private readonly ApplicationEventPublisher _eventPublisher;

        public MemberRepository(BowlingAlleyContext context) {
            _context = context;

            _eventPublisher = new ApplicationEventPublisher();
            var loggerObserver = new LoggerObserver();
            _eventPublisher.Attach(loggerObserver);
        }

        public async Task<IEnumerable<Member>> GetAllAsync() {
            return await _context.Members.ToListAsync();
        }

        public async Task<Member> GetByIdAsync(int id) {
            return await _context.Members.FindAsync(id);
        }

        public async Task AddAsync(Member member) {
            try {
                await _context.Members.AddAsync(member);
                await _context.SaveChangesAsync();
                _eventPublisher.Notify($"Member added: {member.Name}");

            } catch (Exception ex) {
                Console.WriteLine($"Error occured: {ex.Message}");
            }
        }

        public async Task UpdateAsync(Member member) {
            var existingMember = await _context.Members.FindAsync(member.Id);
            if (existingMember != null) {
                // Map updated values here
                existingMember.Name = member.Name;
                existingMember.Address = member.Address;
                existingMember.Telephone = member.Telephone;
                await _context.SaveChangesAsync();
                _eventPublisher.Notify($"Member updated: {member.Name}");
            }
        }

        public async Task DeleteAsync(int id) {
            var member = await _context.Members.FindAsync(id);
            if (member != null) {
                _context.Members.Remove(member);
                await _context.SaveChangesAsync();
                _eventPublisher.Notify($"Member deleted: {member.Name}");
            }
        }
    }
}
